# ⚖️ Sistema de Gestión de Casos Jurídicos

Backend robusto y escalable desarrollado en .NET 8 con Clean Architecture, EF Core y buenas prácticas profesionales.

---

## 🚀 Tecnologías principales

- ASP.NET Core 8 (Web API RESTful)
- Entity Framework Core 9
- SQL Server
- Clean Architecture (capas bien definidas)
- FluentValidation
- Swagger UI (documentación interactiva)

---

## ✅ Funcionalidades completadas

- [x] CRUD completo para entidad **Caso**
- [x] Relación 1:N entre Cliente y Casos
- [x] Validaciones con FluentValidation
- [x] Creación automática de Cliente al crear un Caso
- [x] Middleware global de manejo de errores (`ProblemDetails`)
- [x] Endpoint especial `/api/casos/conteo-casos`
- [x] Endpoint `/api/casos` con paginación, búsqueda y ordenamiento dinámico
- [x] Documentación Swagger lista para testing

---

## 🧪 Pruebas en Swagger

URL:
https://localhost:7266/swagger
