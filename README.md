# ⚖️ Sistema de Gestión de Casos Jurídicos

Backend robusto y escalable desarrollado en .NET 8 con Clean Architecture, EF Core, JWT y buenas prácticas profesionales.

---

## 🧠 Tecnologías principales

- ✅ ASP.NET Core 8 (Web API RESTful)
- ✅ Entity Framework Core 9
- ✅ SQL Server
- ✅ Clean Architecture (Application, Domain, Infrastructure)
- ✅ FluentValidation
- ✅ JWT Authentication
- ✅ Swagger UI (documentación interactiva)

---

## 🎨 Frontend – Legal Cases UI

Interfaz desacoplada en HTML, CSS y JS puro (Vanilla JS), conectada al backend mediante tokens JWT.

### 🛠 Tecnologías
- HTML5 + Bootstrap 5
- JavaScript moderno (Fetch API)
- Diseño responsive con glassmorphism

### 💡 Características
- Login con validación visual y animaciones
- Dashboard con tabla dinámica de casos
- Filtro por estado, paginación y buscador
- Acciones de editar, eliminar, cerrar caso, etc.

---

## 🚀 Funcionalidades clave

- [x] CRUD completo de **Casos**
- [x] Relación Cliente–Caso (1:N)
- [x] Creación automática de cliente desde API
- [x] Filtros combinados por estado, búsqueda y fecha
- [x] Ordenamiento dinámico y paginación real
- [x] Middleware global de manejo de errores (`ProblemDetails`)
- [x] Seguridad con JWT (Autenticación Bearer)
- [x] Swagger UI listo para probar endpoints
- [x] Separación estricta por capas (Clean Arch)

---

## 📂 Estructura del proyecto
CasosJuridicosRobusto/
├── API/ # Proyecto Web API (Startup + Controllers + JWT)
├── Aplicacion/ # Casos de uso, servicios, DTOs
├── Dominio/ # Entidades y enums del modelo de negocio
├── Infraestructura/ # Acceso a datos y repositorios
├── wwwroot/ # HTML, CSS y JS (frontend desacoplado)
├── appsettings.example.json
├── README.md
└── .gitignore

## 🧪 Pruebas rápidas con Swagger
1. Ejecutá el proyecto:
   ```bash
   dotnet run --project API
1. Abre tu navegador en:


https://localhost:7266/swagger

🔒 Seguridad aplicada
Autenticación con JWT Bearer

Login protegido

.gitignore para evitar leaks de:

appsettings.json

.env

tokens o secretos

🧾 Licencia
Uso educativo y demostrativo.
Desarrollado por @Derox1

yaml
Copiar
Editar

---

### 📌 Siguiente paso:
- Pegá esto como `README.md` en tu carpeta raíz.
- Confirmá que `git status` lo marque como nuevo.
- Luego:

```bash
git add README.md
git commit -m "docs: agregar README profesional con detalles del proyecto"
git push