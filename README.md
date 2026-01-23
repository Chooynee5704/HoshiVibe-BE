# HoshiVibe Backend

Backend API for HoshiVibe Web, built with ASP.NET Core 8. Focused on performance, security, and core commerce workflows.

- Live site: https://fe-hoshi-vibe.vercel.app/

## Screenshots

<p align="center">
  <img src="image/home.png" width="32%" alt="Home" />
  <img src="image/sanpham.png" width="32%" alt="Products" />
  <img src="image/gioithieu.png" width="32%" alt="About" />
</p>

## Features

- RESTful APIs: product, cart, order, voucher, user profile, dashboard
- Auth: JWT + Google OAuth
- Payments: VNPAY, MoMo
- Swagger/OpenAPI docs
- Layered architecture: Controllers / Service / Repository / Entities

## Tech Stack

- ASP.NET Core 8, C#
- EF Core 9, AutoMapper
- PostgreSQL (Npgsql)
- JWT, Swashbuckle

## Quick Start

```bash
dotnet restore
dotnet run
```

Configuration: use `.env` or `appsettings.json`. Swagger is available at `http://localhost:5243/swagger` (or `https://localhost:7217/swagger`).
