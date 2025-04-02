# Sistema de Gestión de Casos Jurídicos

Backend robusto en .NET 8 con Clean Architecture, EF Core y buenas prácticas.

## Tecnologías
- ASP.NET Core 8
- EF Core 9
- SQL Server
- Swagger
- FluentValidation

## Funcionalidades
✅ CRUD completo para entidad Caso  
✅ Relación 1:N entre Cliente y Casos  
✅ Validación con FluentValidation  
✅ Creación automática de cliente  
✅ Swagger operativo para testing  

## Cómo correrlo
1. Restaurar paquetes con `dotnet restore`
2. Aplicar migraciones si es necesario: `dotnet ef database update`
3. Ejecutar con `dotnet run` desde la API