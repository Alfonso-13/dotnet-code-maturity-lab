using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

/// <summary>
/// Bilingual learning specification for the Middle path. These plans define
/// what each module must teach before lesson panes, interview options, or the
/// capstone implementation are written.
/// </summary>
public static class MiddleCurriculum
{
    public static IReadOnlyList<CurriculumModulePlan> Modules { get; } =
    [
        Module(
            "solid",
            Text(
                "Refactor the order flow by assigning each reason to change to the correct abstraction.",
                "Refactoriza el flujo de órdenes asignando cada razón de cambio a la abstracción correcta."),
            [
                Text("Single responsibility and cohesion", "Responsabilidad única y cohesión"),
                Text("Open/closed design through stable extension points", "Diseño abierto/cerrado mediante puntos de extensión estables"),
                Text("Substitutability, focused interfaces, and dependency inversion", "Sustitución, interfaces enfocadas e inversión de dependencias")
            ],
            Text(
                "Apply a SOLID principle only when a demonstrated change or testability problem justifies the extra boundary.",
                "Aplica un principio SOLID solo cuando un problema demostrado de cambio o comprobabilidad justifique el límite adicional."),
            Perspectives(
                Text("S — separate independent reasons to change.", "S — separa razones independientes de cambio."),
                Text("O — extend a demonstrated variation without editing stable policy.", "O — extiende una variación demostrada sin editar la política estable."),
                Text("L — ensure every substitute preserves the promised behavior.", "L — asegura que cada sustituto conserve la conducta prometida."),
                Text("I — shape focused interfaces around consumer needs.", "I — diseña interfaces enfocadas según las necesidades del consumidor."),
                Text("D — make high-level policy depend on application-owned abstractions.", "D — haz que la política de alto nivel dependa de abstracciones propias de la aplicación.")),
            Text(
                "Name the specific change that hurts before introducing an interface or a new class.",
                "Nombra el cambio específico que duele antes de introducir una interfaz o una clase nueva."),
            Text(
                "Split validation, pricing, persistence, and notification responsibilities while preserving existing tests.",
                "Separa validación, precios, persistencia y notificación conservando las pruebas existentes."),
            [
                Text("Which SOLID principle addresses this concrete design problem, and what does the refactoring cost?", "¿Qué principio SOLID resuelve este problema concreto y cuál es el costo de la refactorización?"),
                Text("When would applying SOLID make a small service harder to understand?", "¿Cuándo aplicar SOLID haría un servicio pequeño más difícil de entender?")
            ]),

        Module(
            "inyeccion-dependencias",
            Text(
                "Compose replaceable collaborators with explicit lifetimes and keep business code independent of the container.",
                "Compón colaboradores reemplazables con ciclos de vida explícitos y mantén el negocio independiente del contenedor."),
            [
                Text("Constructor injection and explicit dependencies", "Inyección por constructor y dependencias explícitas"),
                Text("Transient, scoped, and singleton lifetimes", "Ciclos de vida transient, scoped y singleton"),
                Text("Composition root, factories, and lifetime safety", "Raíz de composición, fábricas y seguridad de ciclos de vida")
            ],
            Text(
                "Choose the narrowest lifetime that matches ownership and never let a longer-lived service capture a shorter-lived dependency.",
                "Elige el ciclo de vida más acotado que corresponda al propietario y nunca permitas que un servicio más largo capture una dependencia más corta."),
            Perspectives(
                Text("Trace how ASP.NET Core resolves the order collaborators.", "Sigue cómo ASP.NET Core resuelve los colaboradores de órdenes."),
                Text("Compare constructor injection, factories, and service location.", "Compara inyección por constructor, fábricas y localización de servicios."),
                Text("Assign lifetimes from state ownership and concurrency requirements.", "Asigna ciclos de vida según propiedad del estado y concurrencia."),
                Text("Override boundaries in tests without rebuilding application logic.", "Sustituye límites en pruebas sin reconstruir la lógica de la aplicación."),
                Text("Build the order composition root and detect a captive dependency.", "Construye la raíz de composición de órdenes y detecta una dependencia cautiva.")),
            Text(
                "A lifetime describes ownership and sharing, not performance by itself.",
                "Un ciclo de vida describe propiedad y uso compartido, no rendimiento por sí mismo."),
            Text(
                "Register the refactored order services, replace one adapter in a test host, and explain every lifetime.",
                "Registra los servicios refactorizados, sustituye un adaptador en un host de pruebas y explica cada ciclo de vida."),
            [
                Text("Why cannot a singleton safely depend directly on a scoped DbContext?", "¿Por qué un singleton no puede depender directamente de un DbContext scoped?"),
                Text("When is a factory clearer than injecting another service directly?", "¿Cuándo una fábrica es más clara que inyectar otro servicio directamente?")
            ]),

        Module(
            "strategy-patrones",
            Text(
                "Model a real variation in order behavior with Strategy and distinguish useful patterns from speculative abstractions.",
                "Modela una variación real del comportamiento de órdenes con Strategy y distingue patrones útiles de abstracciones especulativas."),
            [
                Text("Strategy as interchangeable behavior", "Strategy como comportamiento intercambiable"),
                Text("Factory selection at an application boundary", "Selección mediante fábrica en un límite de aplicación"),
                Text("Pattern intent, context, and implementation cost", "Intención, contexto y costo de un patrón")
            ],
            Text(
                "Introduce a pattern when the variation is real, named, and likely to evolve independently.",
                "Introduce un patrón cuando la variación sea real, tenga nombre y pueda evolucionar de manera independiente."),
            Perspectives(
                Text("Locate conditional behavior that represents a business variation.", "Ubica conducta condicional que representa una variación del negocio."),
                Text("Compare a switch, a strategy map, and polymorphic implementations.", "Compara un switch, un mapa de estrategias e implementaciones polimórficas."),
                Text("Design a small strategy contract around the order use case.", "Diseña un contrato Strategy pequeño alrededor del caso de órdenes."),
                Text("Test each strategy and the selection rule independently.", "Prueba cada estrategia y la regla de selección por separado."),
                Text("Add a pricing or notification strategy without modifying the orchestrator.", "Agrega una estrategia de precio o notificación sin modificar el orquestador.")),
            Text(
                "Keep selection separate from execution; otherwise the conditional simply moves to another class.",
                "Separa selección de ejecución; de otro modo el condicional solo cambia de clase."),
            Text(
                "Replace a channel switch with strategies and prove a new channel can be added through registration.",
                "Reemplaza un switch de canal con estrategias y demuestra que un canal nuevo se agrega mediante registro."),
            [
                Text("Why is Strategy preferable to a switch in this case, and when would the switch be enough?", "¿Por qué Strategy es preferible a un switch en este caso y cuándo sería suficiente el switch?"),
                Text("Where should strategy selection happen?", "¿Dónde debe ocurrir la selección de la estrategia?")
            ]),

        Module(
            "refactorizacion",
            Text(
                "Change structure safely through small steps protected by observable tests and explicit refactoring goals.",
                "Cambia la estructura de forma segura mediante pasos pequeños protegidos por pruebas observables y objetivos explícitos."),
            [
                Text("Characterization tests and behavior preservation", "Pruebas de caracterización y preservación de conducta"),
                Text("Code smells versus demonstrated design pressure", "Code smells frente a presión de diseño demostrada"),
                Text("Small transformations, commits, and rollback points", "Transformaciones pequeñas, commits y puntos de retorno")
            ],
            Text(
                "Separate behavior changes from structural changes so failures reveal what actually changed.",
                "Separa los cambios de conducta de los cambios estructurales para que los fallos revelen qué cambió realmente."),
            Perspectives(
                Text("Describe existing behavior before changing the implementation.", "Describe la conducta existente antes de cambiar la implementación."),
                Text("Compare a rewrite with incremental, test-protected refactoring.", "Compara una reescritura con una refactorización incremental protegida por pruebas."),
                Text("Plan seams and extraction steps around the highest-risk coupling.", "Planea seams y extracciones alrededor del acoplamiento de mayor riesgo."),
                Text("Use characterization and focused unit tests as a safety net.", "Usa pruebas de caracterización y unitarias enfocadas como red de seguridad."),
                Text("Refactor the order service in reviewable steps with unchanged outputs.", "Refactoriza el servicio de órdenes en pasos revisables con salidas sin cambios.")),
            Text(
                "If a step cannot be explained and reviewed independently, it is probably too large.",
                "Si un paso no puede explicarse y revisarse de forma independiente, probablemente es demasiado grande."),
            Text(
                "Create characterization tests, perform three small extractions, and show the before/after dependency graph.",
                "Crea pruebas de caracterización, realiza tres extracciones pequeñas y muestra el grafo de dependencias antes/después."),
            [
                Text("How do you refactor code that has no tests?", "¿Cómo refactorizas código que no tiene pruebas?"),
                Text("What evidence proves a refactoring preserved behavior?", "¿Qué evidencia demuestra que una refactorización preservó la conducta?")
            ]),

        Module(
            "ef-core",
            Text(
                "Design EF Core persistence around query intent, transaction boundaries, and observable database behavior.",
                "Diseña persistencia con EF Core alrededor de la intención de consulta, límites transaccionales y conducta observable de base de datos."),
            [
                Text("Tracking, AsNoTracking, projections, and query shape", "Tracking, AsNoTracking, proyecciones y forma de consulta"),
                Text("Relationships, migrations, and transaction boundaries", "Relaciones, migraciones y límites transaccionales"),
                Text("N+1 queries, generated SQL, and database indexes", "Consultas N+1, SQL generado e índices de base de datos")
            ],
            Text(
                "Shape each query for its use case and measure generated SQL before adding repository abstractions or performance fixes.",
                "Da forma a cada consulta según su caso y mide el SQL generado antes de agregar abstracciones o correcciones de rendimiento."),
            Perspectives(
                Text("Trace DbContext tracking and the SQL produced by an order query.", "Sigue el tracking de DbContext y el SQL producido por una consulta de órdenes."),
                Text("Compare entities, projections, eager loading, and explicit loading.", "Compara entidades, proyecciones, carga eager y carga explícita."),
                Text("Define aggregate persistence and transaction boundaries.", "Define límites de persistencia y transacción del agregado."),
                Text("Verify queries against SQLite and inspect command counts.", "Comprueba consultas contra SQLite e inspecciona la cantidad de comandos."),
                Text("Persist an order with related data without an N+1 query.", "Persiste una orden con datos relacionados sin una consulta N+1.")),
            Text(
                "LINQ readability does not guarantee efficient SQL; inspect the translation and data volume.",
                "La legibilidad de LINQ no garantiza SQL eficiente; inspecciona la traducción y el volumen de datos."),
            Text(
                "Implement a projected order-detail query, migration, and integration test that detects an N+1 regression.",
                "Implementa una consulta proyectada de detalle, una migración y una prueba de integración que detecte una regresión N+1."),
            [
                Text("When should a query use AsNoTracking and projection?", "¿Cuándo debe una consulta usar AsNoTracking y proyección?"),
                Text("How would you diagnose and fix an N+1 problem?", "¿Cómo diagnosticarías y corregirías un problema N+1?")
            ]),

        Module(
            "problem-details",
            Text(
                "Translate domain, validation, and infrastructure failures into stable ProblemDetails contracts at the HTTP boundary.",
                "Traduce fallos de dominio, validación e infraestructura a contratos ProblemDetails estables en el límite HTTP."),
            [
                Text("Expected errors versus unexpected exceptions", "Errores esperados frente a excepciones inesperadas"),
                Text("ProblemDetails, validation details, and status mapping", "ProblemDetails, detalles de validación y mapeo de estados"),
                Text("Centralized exception handling and safe diagnostics", "Manejo centralizado de excepciones y diagnósticos seguros")
            ],
            Text(
                "Map errors once at the transport boundary and never leak stack traces or persistence details to clients.",
                "Mapea errores una sola vez en el límite de transporte y nunca expongas stack traces ni detalles de persistencia al cliente."),
            Perspectives(
                Text("Classify failures by ownership and recoverability.", "Clasifica fallos según responsabilidad y posibilidad de recuperación."),
                Text("Compare ad-hoc JSON errors with consistent ProblemDetails responses.", "Compara errores JSON improvisados con respuestas ProblemDetails consistentes."),
                Text("Design an application result and centralized HTTP translation.", "Diseña un resultado de aplicación y traducción HTTP centralizada."),
                Text("Verify status, type, title, extensions, and absence of sensitive details.", "Comprueba status, type, title, extensiones y ausencia de detalles sensibles."),
                Text("Handle validation, conflict, not found, and unexpected failure paths.", "Maneja rutas de validación, conflicto, ausencia y fallo inesperado.")),
            Text(
                "The same domain error must produce the same HTTP contract regardless of the endpoint that exposes it.",
                "El mismo error de dominio debe producir el mismo contrato HTTP sin importar el endpoint que lo exponga."),
            Text(
                "Add centralized exception handling and contract tests for 400, 404, 409, and 500 responses.",
                "Agrega manejo centralizado de excepciones y pruebas de contrato para respuestas 400, 404, 409 y 500."),
            [
                Text("Where should exceptions become HTTP responses?", "¿Dónde deben convertirse las excepciones en respuestas HTTP?"),
                Text("What information belongs in ProblemDetails, and what must remain internal?", "¿Qué información pertenece a ProblemDetails y cuál debe permanecer interna?")
            ]),

        Module(
            "seguridad-jwt",
            Text(
                "Design how bearer-token validation and claim policies would protect the order API.",
                "Diseña cómo la validación de tokens bearer y las políticas de claims protegerían la API de órdenes."),
            [
                Text("JWT signature, issuer, audience, and expiration validation", "Validación de firma, issuer, audience y expiración del JWT"),
                Text("ClaimsPrincipal, roles, claims, and named authorization policies", "ClaimsPrincipal, roles, claims y políticas de autorización con nombre"),
                Text("401 authentication failures versus 403 authorization failures", "Fallos 401 de autenticación frente a fallos 403 de autorización")
            ],
            Text(
                "Authenticate the caller before evaluating application-owned permission policies; never trust an unvalidated token payload.",
                "Autentica al solicitante antes de evaluar políticas de permisos propias de la aplicación; nunca confíes en el payload de un token sin validar."),
            Perspectives(
                Text("Trace how bearer authentication validates a token and creates ClaimsPrincipal.", "Sigue cómo la autenticación bearer valida un token y crea ClaimsPrincipal."),
                Text("Compare roles, raw claims, and policies as authorization models.", "Compara roles, claims directos y políticas como modelos de autorización."),
                Text("Design a named orders.write policy around a permission claim.", "Diseña una política orders.write con nombre alrededor de un claim de permiso."),
                Text("Compare the expected 401, 403, and 201 HTTP contracts in illustrative tests.", "Compara los contratos HTTP esperados 401, 403 y 201 en pruebas ilustrativas."),
                Text("Design a protected create-order endpoint without leaking token or identity details.", "Diseña un endpoint protegido de creación de órdenes sin filtrar detalles del token o la identidad.")),
            Text(
                "A valid token proves identity; a satisfied policy proves permission. They are related decisions, not synonyms.",
                "Un token válido demuestra identidad; una política satisfecha demuestra permiso. Son decisiones relacionadas, no sinónimos."),
            Text(
                "Explain a proposed JWT bearer configuration and orders.write policy, then specify the integration tests needed to prove 401, 403, and 201 in a future protected API.",
                "Explica una configuración propuesta de JWT bearer y la política orders.write; después define las pruebas de integración necesarias para demostrar 401, 403 y 201 en una futura API protegida."),
            [
                Text("Which JWT properties must the API validate before trusting its claims?", "¿Qué propiedades del JWT debe validar la API antes de confiar en sus claims?"),
                Text("Why does a valid token sometimes produce 403 instead of 401?", "¿Por qué un token válido produce a veces 403 en lugar de 401?")
            ]),

        Module(
            "pruebas-integracion",
            Text(
                "Verify real HTTP, dependency-injection, serialization, and persistence collaboration with focused integration tests.",
                "Comprueba la colaboración real de HTTP, inyección de dependencias, serialización y persistencia mediante pruebas de integración enfocadas."),
            [
                Text("WebApplicationFactory and test-host composition", "WebApplicationFactory y composición del host de pruebas"),
                Text("Isolated databases and deterministic fixtures", "Bases aisladas y fixtures deterministas"),
                Text("Contract assertions and useful test boundaries", "Aserciones de contrato y límites útiles de prueba")
            ],
            Text(
                "Use integration tests for collaboration risk and unit tests for decision detail; do not duplicate every scenario at every layer.",
                "Usa pruebas de integración para riesgo de colaboración y unitarias para detalle de decisiones; no dupliques cada escenario en cada capa."),
            Perspectives(
                Text("Identify what a unit test cannot prove about the running API.", "Identifica qué no puede demostrar una prueba unitaria sobre la API en ejecución."),
                Text("Compare mocked controllers with an in-process HTTP test host.", "Compara controladores simulados con un host HTTP en proceso."),
                Text("Design isolated fixtures and replace only true external boundaries.", "Diseña fixtures aislados y sustituye solo límites externos reales."),
                Text("Assert status, headers, payload, persistence, and side effects.", "Comprueba estado, encabezados, payload, persistencia y efectos secundarios."),
                Text("Build the critical create-order success and failure journeys.", "Construye los recorridos críticos de éxito y fallo al crear órdenes.")),
            Text(
                "A test that only calls a service is not an HTTP integration test, even if the service uses a database.",
                "Una prueba que solo llama un servicio no es una prueba de integración HTTP, aunque el servicio use una base de datos."),
            Text(
                "Create a WebApplicationFactory suite covering creation, validation, conflict, persistence, and ProblemDetails.",
                "Crea una suite con WebApplicationFactory que cubra creación, validación, conflicto, persistencia y ProblemDetails."),
            [
                Text("What should an HTTP integration test verify beyond the status code?", "¿Qué debe comprobar una prueba de integración HTTP además del código de estado?"),
                Text("How do you keep integration tests isolated and deterministic?", "¿Cómo mantienes las pruebas de integración aisladas y deterministas?")
            ]),

        Module(
            "limites-arquitectura",
            Text(
                "Place business rules, application orchestration, infrastructure adapters, and HTTP concerns behind explicit dependency boundaries.",
                "Ubica reglas de negocio, orquestación de aplicación, adaptadores de infraestructura y HTTP detrás de límites de dependencia explícitos."),
            [
                Text("Domain, application, infrastructure, and presentation responsibilities", "Responsabilidades de dominio, aplicación, infraestructura y presentación"),
                Text("Dependency direction and ports/adapters", "Dirección de dependencias y puertos/adaptadores"),
                Text("Pragmatic boundaries versus ceremonial projects", "Límites pragmáticos frente a proyectos ceremoniales")
            ],
            Text(
                "Create a boundary to protect a meaningful policy or volatile dependency, not merely to imitate a folder diagram.",
                "Crea un límite para proteger una política importante o una dependencia volátil, no solo para imitar un diagrama de carpetas."),
            Perspectives(
                Text("Map responsibilities and current dependency directions.", "Mapea responsabilidades y direcciones actuales de dependencia."),
                Text("Compare layered, feature-oriented, and ports-and-adapters arrangements.", "Compara organización por capas, por feature y por puertos y adaptadores."),
                Text("Choose boundaries for the order case and justify each one.", "Elige límites para el caso de órdenes y justifica cada uno."),
                Text("Enforce dependency direction with tests and project references.", "Protege la dirección de dependencias con pruebas y referencias de proyecto."),
                Text("Move HTTP and EF Core details outside the application decision flow.", "Mueve detalles HTTP y EF Core fuera del flujo de decisiones de aplicación.")),
            Text(
                "A clean dependency graph matters more than the number of projects in the solution.",
                "Un grafo limpio de dependencias importa más que la cantidad de proyectos en la solución."),
            Text(
                "Draw the before/after dependency graph and refactor one endpoint through explicit application and infrastructure boundaries.",
                "Dibuja el grafo de dependencias antes/después y refactoriza un endpoint con límites explícitos de aplicación e infraestructura."),
            [
                Text("Which direction should dependencies point, and why?", "¿En qué dirección deben apuntar las dependencias y por qué?"),
                Text("When is a separate layer or project unnecessary?", "¿Cuándo es innecesaria una capa o un proyecto separado?")
            ]),

        Module(
            "ia-integracion",
            Text(
                "Integrate an LLM behind a replaceable application boundary with structured output, cancellation, tests, and deterministic fallback.",
                "Integra un LLM detrás de un límite reemplazable de aplicación con salida estructurada, cancelación, pruebas y fallback determinista."),
            [
                Text("Provider-independent AI service boundaries", "Límites de servicio de IA independientes del proveedor"),
                Text("Structured output, validation, timeout, and cancellation", "Salida estructurada, validación, timeout y cancelación"),
                Text("Fakes, contract tests, fallback, and human review", "Fakes, pruebas de contrato, fallback y revisión humana")
            ],
            Text(
                "Treat the model as an unreliable external collaborator: constrain its task, validate its output, and preserve a deterministic path.",
                "Trata el modelo como un colaborador externo no confiable: limita su tarea, valida su salida y conserva una ruta determinista."),
            Perspectives(
                Text("Trace the Junior AI suggestion boundary and identify its missing operational contracts.", "Sigue el límite de sugerencias IA de Junior e identifica sus contratos operativos faltantes."),
                Text("Compare direct SDK coupling with a provider-independent application interface.", "Compara acoplamiento directo al SDK con una interfaz de aplicación independiente del proveedor."),
                Text("Design structured input/output, validation, timeout, and fallback policies.", "Diseña entrada/salida estructurada, validación, timeout y políticas de fallback."),
                Text("Verify behavior with deterministic fakes and provider contract tests.", "Comprueba conducta con fakes deterministas y pruebas de contrato del proveedor."),
                Text("Add an optional order-note classification without giving the model domain authority.", "Agrega una clasificación opcional de notas sin entregar autoridad de dominio al modelo.")),
            Text(
                "The UI and domain must not know which model provider implements the AI boundary.",
                "La UI y el dominio no deben saber qué proveedor implementa el límite de IA."),
            Text(
                "Replace the deterministic Junior provider through IAiOrderSuggestionService, validate structured output, and prove timeout fallback with tests.",
                "Sustituye el proveedor determinista de Junior mediante IAiOrderSuggestionService, valida la salida estructurada y demuestra el fallback por timeout con pruebas."),
            [
                Text("How do you keep an LLM integration testable and provider-independent?", "¿Cómo mantienes una integración LLM comprobable e independiente del proveedor?"),
                Text("What should happen when the model times out or returns invalid structured output?", "¿Qué debe ocurrir cuando el modelo excede el tiempo o devuelve salida estructurada inválida?")
            ])
    ];

    public static CurriculumModulePlan Get(string slug) =>
        Modules.First(module => module.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));

    private static CurriculumModulePlan Module(
        string slug,
        LocalizedText objective,
        IReadOnlyList<LocalizedText> concepts,
        LocalizedText decision,
        IReadOnlyList<CurriculumPerspectiveDefinition> perspectives,
        LocalizedText hint,
        LocalizedText finalExercise,
        IReadOnlyList<LocalizedText> interviewQuestions) =>
        new(slug, objective, concepts, decision, perspectives, hint, finalExercise, interviewQuestions);

    private static IReadOnlyList<CurriculumPerspectiveDefinition> Perspectives(
        LocalizedText understand,
        LocalizedText compare,
        LocalizedText design,
        LocalizedText verify,
        LocalizedText integrate) =>
        [
            new("understand", Text("Understand", "Entiende"), understand),
            new("compare", Text("Compare", "Compara"), compare),
            new("design", Text("Design", "Diseña"), design),
            new("verify", Text("Verify", "Comprueba"), verify),
            new("integrate", Text("Integrate", "Integra"), integrate)
        ];

    private static LocalizedText Text(string english, string spanish) => new(english, spanish);
}
