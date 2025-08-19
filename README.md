Million – API de Gestión de Propiedades Inmobiliaria

API para gestionar propiedades inmobiliarias (EE. UU.).
Arquitectura limpia con DDD, CQRS (MediatR), EF Core 8 (SQL Server), versionado y JWT.
Incluye tests unitarios (NUnit + FluentAssertions + Moq + EFCore.Sqlite in-memory).

## ⚙️ Stack

.NET 8

ASP.NET Core (controllers, API versioning, ProblemDetails, Swagger)

MediatR (CQRS: Commands/Queries + pipeline de validación)

EF Core 8 (SQL Server), SqlClient

AutoMapper (proyección server-side para queries)

FluentValidation

JWT Bearer (Microsoft.IdentityModel.Tokens)

NUnit · FluentAssertions · Moq (tests)

NLog/Console (logs mínimos)

## 🏗️ Arquitectura

Clean Architecture + DDD

Domain: Entidades/agregados, Value Objects, eventos de dominio. Sin dependencias a frameworks.

Application: Casos de uso (Commands/Queries con MediatR), DTOs, validaciones, mapeos, puertos (Repository/UnitOfWork), resultados (Result<T>).

Infrastructure: EF Core (DbContext, Configs, Migrations), repositorios/UoW, read model optimizado (proyección con ProjectTo).

WebApi: Endpoints versionados, JWT, Swagger, health checks, manejo de errores con ProblemDetails.

CQRS

Commands (escritura): IRepository<T> + IUnitOfWork.

Queries (lectura): repositorio especializado (IPropertyReadRepository) que proyecta a DTO en servidor con ProjectTo.

DDD (dominante)

Agregado Property (raíz) con colecciones privadas de PropertyImage y PropertyTrace.

Value Objects inmutables: Address, Money.

Evento de dominio: PriceChangedDomainEvent.

## 📁 Estructura de la solución

<details><summary>Ver estructura</summary>

```text
Million-Technical-Test/
├─ src/
│  ├─ Million.Domain/
│  │  ├─ Abstractions/                (Entity, AggregateRoot, IDomainEvent)
│  │  ├─ Common/                      (Guard, DomainException)
│  │  ├─ Events/                      (PriceChangedDomainEvent.cs)
│  │  ├─ Owners/                      (Owner.cs)
│  │  └─ Properties/
│  │     ├─ Property.cs, PropertyImage.cs, PropertyTrace.cs
│  │     └─ ValueObjects/             (Address.cs, Money.cs)
│  ├─ Million.Application/
│  │  ├─ DependencyInjection.cs
│  │  ├─ Common/                      (Result, PagedList)
│  │  ├─ Abstractions/
│  │  │  └─ Persistence/              (IRepository, IUnitOfWork, IPropertyReadRepository, IOwnerReadRepository)
│  │  ├─ DTOs/                        (PropertyDTO, PropertyListItemDTO, PropertyImageItemDTO, PropertyTraceItemDTO,
│  │  │                                 OwnerDTO, OwnerPropertyItemDTO)
│  │  ├─ Mapping/                     (MappingProfile.cs)
│  │  ├─ Behaviors/                   (ValidationBehavior.cs)
│  │  └─ Properties/
│  │     ├─ Commands/                 (CreateProperty, AddPropertyImage, ChangePropertyPrice, UpdateProperty)
│  │     └─ Queries/                  (ListProperties)
│  └─ Million.Infrastructure/
│     ├─ DependencyInjection.cs
│     └─ Persistence/
│        ├─ MillionDbContext.cs
│        ├─ Configurations/           (Owner, Property, PropertyImage, PropertyTrace)
│        └─ Repositories/
│           ├─ Repository.cs, UnitOfWork.cs
│           └─ PropertyReadRepository.cs   (read model con AutoMapper.ProjectTo)
│
├─ src/Million.WebApi/
│  ├─ Program.cs                      (JWT, Swagger, versionado, middlewares)
│  ├─ Controllers/
│  │  ├─ PropertiesController.cs
│  │  └─ OwnersController.cs
│  ├─ Contracts/
│  │  ├─ Requests/…                   ┆
│  │  └─ Responses/…
│  ├─ Middleware/                     (ErrorHandlingMiddleware.cs)
│  └─ Services/Images/                (FileSystemImageStorage.cs → wwwroot/images)
│
└─ tests/Million.Tests/
   ├─ Domain/                         (PropertyAggregateTests.cs)
   ├─ Application/
   │  ├─ Handlers/                    (CreatePropertyHandlerTests, ChangePropertyPriceHandlerTests)
   │  └─ Mapping/                     (MappingProfileTests.cs)
   └─ Infrastructure/                 (PropertyReadRepositoryTests.cs - Sqlite in-memory)
```
</details>




## Restricciones clave (EF Core)

Property.CodeInternal único global (índice único).

Índices: Address.City, Address.State, PropertyTrace.DateSale.

Owned types: Address y Money mapeados a columnas planas.

Borrado en cascada para Property.Images y Property.Traces.

## 🚀 Puesta en marcha
1. Requisitos

.NET 8 SDK

SQL Server local (o Docker)

Visual Studio 2022 / VS Code

Docker SQL Server (opcional):

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Your_password123" \
  -p 1433:1433 --name sql -d mcr.microsoft.com/mssql/server:2022-latest

2. Configuración

src/Million.WebApi/appsettings.json

{
  "ConnectionStrings": {
    "MillionConnection": "Server=localhost,1433;Database=MillionDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Issuer": "Million",
    "Audience": "MillionAudience",
    "Key": "SúperClaveUltrasecreta_dev_!ChangeMe!",
    "ExpireMinutes": 120
  }
}

3. Migraciones y BD

Visual Studio – Package Manager Console
Proyecto de inicio: Million.WebApi · Proyecto predeterminado: Million.Infrastructure

Add-Migration Initial  -Project Million.Infrastructure -StartupProject Million.WebApi
Update-Database        -Project Million.Infrastructure -StartupProject Million.WebApi

4) Ejecutar
dotnet run --project src/Million.WebApi


Abrir Swagger: https://localhost:7224/swagger

## 🔐 Seguridad (JWT)

Todos los endpoints requieren Bearer token, salvo GET /api/v1/properties (anónimo).

En Swagger usa Authorize y pega solo el TOKEN.


## 🧾 Endpoints (v1)
Properties

GET /api/v1/properties — anónimo, filtros y paginación
Query: city, state, minPrice, maxPrice, minYear, maxYear, rooms, page=1, pageSize=20, orderBy=price_desc|price_asc|year_desc|year_asc|name_desc|name_asc
Devuelve PagedList<PropertyListItemDTO> con: Id, Name, Street, City, State, CodeInternal, Price, Currency, Year, Rooms, Images[], Traces[].

POST /api/v1/properties — [Authorize]

{
  "name": "Downtown Loft",
  "street": "1200 Market St",
  "city": "San Francisco",
  "state": "CA",
  "zip": "94103",
  "price": 735000,
  "currency": "USD",
  "codeInternal": "SF-LOFT-001",
  "year": 2016,
  "rooms": 2,
  "ownerId": "00000000-0000-0000-0000-000000000001"
}


Errores mapeados a ProblemDetails
property.duplicate_codeinternal → 409 · owner.not_found → 404 · otros de dominio → 422.

POST /api/v1/properties/{id}/images — [Authorize] (multipart/form-data)

File: archivo (imagen)

Enabled: true|false
Guarda en wwwroot/images y persiste PropertyImage.

POST /api/v1/properties/{id}/price — [Authorize]

{ "newAmount": 485000, "currency": "USD", "dateSale": "2025-08-15", "tax": 0 }


Registra PropertyTrace y dispara PriceChangedDomainEvent.

PUT /api/v1/properties/{id} — [Authorize]
Actualiza datos principales (nombre, dirección, año, rooms).

GET /health — health checks (DB + proceso).

Owners

GET /api/v1/owners — [Authorize]
Lista todos los owners (OwnerDTO: Id, Name, Address?, Photo?, Birthday?).

GET /api/v1/owners/with-properties — [Authorize]
Solo propietarios con propiedades.
OwnerDTO incluye: PropertiesCount y Properties[] (Id, Name, CodeInternal).

Relación: Owner 1..N Property (FK Property.OwnerId).

## 🧠 Manejo de errores (ProblemDetails)

En PropertiesController (y el middleware global) se mapean códigos de dominio a HTTP:

"property.not_found" → 404

"property.duplicate_codeinternal" → 409

Otros dominios/validación → 422

Excepciones no controladas → 500 (https://million/errors/unexpected)

📸 Subida de imágenes

Carpeta: src/Million.WebApi/wwwroot/images

Servicio: FileSystemImageStorage

Límite por request: 5 MB

Probar en Swagger: seleccionar multipart/form-data, campo File, Enabled=true.

## 📈 Performance y decisiones

AsNoTracking en queries.

Proyección server-side con AutoMapper.ProjectTo (solo columnas necesarias).

Paginación + orden dinámico.

Índices en columnas de consulta.

Backing fields (UsePropertyAccessMode(Field)) para colecciones.

## ✅ Unit Tests

Proyectos y alcance

Domain

PropertyAggregateTests → comportamiento de ChangePrice (actualiza precio, agrega traza).

Application / Handlers

CreatePropertyHandlerTests

NotFound si OwnerId no existe.

Conflict si CodeInternal duplicado.

Crea OK y confirma UnitOfWork.

ChangePropertyPriceHandlerTests

NotFound si Property no existe.

Actualiza precio, agrega traza y guarda.

Application / Mapping

MappingProfileTests → AssertConfigurationIsValid() (todos los mapeos correctos).

Infrastructure

PropertyReadRepositoryTests (Sqlite in-memory)

Filtra/ordena/pagina y proyecta a DTO (incluye Images y Traces).

Cómo ejecutarlos

# Ejecución normal
dotnet test -c Release

# Con reporte TRX
dotnet test -c Release --logger "trx;LogFileName=TestResults.trx"

# Con cobertura (Coverlet)
dotnet test -c Release /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura


Resultado esperado
10/10 pruebas en verde.

## 🔎 Swagger: ejemplos de creación
//1. NYC, USD, Owner 1
{ "name":"Downtown Flat","street":"1 Main St","city":"New York","state":"NY","zip":"10001",
  "price":450000,"currency":"USD","codeInternal":"NYC-DW-001","year":2010,"rooms":2,
  "ownerId":"00000000-0000-0000-0000-000000000001" }

//2. Miami
{ "name":"Brickell Condo","street":"200 SE 3rd Ave","city":"Miami","state":"FL","zip":"33131",
  "price":520000,"currency":"USD","codeInternal":"MIA-CON-002","year":2018,"rooms":3,
  "ownerId":"00000000-0000-0000-0000-000000000001" }

//3. SF
{ "name":"Sunset Loft","street":"1200 Market St","city":"San Francisco","state":"CA","zip":"94103",
  "price":735000,"currency":"USD","codeInternal":"SF-LOFT-001","year":2016,"rooms":2,
  "ownerId":"00000000-0000-0000-0000-000000000002" }


Nota: CodeInternal es único. Si se repite, el API devuelve 409 con detalle.


## ✍️ Notas finales

El Dominio permanece libre de frameworks, lo que permite pruebas rápidas y confiables.

Application orquesta con Result<T> sin excepciones como control de flujo.

Infrastructure concentra EF Core, mapeos y repos.

WebApi entrega: ProblemDetails, Swagger, JWT, health y versión.

Tests cubren núcleo de negocio y acceso (lectura) con repositorio especializado.
