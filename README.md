# .NET Code Maturity Lab

Laboratorio visual e interactivo de Red & Black Codeworkers que muestra cómo una solución .NET evoluciona desde fundamentos de desarrollo hasta decisiones de arquitectura y operación en producción.

El proyecto utiliza un único caso práctico —una API de procesamiento de órdenes— para comparar tres niveles:

- **Junior:** C#, POO, validación, colecciones, LINQ, asincronía, pruebas AAA, REST, EF Core y fundamentos de ASP.NET Core.
- **Middle:** SOLID, inyección de dependencias, patrones justificados, refactorización y pruebas de integración.
- **Senior:** idempotencia, concurrencia, resiliencia, observabilidad, seguridad y escalabilidad en Azure.

## Estado

El nivel Junior contiene doce módulos guiados antes de aplicar los conceptos en la API. Cada módulo reúne concepto, comparación visual, código y pistas dentro de una misma pantalla.

La ruta de madurez separa el aprendizaje Junior, Middle y Senior. Cada nivel muestra su avance guardado únicamente en `localStorage`; los laboratorios son ejercicios independientes asociados a cada nivel y no requieren una cuenta.

La ruta comienza en `/aprender/junior`. La práctica independiente está en `/laboratorio/junior` e incluye endpoints reales `GET /api/orders`, `GET /api/orders/{id}` y `POST /api/orders`, validación, persistencia local con EF Core y SQLite, demostración interactiva, pruebas unitarias con AAA y pruebas de integración HTTP.

## Ejecutar localmente

Requiere el SDK de .NET 10.

Para iniciar la aplicación y abrirla automáticamente en el navegador:

```powershell
.\start-local.ps1
```

También puedes hacer doble clic en `start-local.cmd` desde el Explorador de archivos.

El script utiliza `http://localhost:5187` y mantiene el servidor en la consola. Presiona `Ctrl+C` para detenerlo.

También puedes ejecutarla directamente con la CLI:

```powershell
dotnet run
```

La aplicación es pública y no utiliza autenticación ni usuarios. El laboratorio Junior crea `App_Data/orders.db` al iniciar y conserva las órdenes localmente mediante EF Core y SQLite; este archivo no se versiona.

## Pruebas

Abre `DotnetCodeMaturityLab.slnx` en Visual Studio para trabajar con la aplicación y el proyecto de pruebas.

```powershell
dotnet test DotnetCodeMaturityLab.slnx
```
