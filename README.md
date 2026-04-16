# Joyitas Chirinos - Sistema de Gestión

Sistema completo de inventario y comercialización para joyería. .NET 10 + React + PostgreSQL.

## Stack
- **Backend**: .NET 10, Clean Architecture, CQRS/MediatR, EF Core, FluentValidation
- **Base de datos**: PostgreSQL 16
- **Auth**: JWT con roles (Admin / Vendedor)
- **Storage**: Cloudinary (fotos de productos)
- **Frontend**: React 18 + TypeScript + Tailwind CSS (PWA)

## Levantar la base de datos
```bash
cp .env.example .env
docker compose up -d
# pgAdmin → http://localhost:5050
```

## Ejecutar la API
```bash
cd src/JoyitasChirinos.API
dotnet run
# Swagger → http://localhost:5000/swagger
```

## Migraciones
```bash
dotnet ef migrations add InitialCreate --project src/JoyitasChirinos.Infrastructure --startup-project src/JoyitasChirinos.API
dotnet ef database update --project src/JoyitasChirinos.Infrastructure --startup-project src/JoyitasChirinos.API
```

## Tests
```bash
dotnet test
```
