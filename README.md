# .NET Code Maturity Lab

Laboratorio visual e interactivo de Red & Black Codeworkers que muestra cómo una solución .NET evoluciona desde fundamentos de desarrollo hasta decisiones de arquitectura y operación en producción.

El proyecto utiliza un único caso práctico —una API de procesamiento de órdenes— para comparar tres niveles:

- **Junior:** C#, POO, validación, LINQ, REST, HTTP y pruebas unitarias con AAA.
- **Middle:** SOLID, inyección de dependencias, patrones justificados, refactorización y pruebas de integración.
- **Senior:** idempotencia, concurrencia, resiliencia, observabilidad, seguridad y escalabilidad en Azure.

## Estado

El repositorio contiene el cascarón visual inicial. Las páginas y ejemplos actuales sirven para validar navegación, identidad y composición; todavía no representan el contenido definitivo de los ejercicios.

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

La aplicación es pública y no utiliza autenticación, usuarios ni base de datos en esta etapa.
