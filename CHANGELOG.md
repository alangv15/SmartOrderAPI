# Historial de cambios

El formato se basa en [Keep a Changelog](https://keepachangelog.com/es-ES/1.1.0/) y el proyecto sigue [Versionado Semántico](https://semver.org/lang/es/).

## [1.1.0] - 2026-09-19

### Agregado

- API de conceptos de costo y costos efectivos por fecha.
- API de recetas base y recetas versionadas de producto.
- Cálculo del costo vigente de productos y detección de información faltante.
- Reporte mensual de seis meses con ingresos, efectivo, tarjeta, costos, utilidad bruta y margen bruto.
- Contratos para conservar precio, receta y costo histórico en las partidas de venta y pedido.

### Cambiado

- El resumen acumulado integra ventas de mostrador y pedidos.
- Los servicios de ventas y pedidos calculan y almacenan sus valores económicos al registrar la operación.
- Los mapeos y repositorios incluyen la información de costos y utilidad.
- La versión de todos los ensamblados se centralizó y actualizó a 1.1.0.

### Corregido

- Selección de la receta y del costo aplicables según la fecha de la operación.
- Cálculo de rentabilidad para operaciones históricas que ya cuentan con costos reprocesados.

## [1.0.0] - 2026-04-07

### Agregado

- Primera versión de la API de SmartOrder.
- Servicios de catálogos, ventas, pedidos, descuentos y seguridad.
- Persistencia con Entity Framework Core y SQL Server.
- Reportes operativos iniciales consumidos por la aplicación de escritorio.

