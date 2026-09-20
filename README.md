# SmartOrder API

API HTTP de SmartOrder. Centraliza la operación de ventas y pedidos, catálogos, seguridad, costos, recetas y reportes consumidos por las aplicaciones cliente.

**Versión actual:** 1.1.0  
**Rama de entrega:** `release/1.1.0-costos-reportes`

## Capacidades

- Catálogos de productos, categorías, sucursales y clientes.
- Ventas, pedidos, partidas, descuentos y formas de pago.
- Usuarios, roles, permisos y acceso por sucursal.
- Conceptos de costo, recetas base, recetas de producto y precios históricos.
- Cálculo de costos y captura de valores históricos en las partidas.
- Reportes de ventas acumuladas y rentabilidad mensual.

## Arquitectura

| Proyecto | Responsabilidad |
| --- | --- |
| `SmartOrderAPI` | Host ASP.NET Core, controladores y configuración. |
| `SmartOrderAPI.Business` | Reglas y servicios de negocio. |
| `SmartOrderAPI.Data` | Entity Framework Core, contexto, modelos y repositorios. |
| `SmartOrderAPI.Entities` | DTO y contratos públicos. |
| `Tests/CostingChecks` | Comprobaciones del modelo de costos. |

## Requisitos

- .NET SDK 10.
- SQL Server compatible con el esquema de `SmartOrderDB`.
- Una cadena de conexión válida para la base SmartOrder.

## Configuración

1. Copiar `SmartOrderAPI/appsettings.example.json` como `SmartOrderAPI/appsettings.json`.
2. Configurar `ConnectionStrings:SmartOrderConnection`.
3. Revisar la zona horaria y el inicio del día de negocio en `BusinessTime`.

Use secretos de usuario, variables de ambiente o la configuración segura del servidor para credenciales. No incluya contraseñas productivas en Git.

## Compilación y ejecución

```powershell
dotnet restore SmartOrderAPI.sln
dotnet build SmartOrderAPI.sln -c Release
dotnet run --project SmartOrderAPI/SmartOrderAPI.csproj
```

Swagger está disponible cuando el ambiente es `Development`.

## Comprobaciones de costos

```powershell
dotnet run --project Tests/CostingChecks/CostingChecks.csproj -c Release
```

## Publicación

```powershell
dotnet publish SmartOrderAPI/SmartOrderAPI.csproj -c Release -o ./publish
```

Para una liberación en IIS:

1. Compilar desde una copia limpia y respaldar la publicación activa.
2. Conservar la configuración productiva fuera del paquete generado.
3. Colocar temporalmente `app_offline.htm` durante el reemplazo de archivos.
4. Copiar todos los artefactos y retirar `app_offline.htm`.
5. Verificar catálogos, costos, recetas, pedidos y reportes mediante HTTP.

## Versionado

Todos los ensamblados reciben la versión desde `Directory.Build.props`. Se utiliza versionado semántico `MAJOR.MINOR.PATCH`; los contratos incompatibles requieren una versión mayor.

Consulte [CHANGELOG.md](CHANGELOG.md) para conocer los cambios de cada entrega.

## Repositorios relacionados

- `SmartOrder`: aplicación de escritorio y clientes.
- `SmartOrderDB`: esquema, migraciones y cargas de datos.

