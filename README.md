# FurnitureERP

## Spuštění aplikace

### Požadavky

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Kroky

1. Obnovte závislosti:
   ```bash
   dotnet restore
   ```

2. Spusťte aplikaci:
   ```bash
   dotnet run --project FurnitureERP
   ```

3. Otevřete prohlížeč na adrese `https://localhost:7188`

4. Přihlaste se:
   - **Uživatel:** `admin`
   - **Heslo:** `admin`

## Databáze

Aplikace používá **SQLite** — databázové soubory jsou součástí projektu (`FurnitureERP.db`, `FurnitureERP-Business.db`). Žádná konfigurace připojení není potřeba.
