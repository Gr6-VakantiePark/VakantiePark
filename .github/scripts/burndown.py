import json
import os
import re
import sys
import urllib.request
from datetime import date, datetime, timedelta
from zoneinfo import ZoneInfo

import matplotlib

matplotlib.use("Agg")
import matplotlib.dates as mdates
import matplotlib.pyplot as plt

TIMEZONE = ZoneInfo("Europe/Amsterdam")


def fetch_issues(repository, token):
    issues = []
    url = f"https://api.github.com/repos/{repository}/issues?state=all&per_page=100"
    while url:
        request = urllib.request.Request(url, headers={
            "Authorization": f"Bearer {token}",
            "Accept": "application/vnd.github+json",
        })
        with urllib.request.urlopen(request) as response:
            issues.extend(json.load(response))
            match = re.search(r'<([^>]+)>; rel="next"', response.headers.get("Link", ""))
            url = match.group(1) if match else None
    return [issue for issue in issues if "pull_request" not in issue]


def hours_of(issue):
    lines = [line.strip() for line in (issue.get("body") or "").splitlines()]
    for index, line in enumerate(lines):
        if not line.startswith("|"):
            continue
        headers = [cell.strip().lower() for cell in line.strip("|").split("|")]
        if "uren" not in headers or index + 2 >= len(lines):
            continue
        values = [cell.strip() for cell in lines[index + 2].strip("|").split("|")]
        column = headers.index("uren")
        if column < len(values):
            try:
                return float(values[column].replace(",", "."))
            except ValueError:
                return 0.0
    return 0.0


def local_date(timestamp):
    return datetime.fromisoformat(timestamp.replace("Z", "+00:00")).astimezone(TIMEZONE).date()


def parse_date(value):
    return date.fromisoformat(value) if value else None


def build(issues, today, start, end):
    work = []
    for issue in issues:
        if (issue.get("sub_issues_summary") or {}).get("total", 0) > 0:
            continue
        work.append({
            "hours": hours_of(issue),
            "created": local_date(issue["created_at"]),
            "closed": local_date(issue["closed_at"]) if issue.get("closed_at") else None,
        })

    first = min(item["created"] for item in work)
    start = start or first
    days = [start + timedelta(days=offset) for offset in range((today - start).days + 1)]

    def remaining(day):
        return sum(item["hours"] for item in work
                   if item["created"] <= day and (item["closed"] is None or item["closed"] > day))

    def scope(day):
        return sum(item["hours"] for item in work if item["created"] <= day)

    return {
        "days": days,
        "remaining": [remaining(day) for day in days],
        "scope": [scope(day) for day in days],
        "start": start,
        "end": end,
        "open_items": sum(1 for item in work if item["closed"] is None),
        "unestimated": sum(1 for item in work if item["closed"] is None and item["hours"] == 0),
    }


def plot(data, path):
    days, remaining = data["days"], data["remaining"]
    figure, axes = plt.subplots(figsize=(10, 5.5), dpi=120)
    figure.patch.set_facecolor("#2b2d31")
    axes.set_facecolor("#2b2d31")

    axes.plot(days, data["scope"], color="#80848e", linewidth=1.5, label="Total scope")
    axes.plot(days, remaining, color="#5865f2", linewidth=2.5, label="Remaining hours")
    axes.fill_between(days, remaining, color="#5865f2", alpha=0.15)

    if ideal_today(data) is not None:
        ideal_days = [data["start"] + timedelta(days=offset) for offset in range((data["end"] - data["start"]).days + 1)]
        axes.plot(ideal_days, [ideal_on(data, day) for day in ideal_days],
                  color="#fee75c", linestyle="--", linewidth=1.5, label="Ideal")
        axes.set_xlim(data["start"], max(data["end"], days[-1]))

    axes.annotate(f"{remaining[-1]:g} h", (days[-1], remaining[-1]),
                  textcoords="offset points", xytext=(6, 6), color="#ffffff", fontsize=10)

    axes.set_ylim(bottom=0)
    axes.set_ylabel("Hours", color="#dbdee1")
    axes.xaxis.set_major_formatter(mdates.DateFormatter("%d-%m"))
    axes.tick_params(colors="#dbdee1")
    axes.grid(color="#3f4147", linewidth=0.8)
    for spine in axes.spines.values():
        spine.set_color("#3f4147")
    axes.set_title("Burndown — all issues", color="#ffffff", fontsize=14, loc="left")
    legend = axes.legend(facecolor="#313338", edgecolor="#3f4147", loc="upper right")
    for text in legend.get_texts():
        text.set_color("#dbdee1")

    figure.autofmt_xdate()
    figure.tight_layout()
    figure.savefig(path, facecolor=figure.get_facecolor())


def workdays(first, last):
    return sum(1 for offset in range((last - first).days) if (first + timedelta(days=offset)).weekday() < 5)


def ideal_on(data, day):
    total = workdays(data["start"], data["end"])
    passed = min(workdays(data["start"], min(day, data["end"])), total)
    return data["scope"][-1] * (1 - passed / total)


def ideal_today(data):
    if not data["end"] or data["end"] <= data["start"] or workdays(data["start"], data["end"]) == 0:
        return None
    return ideal_on(data, data["days"][-1])


def main():
    repository = os.environ["REPOSITORY"]
    token = os.environ["GH_TOKEN"]
    output = sys.argv[1] if len(sys.argv) > 1 else "."
    today = datetime.now(TIMEZONE).date()
    start = parse_date(os.environ.get("BURNDOWN_START_DATE"))
    end = parse_date(os.environ.get("BURNDOWN_END_DATE"))

    data = build(fetch_issues(repository, token), today, start, end)
    plot(data, os.path.join(output, "burndown.png"))

    remaining = data["remaining"]
    ideal = ideal_today(data)
    summary = {
        "remaining": remaining[-1],
        "scope": data["scope"][-1],
        "done": data["scope"][-1] - remaining[-1],
        "change": remaining[-1] - remaining[-2] if len(remaining) > 1 else 0,
        "open_items": data["open_items"],
        "unestimated": data["unestimated"],
        "ideal": round(ideal, 1) if ideal is not None else None,
        "end": data["end"].isoformat() if data["end"] else None,
        "days_left": workdays(today, data["end"]) if data["end"] and data["end"] > today else 0 if data["end"] else None,
    }
    with open(os.path.join(output, "burndown.json"), "w", encoding="utf-8") as file:
        json.dump(summary, file)
    print(json.dumps(summary))


if __name__ == "__main__":
    main()
