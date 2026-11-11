# Style guide: Vakantiepark D’r op Uut (Blazor WebAssembly)

Rules for AI agents that build or edit the Vakantiepark D’r op Uut site. The site is a **Blazor WebAssembly Standalone** app styled with hand-written CSS. It uses no Bootstrap, no Tailwind and no CSS framework. Icons come from **Font Awesome 4.7**.

The source of truth is the CSS in the project: the global stylesheet plus each component’s scoped CSS (section 3). When this guide and the CSS disagree, report the mismatch and do not guess.

## 1. Ground rules

- **Global stylesheet for the foundation only.** `wwwroot/css/style.css` holds the design tokens, base element styles, and classes used in many places. Everything else is component-scoped CSS in a `.razor.css` file next to the component that renders it. Section 3 explains what goes where.
- **No `<style>` blocks or `style=""` attributes.**
- **No CSS frameworks.** Delete the Bootstrap folder the .NET project template ships with.
- **Icons: Font Awesome 4.7 only** (section 4). No other icon set and no inline SVG icons.
- **Never hard-code** a color, font, font size, radius or width that exists as a custom property. Use `var(--…)` in both global and scoped CSS.
- **Content is Dutch, code is English.** Visible text is Dutch and the page uses `<html lang="nl">`. Everything in the code is English: CSS custom properties, CSS class names, element IDs, URL fragments, and every C# type, variable, parameter, field, property and method.
- **Component names are not fixed by this guide.** It describes components by their role (page header, breadcrumbs, type card). Use whatever names exist in the project, and when creating a new component, give it a clear English name.
- **No dark mode for now.** Do not add `prefers-color-scheme` rules or theme switching.
- **No code comments** unless the reason behind something is not obvious from the code (a framework workaround, for example). Never write comments that describe what the code does.
- **Never run `git commit` or `git push`.** Make the file changes and leave committing to the developer.

## 2. Design tokens

All tokens live on `:root` in `wwwroot/css/style.css`. Scoped CSS reads them with `var(--…)`. Never redefine them in a `.razor.css` file.

### Colors (taken from the logo)

| Token | Value | Role |
|---|---|---|
| `--forest` | `#0e5b3a` | Primary. Headings, links, buttons, footer background, borders on emphasized boxes |
| `--grass` | `#0b8a50` | Hover state for links and buttons, checkmarks, the "houses" zone dot |
| `--leaf` | `#7db443` | Accent lines only: active nav underline, top rule on `.key-facts` and `.type-card__summary`, checkmarks in `.amenities--some`. Never use it for text |
| `--water` | `#3e9d9b` | Focus ring, `.feature` background, pool accents (`.hours`, `.pool`), module placeholder border |
| `--sand` | `#d9c9a1` | Footer link hover, "camping" zone dot. Decorative only |
| `--ink` | `#17301f` | Body text |
| `--ink-soft` | `#4a5e50` | Secondary text: intros, notes, counts, `dt` labels |
| `--mist` | `#eef4ee` | Light surfaces: `.section--mist`, image placeholder background, outline-button hover |
| `--white` | `#ffffff` | Page background, cards |
| `--line` | `#d4e0d6` | Hairlines, field borders, row dividers |
| `--error` | `#b3261e` | Validation messages and the border of invalid form fields. About 6.5:1 on white. Never use it for anything else |

Two literal colors exist: `#dfeadf` (footer body text) and `#b9a877` (border of the sand zone dot). Reuse them only in those same roles.

There is **no warning or success color** yet. When status styling is needed, ask the user for the value of a new token. Do not invent one.

Contrast warnings:
- White text on `--water` is about 3.2:1, which fails WCAG AA for body text. It is only used in `.feature`. Do not put white body text on `--water` anywhere new.
- White on `--grass` is about 4.4:1, just below AA for normal text. That is acceptable only as a hover state. Never make `--grass` a resting background behind text.

### Typography

| Token | Value |
|---|---|
| `--font-heading` | `"Young Serif", Georgia, "Times New Roman", serif`. Headings, brand name, large numbers |
| `--font-body` | `"Atkinson Hyperlegible", "Segoe UI", Arial, sans-serif`. Everything else |

| Token | Value | Used for |
|---|---|---|
| `--text-sm` | `0.875rem` | Breadcrumbs, notes, counts, `dt` labels, footer bottom bar |
| `--text-base` | `1.0625rem` | Body text, buttons |
| `--text-md` | `1.3rem` | `h3`, page intro paragraphs, `.lead`, footer `h2` |
| `--text-lg` | `1.65rem` | Card names, key-fact values, sidebar card `h2` |
| `--text-xl` | `2.1rem` | `h2` |
| `--text-2xl` | `clamp(2.4rem, 5.5vw, 3.9rem)` | `h1` |

- `h1`–`h3` use `--font-heading` at weight 400, `line-height: 1.15`, color `--forest`. Never bold the serif.
- Body uses `line-height: 1.6`. Paragraphs are capped at `max-width: 65ch`.
- Bold (700) belongs to the sans font: nav links, buttons, labels, `dd` values in `.sidebar-card` and `.hours`.
- Only these weights are loaded: Atkinson Hyperlegible 400 and 700, and Young Serif 400.

### Spacing, size and shape

| Token | Value | Use |
|---|---|---|
| `--section-space` | `clamp(3rem, 7vw, 5.5rem)` | Vertical padding of every `.section`, footer top margin |
| `--content-width` | `72rem` | Max content width (via `.container`) |
| `--radius-sm` | `6px` | Form fields, table wrapper |
| `--radius-lg` | `22px` | Cards, sidebar card, feature block, large gallery corners, footer logo |

- Buttons and `.tag` are pills: `border-radius: 999px`. Small gallery images use `8px`.
- Type-card photos use the signature asymmetric radius `var(--radius-lg) var(--radius-lg) var(--radius-lg) 4px`.
- The design uses no box shadows.

### Breakpoints

Desktop-first `max-width` media queries: `760px` is the main breakpoint, and `860px` is used only for the `.detail` layout. Custom properties don’t work inside media queries, so write these two values literally, in both global and scoped CSS. Prefer `auto-fill`/`auto-fit` grids with `minmax()` over adding new breakpoints.

## 3. Where CSS goes

### Global: `wwwroot/css/style.css`

Only these belong in the global stylesheet:

1. **Tokens:** the `:root` block from section 2.
2. **Base element styles:** box-sizing reset, `body`, `img`/`svg`, `h1`–`h3`, `p`, `a`, and the global `:focus-visible` ring.
3. **Shared classes**, used by many components or pages:
   - `.container`
   - `.section`, `.section--mist`, `.section__header`
   - `.button`, `.button--outline`
   - `.two-columns`
   - `.visually-hidden`
   - small text helpers: `.lead`, `.note`, `.count`
4. **Rules that span components** and so cannot be scoped: the footer rule in section 7, `#content:focus`, and the `#blazor-error-ui` and `.loading` styles for `index.html`.

### Scoped: `.razor.css`

Everything else lives in the scoped CSS of the component that renders the markup. For example, `.type-card` goes with the type-card component, `.gallery` with the gallery component, and `.main-nav` with the navigation component. Page-specific blocks such as `.feature`, `.zones`, `.hours` and `.pools` go in the scoped CSS of the page that uses them.

### When to promote a class to global

Move a class from scoped to global when it is used in **three or more** unrelated components or pages, or when it has to style markup across component boundaries in a way `::deep` can’t handle cleanly. Copying the same rule into two scoped files is a sign it should move. When you move a class, delete it from every scoped file. A class must never be defined in both places.

### Scoped CSS rules

- A scoped rule only matches elements written in **that component’s own markup**. Elements rendered by a child component (including `NavLink`, `EditForm`, `InputText` and the project’s own components) are not matched. Reach them with `::deep` from a scoped parent element: `.main-nav ::deep a { … }`.
- Use `::deep` only for markup rendered by built-in components such as `NavLink` and `Input*`. For the project’s own child components, style the child in its own `.razor.css` file.
- `::deep` only works when the parent element is in the component’s own markup. A component whose root is another component cannot use it, so wrap that component in an element.
- Keep the `<link>` to the `{Project}.styles.css` bundle in `index.html` (section 5).

## 4. Icons: Font Awesome 4.7

- Load Font Awesome 4.7 from a local copy in `wwwroot/lib/font-awesome/` (the `css` and `fonts` folders from the 4.7.0 release). Self-hosting avoids another third-party runtime dependency. If the project already uses the CDN, keep using it.
- Use only the 4.7 class syntax: `fa fa-<name>`. Font Awesome 5 and later syntax (`fas`, `fa-solid`, `fa-regular`) and newer icons do not exist in 4.7. Check the name against the 4.7 icon list before using it. Useful icons that do exist in 4.7 include `fa-wheelchair`, `fa-paw`, `fa-bed`, `fa-bath`, `fa-car`, `fa-bolt`, `fa-tint`, `fa-fire`, `fa-users`, `fa-phone`, `fa-envelope`, `fa-map-marker`, `fa-clock-o` and `fa-check`.
- **Icon next to text** (decorative): `<i class="fa fa-phone" aria-hidden="true"></i> 000 - 000 00 00`.
- **Icon-only control:** hide the icon and give the control an accessible name: `<button class="…" aria-label="Sluiten"><i class="fa fa-times" aria-hidden="true"></i></button>`. You can also use a `<span class="visually-hidden">` with the text instead of `aria-label`.
- Icons inherit `color` and `font-size`. Color them with tokens through their parent. Size them with the type-scale tokens rather than `fa-2x` or `fa-3x`. `fa-fw` is fine for aligning icons in a list.
- Never use an icon as the only carrier of meaning (status, availability). Always pair it with text.
- The CSS-drawn checkmarks in `.amenities` and the zone dots are part of the design. Keep them; don’t swap them for icons.

## 5. Project setup

### Folders

```
wwwroot/
  index.html
  css/style.css
  lib/font-awesome/css/font-awesome.min.css
  lib/font-awesome/fonts/…
  img/favicon.png, logo.png, logo-mark.png
  img/photos/<slug>-1.jpg … <slug>-4.jpg, hero.jpg, pool-1..3.jpg
```

Keep the layout, pages and components in the folders the project already uses.

Delete the .NET template leftovers: `wwwroot/lib/bootstrap`, the default `app.css`, the default navigation menu and its CSS, the Counter and Weather pages, and `sample-data`.

### Routes

URLs are visible to visitors, so they are Dutch:

| Route | Content |
|---|---|
| `/` | Homepage |
| `/accommodaties` | Overview of all accommodation types |
| `/accommodaties/{Slug}` | One accommodation type |
| `/zwembad` | Swimming pool |
| `/contact` | Contact form and details |
| `/zoek-en-boek?type={slug}` | Search and booking module |

All accommodation types share **one** routed page fed by data. Do not create a page per accommodation. An unknown slug shows a not-found state that still uses the page header.

Read the query string with `[SupplyParameterFromQuery(Name = "type")] public string? Type { get; set; }` (the query key stays `type`).

### `wwwroot/index.html`

```html
<!DOCTYPE html>
<html lang="nl">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Vakantiepark D’r op Uut</title>
    <base href="/">
    <link rel="icon" type="image/png" href="img/favicon.png">
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Atkinson+Hyperlegible:wght@400;700&family=Young+Serif&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="lib/font-awesome/css/font-awesome.min.css">
    <link rel="stylesheet" href="css/style.css">
    <link rel="stylesheet" href="{Project}.styles.css">
</head>
<body>
    <div id="app">
        <p class="loading">Laden…</p>
    </div>

    <div id="blazor-error-ui" role="alert">
        Er ging iets mis. <a href="." class="reload">Opnieuw laden</a>
    </div>
</body>
</html>
```

- Keep the stylesheet order: Font Awesome, then global, then the scoped bundle, so component CSS can override the shared styles.
- Replace `{Project}` with the project’s assembly name, exactly as the template generated it.
- Keep the `<script>`, preload and import-map lines the .NET template generated. Their exact form depends on the SDK version.
- Keep `<HeadOutlet>` registered in `Program.cs` so pages can set their own title and meta description.
- `#app` and `#blazor-error-ui` are names Blazor itself expects. Keep them as they are.

These go in the global `style.css`, because `index.html` has no component:

```css
.loading {
  padding: var(--section-space) 1.25rem;
  text-align: center;
  color: var(--ink-soft);
}

#blazor-error-ui {
  display: none;
  position: fixed;
  inset: auto 0 0 0;
  z-index: 1000;
  padding: 0.75rem 1.25rem;
  background: var(--forest);
  color: var(--white);
}

#blazor-error-ui a {
  color: var(--white);
  font-weight: 700;
  margin-left: 1rem;
}
```

## 6. Paths and links

- `<base href="/">` resolves every relative URL against the site root. Write paths **without** a leading slash and **without** `../`: `img/photos/berk-4-1.jpg`, `accommodaties/berk-4`, `zoek-en-boek?type=berk-4`.
- Routes have no `.html` extension. The home link is `href=""`.
- Fragment links to another page include that page’s path: `accommodaties#camping`.
- A bare `#fragment` resolves to the **homepage**, not the current page. `href="#zones"` therefore works from any page, but same-page fragment links do not work without extra handling. The skip link deals with this in section 7.
- The main CTA is always labelled "Zoek en boek" and always points to `zoek-en-boek`. Accommodation pages pass the type: `zoek-en-boek?type=@Slug`.
- Section anchors on the overview page are `#holiday-homes` and `#camping`. The homepage zone section is `#zones`.

## 7. Layout

The layout component holds the parts every page shares. Pages render only what goes inside `<main>`.

```razor
@inherits LayoutComponentBase

<a class="skip-link" href="#content" @onclick="SkipToContent" @onclick:preventDefault>Naar de inhoud</a>

<header class="site-header">
    <div class="container">
        <a class="brand" href="">
            <img src="img/logo-mark.png" alt="" width="58" height="47">
            <span class="brand__name"><span class="brand__label">VAKANTIEPARK</span><span class="brand__title">D’r op Uut</span></span>
        </a>
        …navigation…
    </div>
</header>

<main id="content" tabindex="-1" @ref="_main">
    @Body
</main>

…footer…

@code {
    private ElementReference _main;

    private async Task SkipToContent() => await _main.FocusAsync();
}
```

- `.skip-link`, `.site-header` and `.brand*` go in the layout’s scoped CSS.
- The skip link moves focus itself because a plain `#content` would navigate to the homepage (section 6). Add `#content:focus { outline: none; }` to the global stylesheet so the whole page doesn’t get a focus ring when the skip link is used.
- Keep `<FocusOnNavigate RouteData="@routeData" Selector="h1" />` in `App.razor`, so keyboard and screen-reader users land on the new page’s heading after each navigation.

### Navigation

```razor
<nav class="main-nav" aria-label="Hoofdmenu">
    <ul>
        <li><NavLink href="" Match="NavLinkMatch.All">Home</NavLink></li>
        <li><NavLink href="accommodaties">Accommodaties</NavLink></li>
        <li><NavLink href="zwembad">Zwembad</NavLink></li>
        <li><NavLink href="contact">Contact</NavLink></li>
        <li><NavLink class="button" href="zoek-en-boek">Zoek en boek</NavLink></li>
    </ul>
</nav>
```

- Use the default prefix matching for Accommodaties so accommodation pages highlight it. Only Home uses `NavLinkMatch.All`.
- The `<a>` elements are rendered by `NavLink`, so the navigation’s scoped CSS must reach them with `::deep`:

```css
.main-nav ::deep a {
  text-decoration: none;
  font-weight: 700;
  padding-block: 0.3rem;
  border-bottom: 3px solid transparent;
}

.main-nav ::deep a.active,
.main-nav ::deep a[aria-current="page"] {
  border-bottom-color: var(--leaf);
}
```

- `NavLink` marks the active link with the `active` class. Check in the rendered DOM whether your .NET version also sets `aria-current="page"`. If it doesn’t, tell the user, because screen readers rely on that attribute.

### Footer

The footer is a `<footer class="site-footer">` with four columns: logo, Verblijven, Op het park, and Contact. Its links point to routes (`accommodaties#holiday-homes`, `accommodaties#camping`, `zoek-en-boek`, `zwembad`, `#zones`, `contact`), plus `tel:` and `mailto:` links. Footer styles go in the footer’s scoped CSS.

The footer normally sits `--section-space` below the last section. When the last section on a page is `.section--mist`, that gap would show as a white strip between grey and green, so this rule removes it. It spans the layout and the page, so it goes in the **global** stylesheet:

```css
main:has(> .section--mist:last-child) + .site-footer {
  margin-top: 0;
}
```

For this rule to work, pages must render their `<section>` elements as direct children of `<main>`. Do not wrap a page’s markup in an extra `<div>`.

## 8. Page structure

```razor
@page "/zwembad"

<PageTitle>Zwembad | Vakantiepark D’r op Uut</PageTitle>
<HeadContent>
    <meta name="description" content="Openingstijden en baden van het zwembad.">
</HeadContent>

…page header…

<section class="section">
    <div class="container two-columns">
        …
    </div>
</section>
```

- `<PageTitle>` always follows `Paginanaam | Vakantiepark D’r op Uut`. The homepage uses `Home | Vakantiepark D’r op Uut`.
- Every page except the homepage starts with the page header (section 9). The homepage starts with the hero.
- Exactly one `h1` per page. The page header and the hero provide it.
- The site has no render modes to choose: in a standalone WebAssembly app everything is interactive. Do not add `@rendermode`.

## 9. Building blocks

The markup below is what each building block must render. Put each one in a component under whatever name fits the project.

### Wave divider

The brand’s signature element. It closes every hero and page header. Paste the SVG verbatim and do not change the paths or colors. The `fill` values match `--grass`, `--water`, `--forest` and `--sand`; they are hard-coded because SVG presentation attributes cannot read CSS variables.

```html
<svg class="waves" viewBox="0 0 1440 170" preserveAspectRatio="none" aria-hidden="true" focusable="false">
    <path fill="#0b8a50" d="M0,20 C360,-10 720,70 1440,30 L1440,58 C720,98 360,18 0,48 Z"/>
    <path fill="#3e9d9b" d="M0,62 C400,40 760,110 1440,72 L1440,92 C760,130 400,60 0,82 Z"/>
    <path fill="#0e5b3a" d="M0,96 C380,80 800,145 1440,104 L1440,122 C800,163 380,98 0,114 Z"/>
    <path fill="#d9c9a1" d="M200,134 C520,120 860,168 1240,140 L1240,152 C860,178 520,140 200,148 Z"/>
</svg>
```

The `.waves` styles and the `wave-in` animation go in this component’s scoped CSS. The waves animate in each time a page mounts. That is intended, and the animation sits inside `@media (prefers-reduced-motion: no-preference)`.

### Page header and breadcrumbs

```html
<section class="page-header">
    <div class="container">
        <nav class="breadcrumbs" aria-label="Kruimelpad">
            <ol>
                <li><a href="">Home</a></li>
                <li><a href="accommodaties">Accommodaties</a></li>
                <li aria-current="page">Berk 4</li>
            </ol>
        </nav>
        <h1>Berk 4</h1>
        <p>Twee slaapkamers</p>
    </div>
    …wave divider…
</section>
```

- The breadcrumbs always start with Home and end with the current page as plain text with `aria-current="page"`. The crumbs in between are passed in as a list of text-and-link pairs.
- Title and intro are required parameters. The intro is one short line; on accommodation pages it is the type’s tagline.

### Hero (homepage only)

```html
<section class="hero">
    <div class="container hero__grid">
        <div>
            <h1>Tussen bos, heide en water</h1>
            <p class="hero__intro">…</p>
            <div class="hero__actions">
                <a class="button" href="zoek-en-boek">Zoek en boek</a>
                <a class="button button--outline" href="accommodaties">Bekijk accommodaties</a>
            </div>
        </div>
        <img class="hero__image" src="img/photos/hero.jpg" alt="Uitzicht over het park" width="1600" height="1100">
    </div>
    …wave divider…
</section>
```

### Type cards

The homepage and the overview page both show grids of accommodation types. The grid owns `.type-cards`, and each card owns `.type-card*`.

```razor
<ul class="type-cards">
    @foreach (var type in Types)
    {
        <li @key="type.Slug">
            <a class="type-card" href="accommodaties/@type.Slug">
                <img src="img/photos/@(type.Slug)-1.jpg" alt="" width="1600" height="1000" loading="lazy">
                <span class="type-card__name">@type.Name</span>
                <span class="type-card__tagline">@type.Tagline</span>
                <span class="type-card__summary">@type.Summary</span>
            </a>
        </li>
    }
</ul>
```

- The image is decorative (`alt=""`) because the name is in the link text.
- The summary always follows the pattern `<personen> personen, <aantal> huizen|plaatsen`.
- If the card becomes its own component that renders the `<li>`, put `@key` on the component instead.

### Gallery

One large image plus three small ones. `gallery--three` gives one large and two small (used on the pool page).

```html
<section class="gallery-section" aria-label="Foto's">
    <div class="container gallery">
        <img class="gallery__main" src="img/photos/berk-4-1.jpg" alt="Berk 4, foto 1" width="1600" height="1000">
        <img src="img/photos/berk-4-2.jpg" alt="Berk 4, foto 2" width="800" height="600" loading="lazy">
        <img src="img/photos/berk-4-3.jpg" alt="Berk 4, foto 3" width="800" height="600" loading="lazy">
        <img src="img/photos/berk-4-4.jpg" alt="Berk 4, foto 4" width="800" height="600" loading="lazy">
    </div>
</section>
```

The first image is the largest one on the page, so it gets no `loading="lazy"`. The gallery CSS depends on the exact image count and order. Do not add images or wrap them in other elements.

### Accommodation page blocks

| Block | Markup | Notes |
|---|---|---|
| Key facts | `<dl class="key-facts">` with `<div><dt>Label</dt><dd>Waarde</dd></div>` per item | 4–5 items. Houses show Personen, Slaapkamers, Badkamers, Bedden and Huisdieren. Camping pitches show Personen, Ondergrond, Stroom, Max. lengte (camper and caravan only) and Huisdieren |
| Amenities | `<ul class="amenities">` (always present) or `amenities amenities--some` (some units, with `<span class="count">N van M</span>`) | The order is fixed: `h2` Voorzieningen, `h3` Altijd aanwezig, then `h3` "Bij een deel van de huizen" (or "plaatsen"), `p.note`, and the `--some` list. `amenities--none` exists for absent items |
| Sidebar card | `<aside class="sidebar-card" aria-labelledby="heading-booking">` | Summary `dl`s, "Te vinden in" zones sorted by count (highest first), a full-width `.button` to `zoek-en-boek?type=@Slug`, and `p.sidebar-card__note`. Its `h3` deliberately renders as bold sans text |

The accommodation page layout is `<div class="container detail">`: the content column plus the sidebar card. `.detail` goes in that page’s scoped CSS.

## 10. Class reference

| Class | Where | Purpose |
|---|---|---|
| `.container` | global | Centered content width |
| `.section` / `.section--mist` / `.section__header` | global | Every content block is `<section class="section">` with one `<div class="container">` inside. Alternate white and mist sections, and never place two `.section--mist` next to each other. `.section__header` is the row with the `h2` and an optional one-line subtitle |
| `.two-columns` | global | Two equal columns, one column below 760px. Put it on the `.container` |
| `.button` / `.button--outline` | global | Primary and secondary buttons. Use at most one of each per action group. Use `<a>` for navigation and `<button type="submit">` for forms |
| `.lead`, `.note`, `.count` | global | Lead text and small secondary text |
| `.visually-hidden` | global | Screen-reader-only text |
| `.section--flush-top` | homepage | Removes the top padding of a section that follows another one directly |
| `.feature` | homepage | Teal feature block (pool teaser). One per page at most |
| `.zones`, `.zone-dot--houses`, `.zone-dot--camping` | homepage | Zone tiles. Green means houses, sand means camping |
| `.hours`, `.pools`, `.pool`, `.pool__size` | pool page | Opening hours and pool tiles |
| `.form`, `.field`, `.field__hint`, `.contact-details`, `.contact-details__note` | contact page | Form and address block (section 11) |
| `.module-placeholder` | booking page | Dashed teal box for unfinished modules |
| `.table-wrap`, `table`, `.tag` | scoped where first used | Data table with pill tags. Not used on any page yet |

Give each section that has a heading `aria-labelledby="heading-…"` pointing at its `h2` (`heading-houses`, `heading-camping`, `heading-pools`). Use stable, hard-coded IDs.

## 11. Forms

Use `EditForm` with the built-in input components. `EditForm` accepts `class="form"`.

```razor
<EditForm class="form" Model="_message" OnValidSubmit="SendAsync" FormName="contact">
    <DataAnnotationsValidator />
    <div class="field">
        <label for="name">Naam</label>
        <InputText id="name" @bind-Value="_message.Name" autocomplete="name" />
        <ValidationMessage For="() => _message.Name" />
    </div>
    <div class="field">
        <label for="reservation-number">Reserveringsnummer <span class="field__hint">Alleen als je al geboekt hebt</span></label>
        <InputText id="reservation-number" @bind-Value="_message.ReservationNumber" />
    </div>
    <div><button class="button" type="submit">Verstuur bericht</button></div>
</EditForm>
```

- `EditForm` is a child component, so its `<form>` and everything inside it is **not** matched by the page’s scoped CSS. Wrap the form in an element and use `::deep`: `.contact-form ::deep .field input { … }`. If forms appear on several pages, move `.form` and `.field*` to the global stylesheet instead (section 3).
- Every control has an English kebab-case `id` that matches its `<label for>`. Help text goes inside the label as `span.field__hint`.
- Use `InputText`, `InputSelect`, `InputTextArea` and `InputDate`.
- Required fields use `[Required]` on the model with a Dutch `ErrorMessage`. Don’t also add the HTML `required` attribute, or the browser and Blazor will both validate. Don’t mark required fields with an asterisk.
- Blazor adds `valid`, `invalid` and `modified` classes to inputs, and renders `div.validation-message`. Style `.invalid` fields with a `var(--error)` border and `div.validation-message` as small bold `var(--error)` text, both through `::deep` from the form wrapper. `valid` and `modified` get no styles.

## 12. Data and text formatting

- Use typographic characters in visible text: ’ in "D’r op Uut" (never `'`), and an en dash – for ranges.
- Ranges in data (key facts, sidebar card, times) use a spaced en dash: `2 – 3`, `09:00 – 21:00`, `10 A – 16 A`. Inside `.type-card__summary` the dash has no spaces: `4–6 personen`.
- Times use the 24-hour clock: `08:30`. Decimals use a comma: `8,5 m`.
- Do not set `CultureInfo.DefaultThreadCurrentCulture` in WebAssembly. Format with an explicit Dutch culture where needed (`new CultureInfo("nl-NL")`), and make sure Dutch globalization data is loaded (`<BlazorWebAssemblyLoadAllGlobalizationData>true</BlazorWebAssemblyLoadAllGlobalizationData>`), otherwise visitors whose browser uses another language may get wrong formatting. Values that never change, such as "max. 2" or "Verhard", can simply be stored as text.
- Tone: friendly and direct. Address the visitor as "je/jij", never "u". Use short sentences and sentence-case headings.
- Use digits in data and spelled-out numbers in short taglines ("Twee slaapkamers").

## 13. Images

- Photos follow the slug: `img/photos/<slug>-1.jpg` is the main image (1600×1000), and `-2` to `-4` are 800×600. The exceptions are `hero.jpg` (1600×1100) and `pool-1..3.jpg` (1600×1000, then 800×600).
- Always set `width` and `height` to the source dimensions.
- Images below the fold get `loading="lazy"`. The hero and the first gallery image do not.
- Decorative images get `alt=""`. Content images get descriptive Dutch alt text.

## 14. Accessibility checklist

- The skip link works and focuses `<main>` (section 7), and `FocusOnNavigate` targets `h1`.
- Every `<nav>` has a Dutch `aria-label` (`Hoofdmenu`, `Kruimelpad`).
- One `h1` per page, and heading levels never skip.
- Use `<dl>` for label/value data and lists for lists. Wrap each `dt`/`dd` pair in a `<div>`.
- Decorative SVGs and Font Awesome icons get `aria-hidden="true"`. Icon-only controls get an accessible name.
- Never remove the global `:focus-visible` outline (3px `--water`).
- Motion goes only inside `@media (prefers-reduced-motion: no-preference)`.

## 15. Don’ts

- No Bootstrap, Tailwind or component libraries (MudBlazor, Radzen and similar). No icon set other than Font Awesome 4.7.
- No `<style>` blocks or `style=""` attributes.
- No component-specific classes in the global stylesheet, and no class defined both globally and scoped.
- No redefining tokens in scoped CSS, and no new colors, fonts, radii or shadows outside the tokens.
- No `!important`, apart from the two gallery corner overrides.
- No Dutch in code: class names, IDs, tokens and C# identifiers are English. Only visible text, `aria-label` values and URLs are Dutch.
- No wrapper `<div>` around a page’s sections (it breaks the footer rule in section 7).
- No dark mode for now.
- No `<div>` soup. Use `section`, `nav`, `aside`, `address`, `dl` and lists where they fit.