using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

/// <summary>
/// Interview decisions for completed Middle lessons. Questions are added only
/// after their lesson exists so the simulator never promises uncovered topics.
/// </summary>
public static class MiddleInterviewCatalog
{
    public static IReadOnlyList<InterviewQuestion> Get(AppLanguage language) =>
    [
        Question(language, "solid",
            "OrderService validates, calculates, persists, and sends email. What is the best first refactoring?",
            "OrderService valida, calcula, persiste y envía correo. ¿Cuál es la mejor primera refactorización?",
            [
                Option(language, "Extract the responsibility with an independent reason to change", "Extraer la responsabilidad con una razón independiente de cambio", true,
                    "This creates a justified boundary while preserving the use case.", "Esto crea un límite justificado mientras conserva el caso de uso."),
                Option(language, "Create one interface for every method", "Crear una interfaz por cada método", false,
                    "More interfaces do not automatically improve cohesion or changeability.", "Más interfaces no mejoran automáticamente cohesión o capacidad de cambio."),
                Option(language, "Rewrite the endpoint before adding tests", "Reescribir el endpoint antes de agregar pruebas", false,
                    "A rewrite combines structural and behavioral risk without a safety net.", "Una reescritura combina riesgo estructural y de conducta sin red de seguridad.")
            ]),
        Question(language, "inyeccion-dependencias",
            "Why is injecting a scoped DbContext into a singleton unsafe?",
            "¿Por qué es inseguro inyectar un DbContext scoped en un singleton?",
            [
                Option(language, "The singleton captures request-owned mutable state beyond its lifetime", "El singleton captura estado mutable de la solicitud más allá de su ciclo de vida", true,
                    "The lifetime mismatch can share stale or disposed state across requests.", "La diferencia de ciclos puede compartir estado obsoleto o liberado entre solicitudes."),
                Option(language, "Singleton services cannot have constructors", "Los servicios singleton no pueden tener constructores", false,
                    "Singletons use constructors normally; ownership is the problem.", "Los singleton usan constructores normalmente; el problema es la propiedad."),
                Option(language, "DbContext can only be registered as transient", "DbContext solo puede registrarse como transient", false,
                    "Scoped is the common web lifetime because it matches one request.", "Scoped es el ciclo web común porque coincide con una solicitud.")
            ]),
        Question(language, "strategy-patrones",
            "When does Strategy provide more value than a switch?",
            "¿Cuándo aporta Strategy más valor que un switch?",
            [
                Option(language, "When behaviors evolve independently and have different dependencies", "Cuando las conductas evolucionan por separado y tienen dependencias distintas", true,
                    "The variation becomes an explicit extension point with focused tests.", "La variación se convierte en un punto de extensión explícito con pruebas enfocadas."),
                Option(language, "Whenever there are two branches", "Siempre que existan dos ramas", false,
                    "A small stable switch may be clearer than a pattern.", "Un switch pequeño y estable puede ser más claro que un patrón."),
                Option(language, "When all behavior must remain inside the selector", "Cuando toda la conducta debe permanecer dentro del selector", false,
                    "The selector chooses behavior; each strategy executes it.", "El selector elige la conducta; cada estrategia la ejecuta.")
            ]),
        Question(language, "refactorizacion",
            "How should you begin refactoring a service that has no useful tests?",
            "¿Cómo debes comenzar a refactorizar un servicio que no tiene pruebas útiles?",
            [
                Option(language, "Record observable behavior with characterization tests", "Registrar la conducta observable con pruebas de caracterización", true,
                    "This creates evidence before structural changes begin.", "Esto crea evidencia antes de iniciar cambios estructurales."),
                Option(language, "Rewrite the service and test the new version later", "Reescribir el servicio y probar la versión nueva después", false,
                    "A rewrite combines behavior and structure changes without a safety net.", "Una reescritura combina cambios de conducta y estructura sin red de seguridad."),
                Option(language, "Extract every private method into an interface", "Extraer cada método privado a una interfaz", false,
                    "Mechanical abstraction does not identify coherent responsibilities.", "La abstracción mecánica no identifica responsabilidades coherentes.")
            ]),
        Question(language, "ef-core",
            "A read-only endpoint loads a complete entity graph but returns three fields. What should you evaluate first?",
            "Un endpoint de solo lectura carga un grafo completo y devuelve tres campos. ¿Qué debes evaluar primero?",
            [
                Option(language, "Use AsNoTracking and project the required fields in the query", "Usar AsNoTracking y proyectar los campos requeridos en la consulta", true,
                    "Projection reduces transferred data and avoids unnecessary tracked entities.", "La proyección reduce datos transferidos y evita entidades rastreadas innecesarias."),
                Option(language, "Make the DbContext a singleton", "Convertir DbContext en singleton", false,
                    "A singleton creates unsafe state sharing and does not improve query shape.", "Un singleton comparte estado de forma insegura y no mejora la consulta."),
                Option(language, "Call SaveChanges after the query", "Llamar SaveChanges después de consultar", false,
                    "A read-only request has no changes to persist.", "Una solicitud de lectura no tiene cambios que guardar.")
            ]),
        Question(language, "problem-details",
            "Where should an application conflict become an HTTP 409 ProblemDetails response?",
            "¿Dónde debe convertirse un conflicto de aplicación en una respuesta HTTP 409 ProblemDetails?",
            [
                Option(language, "At the HTTP boundary through one reusable result mapper", "En el límite HTTP mediante un único mapper reutilizable", true,
                    "The application remains transport-independent and endpoints stay consistent.", "La aplicación permanece independiente del transporte y los endpoints son consistentes."),
                Option(language, "Inside the domain entity", "Dentro de la entidad de dominio", false,
                    "The domain should not depend on HTTP response types.", "El dominio no debe depender de tipos de respuesta HTTP."),
                Option(language, "Inside every repository catch block", "Dentro de cada catch del repositorio", false,
                    "Persistence should not own transport contracts or duplicate mappings.", "Persistencia no debe poseer contratos de transporte ni duplicar mapeos.")
            ]),
        Question(language, "seguridad-jwt",
            "A request has a valid JWT but lacks the orders.write claim. Which response and explanation are correct?",
            "Una solicitud tiene un JWT válido, pero carece del claim orders.write. ¿Qué respuesta y explicación son correctas?",
            [
                Option(language, "403 Forbidden because identity is valid but the required policy is not satisfied", "403 Forbidden porque la identidad es válida, pero no satisface la política requerida", true,
                    "Authentication succeeded; authorization denied the missing permission.", "La autenticación fue exitosa; autorización rechazó el permiso faltante."),
                Option(language, "401 Unauthorized because every denied request is an authentication failure", "401 Unauthorized porque toda solicitud rechazada es un fallo de autenticación", false,
                    "401 applies when the caller cannot authenticate with a valid token.", "401 corresponde cuando el solicitante no puede autenticarse con un token válido."),
                Option(language, "201 Created because any signed token grants every API permission", "201 Created porque cualquier token firmado concede todos los permisos de la API", false,
                    "A valid signature establishes trust in claims; policies still decide access.", "Una firma válida establece confianza en los claims; las políticas todavía deciden el acceso.")
            ]),
        Question(language, "pruebas-integracion",
            "What does an HTTP integration test prove that a direct service test does not?",
            "¿Qué demuestra una prueba de integración HTTP que no demuestra una prueba directa del servicio?",
            [
                Option(language, "Routing, serialization, middleware, DI, and response mapping collaborate correctly", "Que rutas, serialización, middleware, DI y mapeo de respuesta colaboran correctamente", true,
                    "The test crosses the same application boundary used by the client.", "La prueba cruza el mismo límite de aplicación que usa el cliente."),
                Option(language, "Every production dependency is available", "Que toda dependencia de producción está disponible", false,
                    "Controlled external adapters may still be replaced in the test host.", "Los adaptadores externos controlados todavía pueden sustituirse en el host de pruebas."),
                Option(language, "Private methods were called in a specific order", "Que métodos privados se llamaron en un orden específico", false,
                    "Integration tests should verify observable collaboration, not private implementation.", "Las pruebas de integración deben comprobar colaboración observable, no implementación privada.")
            ]),
        Question(language, "limites-arquitectura",
            "What makes an architecture boundary useful instead of ceremonial?",
            "¿Qué hace útil un límite de arquitectura en lugar de ceremonial?",
            [
                Option(language, "It protects stable policy from a volatile or external detail", "Protege una política estable de un detalle volátil o externo", true,
                    "The boundary creates clear ownership and dependency direction.", "El límite crea propiedad y dirección de dependencias claras."),
                Option(language, "It creates the maximum number of projects", "Crea la cantidad máxima de proyectos", false,
                    "Project count does not guarantee useful separation.", "La cantidad de proyectos no garantiza separación útil."),
                Option(language, "It places every class in a folder named Layer", "Coloca cada clase en una carpeta llamada Layer", false,
                    "Folders alone do not enforce dependency direction.", "Las carpetas por sí solas no protegen la dirección de dependencias.")
            ]),
        Question(language, "ia-integracion",
            "How should a Middle-level application respond when an LLM times out or returns invalid structured output?",
            "¿Cómo debe responder una aplicación Middle cuando un LLM excede el tiempo o devuelve salida estructurada inválida?",
            [
                Option(language, "Return a deterministic safe fallback and preserve domain rules", "Devolver un fallback seguro y determinista conservando las reglas de dominio", true,
                    "AI remains optional assistance and cannot block or override deterministic policy.", "La IA permanece como asistencia opcional y no puede bloquear ni reemplazar política determinista."),
                Option(language, "Apply the partial response directly to the order", "Aplicar la respuesta parcial directamente a la orden", false,
                    "Invalid external output must never mutate domain state.", "Una salida externa inválida nunca debe modificar estado del dominio."),
                Option(language, "Expose the provider exception and API key to the browser", "Exponer la excepción del proveedor y la API key al navegador", false,
                    "Credentials and internal diagnostics remain protected on the server.", "Credenciales y diagnóstico interno permanecen protegidos en el servidor.")
            ]),
        Question(language, "inyeccion-dependencias",
            "A refactoring extracts pricing, persistence, and notification services. What evidence shows the new DI design is better rather than merely larger?",
            "Una refactorización extrae servicios de precios, persistencia y notificación. ¿Qué evidencia demuestra que el nuevo diseño DI es mejor y no solo más grande?",
            [
                Option(language, "Each dependency owns a coherent decision, lifetimes are justified, and behavior remains green", "Cada dependencia posee una decisión coherente, los ciclos de vida están justificados y la conducta permanece verde", true,
                    "The boundaries correspond to change pressure and retain observable behavior.", "Los límites corresponden a presión de cambio y conservan la conducta observable."),
                Option(language, "The solution has more interfaces than before", "La solución tiene más interfaces que antes", false,
                    "Interface count is not evidence of cohesion or maintainability.", "La cantidad de interfaces no demuestra cohesión ni mantenibilidad."),
                Option(language, "Every service is registered as singleton", "Cada servicio está registrado como singleton", false,
                    "Lifetimes must follow ownership and thread-safety, not uniformity.", "Los ciclos deben seguir propiedad y seguridad de hilos, no uniformidad.")
            ]),
        Question(language, "pruebas-integracion",
            "If an API later rejects a duplicate order with 409, which test would give the strongest evidence?",
            "Si una API más adelante rechaza una orden duplicada con 409, ¿qué prueba aportaría la evidencia más fuerte?",
            [
                Option(language, "An HTTP integration test asserting ProblemDetails and the unchanged database state", "Una prueba HTTP de integración que compruebe ProblemDetails y el estado sin cambios en la base", true,
                    "It verifies transport mapping, application behavior, and persistence together.", "Comprueba en conjunto mapeo de transporte, conducta de aplicación y persistencia."),
                Option(language, "A unit test that checks a private mapper method", "Una prueba unitaria que revise un método privado de mapeo", false,
                    "It cannot prove the endpoint or transaction behavior.", "No demuestra la conducta del endpoint ni de la transacción."),
                Option(language, "A snapshot containing only the response status", "Un snapshot que contenga solo el estado de respuesta", false,
                    "The contract shape and durable state would remain unverified.", "La forma del contrato y el estado durable quedarían sin comprobar.")
            ]),
        Question(language, "ia-integracion",
            "The GitHub Pages demo uses deterministic AI while the local server can use a configured provider. What must remain identical?",
            "La demo de GitHub Pages usa IA determinista mientras el servidor local puede usar un proveedor configurado. ¿Qué debe permanecer idéntico?",
            [
                Option(language, "The application contract, validation, fallback, and review requirements", "El contrato de aplicación, validación, fallback y requisitos de revisión", true,
                    "Configuration changes the adapter, not the policy observed by the UI and use case.", "La configuración cambia el adaptador, no la política observada por UI y caso de uso."),
                Option(language, "The provider SDK types used by every layer", "Los tipos del SDK usados por cada capa", false,
                    "SDK types should remain inside the infrastructure adapter.", "Los tipos del SDK deben permanecer dentro del adaptador de infraestructura."),
                Option(language, "The production API key embedded in the browser build", "La API key productiva incluida en el build del navegador", false,
                    "Credentials must never be shipped to the browser.", "Las credenciales nunca deben enviarse al navegador.")
            ]),
        Question(language, "seguridad-jwt",
            "Before an API trusts claims from a bearer token, what must JWT validation establish?",
            "Antes de que una API confíe en los claims de un token bearer, ¿qué debe establecer la validación JWT?",
            [
                Option(language, "Trusted signature, expected issuer and audience, and a valid lifetime", "Firma confiable, issuer y audience esperados, y vigencia válida", true,
                    "Only a fully validated access token can create a trusted ClaimsPrincipal.", "Solo un access token completamente validado puede crear un ClaimsPrincipal confiable."),
                Option(language, "That the payload is readable after Base64 decoding", "Que el payload sea legible después de decodificar Base64", false,
                    "JWT payloads are readable; decoding does not prove integrity or origin.", "Los payloads JWT son legibles; decodificar no demuestra integridad ni origen."),
                Option(language, "That the browser stored the token in localStorage", "Que el navegador haya guardado el token en localStorage", false,
                    "Storage location does not validate signature, issuer, audience, or expiration.", "La ubicación de almacenamiento no valida firma, issuer, audience ni expiración.")
            ])
    ];

    private static InterviewQuestion Question(
        AppLanguage language,
        string slug,
        string promptEnglish,
        string promptSpanish,
        InterviewOption[] options)
    {
        var plan = MiddleCurriculum.Get(slug);
        var metadata = MiddleModuleMetadataCatalog.Get(slug, language);
        var defense = plan.InterviewQuestions[0].Get(language);

        return new(
            slug,
            metadata.ShortTitle,
            T(language, promptEnglish, promptSpanish),
            options,
            defense,
            plan.Decision.Get(language));
    }

    private static InterviewOption Option(
        AppLanguage language,
        string textEnglish,
        string textSpanish,
        bool isCorrect,
        string feedbackEnglish,
        string feedbackSpanish) =>
        new(T(language, textEnglish, textSpanish), isCorrect, T(language, feedbackEnglish, feedbackSpanish));

    private static string T(AppLanguage language, string english, string spanish) =>
        language == AppLanguage.Spanish ? spanish : english;
}
