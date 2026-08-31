# Shop API

Simple .NET 9 Onion-style backend for the current Shop frontend.

## Structure

- `Shop.Domain` — entities only
- `Shop.Application` — DTOs, interfaces, services
- `Shop.Infrastructure` — EF Core DbContext, repositories, seed data
- `Shop.API` — controllers, Swagger, CORS, startup

## Public endpoints

- `GET /api/products?pageNumber=1&pageSize=12&search=&category=`
- `GET /api/products/top?count=3`
- `GET /api/products/{id}`
- `GET /api/categories`
- `GET /api/categories/top?count=6`
- `GET /api/categories/{slug}`
- `GET /api/categories/{slug}/products?pageNumber=1&pageSize=12`
- `GET /api/products/{productId}/reviews`
- `POST /api/products/{productId}/reviews`
- `POST /api/newsletter/subscribe`

## Production behavior

- Swagger is enabled in Development and Production.
- `/` redirects to `/swagger`.
- CORS allows any origin, header, and HTTP method.
- The database schema is created automatically on first startup with `EnsureCreatedAsync()`.
- Seed data is inserted automatically only when the corresponding tables are empty.

Initial seeded data:

- 24 categories
- 120 products
- 360 product images
- 240 product tags
- 360 reviews
- 10 newsletter subscriptions

## Run

From the `Shop` folder:

```powershell
dotnet restore
dotnet build
dotnet run --project src/Shop.API
```

Open the URL printed by `dotnet run`, or append `/swagger`.

## EF tool

A local `dotnet-ef` 9.0.19 manifest is included so the global 10.x tool does not need to be removed. If migrations are added later:

```powershell
dotnet tool restore
dotnet ef --version
```

For this current simple version, migrations are not required for the first run because the API creates the schema automatically.
