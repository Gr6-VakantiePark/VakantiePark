# VakantiePark

Website and API for Vakantiepark D’r op Uut (exam project K1W3).

## Projects

- `VakantiePark.API`: ASP.NET Core API
- `VakantiePark.Application`: services, price calculation, validation
- `VakantiePark.Domain`: entities
- `VakantiePark.Infrastructure`: EF Core (SQLite), migrations, seeders
- `VakantiePark.Shared`: DTOs and `ApiResponse<T>`
- `VakantiePark.UI`: Blazor WebAssembly website
- `VakantiePark.UnitTests`: xUnit + Moq tests

Styling rules for the UI are in [STYLEGUIDE.md](STYLEGUIDE.md).

## Prices (US08)

- Park-wide settings live in the `ParkSettings` table: cleaning fee, tourist tax per person per night, tourist tax minimum age, weekend surcharge percentage, pet fee per night, arrival time, departure time and minimum nights in the high season. Each accommodation type has its own `PricePerNight`.
- `PriceSeeder` fills the settings (cleaning fee € 75, tourist tax € 2,50 from 3 years, weekend surcharge 10%, € 10 per pet per night, arrival 15:00, departure 10:00, minimum 7 nights in the high season) and sets the prices per night agreed with the client (Berk 4 € 100, Berk 6 € 125, Beuk 8 € 175, Bosvilla 10 € 250, Eik 6 € 175, Eik 6 luxe € 200, Wilg € 100). Existing databases still on the old example prices are updated at start-up. Camping pitches keep their example prices until the client confirms them.
- `PriceCalculator` (Application) calculates: nights × price per night + weekend surcharge (percentage of the price per night for every Friday and Saturday night) + pets × nights × pet fee + cleaning fee (holiday homes only) + tourist tax ((persons − young children) × nights × tax).
- `persons` is the total group including young children, so young children count towards the maximum number of persons. `youngChildren` is the number of those under the tourist tax minimum age; at least one person must be older.
- The price breakdown (`PriceDto`) is part of every result of `GET /accommodations/search` and of `GET /accommodations/{id}?arrival=yyyy-MM-dd&departure=yyyy-MM-dd&persons={n}&youngChildren={n}&pets={n}`. Without arrival, departure and persons the detail endpoint returns no price. `Price` is `null` when a type has no price yet.

## Booking (US09)

1. Every search result on `/zoek-en-boek` shows the total price and a **Boeken** button to `/boeken?accommodatie={id}&aankomst=&vertrek=&personen=&huisdieren=`. The sidebar on an accommodation page (`/accommodaties/{slug}`) has a small form with dates, persons and pets; **Boeken** there opens `/boeken` for the first free accommodation of that type, or shows a message when none is free.
2. `/boeken` checks the query, loads the accommodation with its price from the API, checks persons and pets against that accommodation and asks for the guest’s name, email, phone and address.
3. The summary step shows the stay, the party, the guest and the price breakdown, with “Gegevens aanpassen” and “Boeking bevestigen”.
4. `/boeken/bevestiging` shows the confirmed booking. The booking is kept in the browser session (`BookingState`) only; saving it in the API comes with US11.

## Employees and rights (US41, US38)

- There are no roles. Each employee is either an **owner** (`IsOwner`) or has their own set of rights (`EmployeePermissions`). The rights are seeded from `Data/Seed/permissions.json`: guests view/manage, reservations, accommodations and pool capacity. An owner may do everything, including managing employees.
- Only owners can open `/beheer/medewerkers` and use `/admin/employees` (policy `Permissions.Owner`). **Bewerken** opens a form to change name and email, a **Rechten** section with a checkbox per right plus **Eigenaar**, and the status (**Uitschakelen** after a confirmation, or **Inschakelen**).
- Endpoints: `PUT /admin/employees/{id}` (name and email, the email must stay unique), `PUT /admin/employees/{id}/active` (`{ "isActive": false }`), `GET /admin/employees/permissions` (all rights) and `PUT /admin/employees/{id}/permissions` (`{ "isOwner": false, "permissions": ["guests.view"] }`). `POST /admin/employees` takes `isOwner` and `permissions` instead of roles.
- Every new endpoint must be `[AllowAnonymous]` or have `[Authorize(Policy = Permissions.X)]` (a right or `Permissions.Owner`); admin endpoints can never be public. Every page under `/beheer` (except the dashboard and the login page) needs `@attribute [Authorize(Policy = Permissions.X)]`, and the UI hides functions an employee has no right to. `AuthorizationRulesTests` fails the build when one of these is missing. Which right each feature needs is written in its user story.
- Rights are checked against the database on every request, so a change works straight away, even for tokens that were already handed out. The UI shows the new rights after logging in again.
- A switched-off employee can't log in, and tokens they already have stop working on the next request. Nobody can switch off their own account or take away their own owner rights.
- Seed accounts: `beheerder@dropuut.example` is an owner, `receptie@dropuut.example` has guests and reservations, `zwembad@dropuut.example` has pool capacity. The `PermissionsPerEmployee` migration converts existing databases: rights from the old roles move to the employees, administrators become owners and the role tables are removed.

## Pool time slots

- `PoolTimeSlots` holds the weekly swim schedule: one row per day of the week and start time, each with its own `Capacity`. `PoolSeeder` fills it with time slots of one hour within the opening hours on `/zwembad` (Monday to Friday 09:00–21:00, Saturday and Sunday 08:00–21:00) and 50 places per time slot.
- `PoolReservations` holds the places that are taken per time slot and date. Available places are the capacity minus the reserved persons, never below 0 (`PoolTimeSlot.AvailablePlaces`). A time slot without available places is full.
- `GET /pool/time-slots?date=yyyy-MM-dd` (public) returns the date and its time slots ordered by start time, each with `startTime`, `endTime`, `capacity`, `availablePlaces` and `isFull`. Without `date` it uses today; a date in the past returns 400.
- The pool page (`/zwembad`) shows these time slots under **Zwemmomenten** (`PoolTimeSlots` component). It opens on today, and the guest picks another day with the date field. Each time slot shows how many places are free; a full time slot is marked with a dashed border and **Vol**.

## API conventions

- Every endpoint returns `ApiResponse<T>`. Services throw `ArgumentException` (400), `KeyNotFoundException` (404) or `UnauthorizedAccessException` (401); controllers wrap the call with `RunAsync`.
- Each request DTO has a `Validate()` extension in `Application/Extensions` (`AuthValidation`, `AccommodationValidation`) that the service calls first. Lengths and limits come from `Shared/Common/FieldLimits.cs`, which the UI also uses for `maxlength`.
- Unexpected errors are logged by `GlobalExceptionHandler` and return a generic 500 message.
- Rate limiting: 50 requests per minute per logged-in employee, otherwise per IP address. Photos are excluded. Over the limit the API returns 429 with an `ApiResponse`.
- JSON (API and UI) uses `JsonDefaults.Options`: camelCase, enums as text, `null` values left out and `DateTime` always as UTC (`yyyy-MM-ddTHH:mm:ss.fffZ`).
- Settings use the options pattern and are checked at startup: `JWT` (`JwtSettings`), `Cors` (`CorsSettings`) and `DataSource`/`DataFolder` (`DatabaseSettings`). A missing or invalid value stops the API with a clear message.

## Database

The API applies all migrations in `Data/Migrations` and runs the seeders at startup. For a later model change:

```
dotnet ef migrations add <Name> --project VakantiePark.Infrastructure --startup-project VakantiePark.API --output-dir Data/Migrations
```

All ids are `Guid`s. Seeded rows get fixed ids from `SeedIds` (derived from the code, slug or email), so an accommodation has the same id on every machine and after a reseed.

The `GuidIds` migration switched the old `int` ids to `Guid`. It drops and recreates the tables, after which the seeders fill them again; employees that were added by hand are gone after this migration.

## Tests

```
dotnet test
```

## JWT-sleutel

De sleutel waarmee de API inlogtokens ondertekent staat niet in de repo.

- **Development:** zonder ingestelde sleutel maakt de API bij elke start een willekeurige sleutel aan. Na een herstart moet je opnieuw inloggen. Wil je dat niet, zet dan een vaste sleutel in user-secrets:
  `dotnet user-secrets set "JWT:Key" "<minimaal 32 willekeurige tekens>" --project VakantiePark.API`
- **Andere omgevingen:** zet de sleutel in de omgevingsvariabele `JWT__Key`. Zonder sleutel start de API niet.

## Discord notifications

GitHub Actions post to the G6 Vakantiepark Discord server. Each workflow uses its own webhook, stored as a repository secret; without the secret the workflow skips the message.

| Workflow | Trigger | Channel | Secret |
|---|---|---|---|
| `discord-pr.yml` | Pull request opened (not as draft), reopened, ready for review, merged or closed | #pull-requests | `DISCORD_WEBHOOK` |
| `discord-issues.yml` | Issue opened, closed, reopened or labelled `bug`; issues with the `bug` label go to #bug-alerts, all others to #issues | #issues / #bug-alerts | `DISCORD_WEBHOOK_ISSUES`, `DISCORD_WEBHOOK_BUGS` |
| `discord-bug-report.yml` | Weekdays at 08:30 Amsterdam time (also summer time), or by hand via *Run workflow*; lists open `bug` issues with age and assignee, plus new and closed bugs of the last 24 hours | #bug-report | `DISCORD_WEBHOOK_BUG_REPORT` |
| `discord-burndown.yml` | Weekdays at 08:30 Amsterdam time, or by hand via *Run workflow*; posts a burndown chart of the remaining hours of all issues (`.github/scripts/burndown.py`) | #burndown | `DISCORD_WEBHOOK_BURNDOWN` |
| `discord-commits.yml` | Push to `master`; lists the pushed commits | #commits | `DISCORD_WEBHOOK_COMMITS` |
| `discord-ci.yml` | *CI + build and test* finished; shows passed/failed, branch, commit and duration | #ci-builds | `DISCORD_WEBHOOK_CI` |

Scheduled and `workflow_run` workflows only run from the default branch (`master`).

The burndown chart counts the hours from the `Uren` table in each issue body. User stories with sub-issues are skipped, so only their tasks count (no double hours). An issue counts as done on the day it is closed. The Actions variable `BURNDOWN_END_DATE` (Settings → Secrets and variables → Actions → Variables, format `YYYY-MM-DD`) is the last coding day and draws the ideal line, which runs from the current total scope at the start date to 0 on that day and only goes down on weekdays (Saturday and Sunday are flat); it is set to `2026-12-11`. `BURNDOWN_START_DATE` is the first day of the chart and is set to `2026-11-11`; without it the chart starts at the first issue.

