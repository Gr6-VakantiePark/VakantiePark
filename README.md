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

- Park-wide settings live in the `ParkSettings` table: cleaning fee, tourist tax per person per night, arrival time and departure time. Each accommodation type has its own `PrijsPerNacht`.
- `PriceSeeder` fills the settings (cleaning fee € 75, tourist tax € 2,50, arrival 15:00, departure 10:00) and gives every type with price 0 a **temporary example price** per night. Replace these with the real amounts from the client.
- `PriceCalculator` (Application) calculates: nights × price per night + cleaning fee + tourist tax (all persons × nights × tax).
- Endpoint: `GET /accommodaties/price?type={slug}&arrival=yyyy-MM-dd&departure=yyyy-MM-dd&persons={n}` returns `ApiResponse<PriceDto>` with the breakdown. Invalid input gives 400, an unknown type 404.

## Booking (US09)

1. `/boeken?type=&aankomst=&vertrek=&volwassenen=&kinderen=&huisdieren=` checks the query (type, dates, party size, max persons and pets) and asks for the guest’s name, email, phone and address.
2. The summary step shows the stay, the party, the guest and the price from the price endpoint, with “Gegevens aanpassen” and “Boeking bevestigen”.
3. `/boeken/bevestiging` shows the confirmed booking. The booking is kept in the browser session (`BookingState`) only; it is not saved in the API yet.

## Database

The API applies the migrations (`InitialCreate`, `ParkSettings`) and runs the seeders at startup. For a later model change:

```
dotnet ef migrations add <Name> --project VakantiePark.Infrastructure --startup-project VakantiePark.API --output-dir Data/Migrations
```

## Tests

```
dotnet test
```
