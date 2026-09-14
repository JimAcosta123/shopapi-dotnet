# ShopApi — API REST de E-commerce (.NET / C#)

API REST para una tienda en linea: catalogo de productos por categoria, autenticacion de usuarios y gestion de pedidos con control de stock. Construida con **ASP.NET Core 8 Web API** y **Entity Framework Core**.

## Funcionalidades

- Registro e inicio de sesion con JWT (contrasenas hasheadas con BCrypt).
- Catalogo de productos publico, con busqueda y filtro por categoria.
- CRUD de productos protegido con `[Authorize]`.
- Creacion de pedidos: valida stock disponible, calcula el total y descuenta el inventario en una sola operacion.
- Actualizacion de estado de pedido (Pendiente / Pagado / Enviado / Cancelado).
- Swagger UI con soporte de JWT (boton "Authorize") para probar la API sin herramientas externas.
- Base de datos **SQLite** por defecto (un solo archivo, sin instalar ningun motor de base de datos) con datos de ejemplo precargados.

## Stack tecnico

C#, ASP.NET Core 8 Web API, Entity Framework Core (SQLite), JWT Bearer Authentication, BCrypt.Net, Swashbuckle (Swagger).

## Estructura del proyecto

```
03-shopapi-dotnet/
└── ShopApi/
    ├── Controllers/    # AuthController, ProductsController, CategoriesController, OrdersController
    ├── Data/           # AppDbContext (con datos semilla)
    ├── DTOs/           # Records de request/response
    ├── Models/         # Entidades: User, Product, Category, Order, OrderItem
    ├── Services/        # TokenService (generacion de JWT)
    ├── Program.cs        # Configuracion: DI, JWT, Swagger, CORS
    └── appsettings.json
```

## Como correrlo en local

### Requisitos
- .NET SDK 8.0+

### Pasos
```bash
cd ShopApi
dotnet restore
dotnet run
```

Al iniciar, la app crea automaticamente `shopapi.db` (SQLite) con 3 categorias y 4 productos de ejemplo. No necesitas instalar SQL Server ni PostgreSQL para probar el proyecto.

- Swagger UI: https://localhost:{puerto}/swagger (el puerto exacto lo imprime la consola al ejecutar `dotnet run`)
- Para probar rutas protegidas: registra un usuario en `/api/auth/register` o inicia sesion en `/api/auth/login`, copia el `token` recibido y pegalo en el boton **Authorize** de Swagger como `Bearer <token>`.

## Endpoints principales

| Metodo | Ruta                     | Descripcion                               | Auth |
|--------|--------------------------|---------------------------------------------|------|
| POST   | /api/auth/register        | Crea un usuario                              | No   |
| POST   | /api/auth/login           | Inicia sesion, devuelve JWT                  | No   |
| GET    | /api/categories            | Lista categorias                             | No   |
| GET    | /api/products?search=&categoryId= | Lista/busca productos                | No   |
| POST   | /api/products               | Crea un producto                             | Si   |
| PUT    | /api/products/:id            | Actualiza un producto                        | Si   |
| DELETE | /api/products/:id            | Elimina un producto                          | Si   |
| GET    | /api/orders/mine              | Pedidos del usuario autenticado              | Si   |
| POST   | /api/orders                    | Crea un pedido (valida y descuenta stock)    | Si   |
| PUT    | /api/orders/:id/status?status=  | Actualiza el estado de un pedido            | Si   |

## Posibles mejoras futuras

- Migraciones EF Core formales (`dotnet ef migrations add`) en lugar de `EnsureCreated()`, para versionar el esquema.
- Politicas de autorizacion por rol (`Admin` vs `User`) en productos.
- Paginacion en `/api/products`.
- Tests con xUnit + WebApplicationFactory para pruebas de integracion.
- Despliegue en Azure App Service o Railway, con PostgreSQL/SQL Server gestionado en produccion.

---
Proyecto desarrollado como parte del portafolio de Jim Kleber Acosta Acosta, egresado de Desarrollo de Software - SENATI.
