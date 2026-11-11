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

- Park-wide settings live in the `ParkSettings` table: cleaning fee, tourist tax per person per night, arrival time, departure time and minimum nights in the high season. Each accommodation type has its own `PricePerNight`.
- `PriceSeeder` fills the settings (cleaning fee € 75, tourist tax € 2,50, arrival 15:00, departure 10:00, minimum 7 nights in the high season) and gives every type with price 0 a **temporary example price** per night. Replace these with the real amounts from the client.
- `PriceCalculator` (Application) calculates: nights × price per night + cleaning fee + tourist tax (all persons × nights × tax).
- The price breakdown (`PriceDto`) is part of every result of `GET /accommodations/search` and of `GET /accommodations/{id}?arrival=yyyy-MM-dd&departure=yyyy-MM-dd&persons={n}`. Without those three query values the detail endpoint returns no price. `Price` is `null` when a type has no price yet.
- `GET /accommodations/price?type={slug}&arrival=&departure=&persons=` returns the breakdown for a type. Invalid input gives 400, an unknown type or accommodation 404.

## Booking (US09)

1. Every search result on `/zoek-en-boek` shows the total price and a **Boeken** button to `/boeken?accommodatie={id}&aankomst=&vertrek=&personen=&huisdieren=`. The sidebar on an accommodation page (`/accommodaties/{slug}`) has a small form with dates, persons and pets; **Boeken** there opens `/boeken` for the first free accommodation of that type, or shows a message when none is free.
2. `/boeken` checks the query, loads the accommodation with its price from the API, checks persons and pets against that accommodation and asks for the guest’s name, email, phone and address.
3. The summary step shows the stay, the party, the guest and the price breakdown, with “Gegevens aanpassen” and “Boeking bevestigen”.
4. `/boeken/bevestiging` shows the confirmed booking. The booking is kept in the browser session (`BookingState`) only; saving it in the API comes with US11.

## Database

The API applies all migrations in `Data/Migrations` and runs the seeders at startup. For a later model change:

```
dotnet ef migrations add <Name> --project VakantiePark.Infrastructure --startup-project VakantiePark.API --output-dir Data/Migrations
```

## Tests

```
dotnet test
```

## JWT-sleutel

De sleutel waarmee de API inlogtokens ondertekent staat niet in de repo.

- **Development:** zonder ingestelde sleutel maakt de API bij elke start een willekeurige sleutel aan. Na een herstart moet je opnieuw inloggen. Wil je dat niet, zet dan een vaste sleutel in user-secrets:
  `dotnet user-secrets set "JWT:Key" "<minimaal 32 willekeurige tekens>" --project VakantiePark.API`
- **Andere omgevingen:** zet de sleutel in de omgevingsvariabele `JWT__Key`. Zonder sleutel start de API niet.
