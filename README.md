# ABC Pharmacy — Medicine Tracker

A single-page application for ABC Pharmacy to track medicines and sales.

- **Backend**: ASP.NET Core 9 Web API (`PharmacyApi/`) — data stored as JSON files on disk
- **Frontend**: Angular 19 SPA styled with Bootstrap (`pharmacyWeb/`)

## Features

- View medicines in a grid (Full Name, Expiry Date, Quantity, Price, Brand)
- Red highlight on Expiry Date when expiring in under 30 days
- Yellow highlight on Quantity when stock is under 10 units
- Search medicines by name, brand, or notes
- Add new medicine records
- Record a sale per medicine (decrements stock, logs the sale)
- View sales history

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v20+ recommended) and npm

## Project Structure

```
ABCPharmacyApp/
├── PharmacyApi/     # ASP.NET Core Web API
└── pharmacyWeb/     # Angular SPA
```

## Setup

- Clone/open the repository
- Install backend dependencies:
  - `cd PharmacyApi`
  - `dotnet restore`
- Install frontend dependencies:
  - `cd pharmacyWeb`
  - `npm install`

## Running the Project

Run the API and the SPA in two separate terminals.

- **Terminal 1 — start the API**
  - `cd PharmacyApi`
  - `dotnet run --urls http://localhost:5075`
  - API available at `http://localhost:5075/api`
- **Terminal 2 — start the SPA**
  - `cd pharmacyWeb`
  - `npx ng serve`
  - App available at `http://localhost:4200`
- Open `http://localhost:4200` in your browser

> The Angular dev server proxies `/api/*` requests to the backend (see `pharmacyWeb/proxy.conf.json`), so no CORS setup is needed for local development.

## Notes

- Sample medicine data is auto-seeded on first run into `PharmacyApi/Data/medicines.json`; sales are logged to `PharmacyApi/Data/sales.json`.
- To reset all data, stop the API and delete both JSON files in `PharmacyApi/Data/` — they'll be recreated (medicines re-seeded) on next run.
- If `npx`/`ng`/`npm` fail in a PowerShell terminal with a "running scripts is disabled" error, either run `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser` once, or use Git Bash/Command Prompt instead.
