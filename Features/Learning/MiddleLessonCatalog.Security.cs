using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class MiddleLessonCatalog
{
    private static ModuleLesson JwtAuthorization(AppLanguage language) => Module("seguridad-jwt",
        Step(language, "Understand", "Entiende", "Validate the token before trusting its claims", "Valida el token antes de confiar en sus claims",
            Pane("Program.Auth.cs", T(language, "Bearer validation", "Validación bearer"), """
                builder.Services
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        // Authority metadata supplies the issuer and signing keys.
                        options.Authority = configuration["Auth:Authority"];
                        options.Audience = configuration["Auth:Audience"];
                        options.MapInboundClaims = false;
                        options.TokenValidationParameters = new()
                        {
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true
                        };
                    });

                app.UseAuthentication();
                app.UseAuthorization();
                """), null,
            "Authentication creates a trusted ClaimsPrincipal", "Autenticación crea un ClaimsPrincipal confiable",
            "The bearer handler validates signature, issuer, audience, and expiration before exposing claims to authorization.", "El handler bearer valida firma, issuer, audience y expiración antes de exponer claims a autorización.",
            "token validation → ClaimsPrincipal", "validación del token → ClaimsPrincipal"),
        Step(language, "Compare", "Compara", "A broad role versus a focused permission policy", "Un rol amplio frente a una política de permiso enfocada",
            Bad("OrderEndpoints.Before.cs", T(language, "Role coupled to one organization", "Rol acoplado a una organización"), """
                app.MapPost("/api/orders", CreateOrder)
                    .RequireAuthorization(policy =>
                        policy.RequireRole("Administrator"));
                """),
            Good("OrderEndpoints.After.cs", T(language, "Capability policy", "Política de capacidad"), """
                builder.Services.AddAuthorizationBuilder()
                    .AddPolicy("orders.write", policy =>
                        policy.RequireClaim("permission", "orders.write"));

                app.MapPost("/api/orders", CreateOrder)
                    .RequireAuthorization("orders.write");
                """),
            "Authorize the capability the endpoint needs", "Autoriza la capacidad que necesita el endpoint",
            "Roles can group people, but a named policy keeps the endpoint coupled to an application permission instead of one organizational title.", "Los roles pueden agrupar personas, pero una política con nombre acopla el endpoint a un permiso de aplicación y no a un cargo organizacional.",
            "identity claims → policy → permission decision", "claims de identidad → política → decisión de permiso"),
        Step(language, "Design", "Diseña", "Keep authorization policy at the HTTP boundary", "Mantén la política de autorización en el límite HTTP",
            Pane("OrderAuthorization.cs", T(language, "Named policy", "Política con nombre"), """
                public static class OrderAuthorization
                {
                    public const string WritePolicy = "orders.write";
                    public const string PermissionClaim = "permission";

                    public static void AddOrderAuthorization(
                        this AuthorizationBuilder authorization) =>
                        authorization.AddPolicy(
                            WritePolicy,
                            policy => policy.RequireClaim(
                                PermissionClaim,
                                WritePolicy));
                }
                """),
            Pane("CreateOrderEndpoint.cs", T(language, "Protected adapter", "Adaptador protegido"), """
                app.MapPost("/api/orders", CreateOrder)
                    .RequireAuthorization(OrderAuthorization.WritePolicy);

                static Task<IResult> CreateOrder(
                    CreateOrderRequest request,
                    CreateOrderUseCase useCase,
                    CancellationToken token) =>
                    useCase.ExecuteAsync(request, token);
                """),
            "HTTP owns access control; the use case owns order rules", "HTTP posee control de acceso; el caso posee reglas de órdenes",
            "The endpoint rejects callers before orchestration while the application service remains reusable outside one authentication mechanism.", "El endpoint rechaza solicitantes antes de orquestar mientras el servicio de aplicación permanece reutilizable fuera de un mecanismo de autenticación.",
            "authorization adapter → application use case", "adaptador de autorización → caso de aplicación"),
        Step(language, "Verify", "Comprueba", "Distinguish authentication and permission failures", "Distingue fallos de autenticación y permisos",
            Pane("OrderAuthorizationTests.cs", T(language, "401 evidence", "Evidencia 401"), """
                [Fact]
                public async Task PostWithoutValidToken_ReturnsUnauthorized()
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        "/api/orders",
                        ValidRequest);

                    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
                    Assert.Contains("Bearer", response.Headers.WwwAuthenticate.ToString());
                }
                """),
            Pane("OrderPermissionTests.cs", T(language, "403 evidence", "Evidencia 403"), """
                [Fact]
                public async Task PostWithoutWriteClaim_ReturnsForbidden()
                {
                    client.DefaultRequestHeaders.Authorization =
                        TestTokens.ValidBearer(permission: "orders.read");

                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        "/api/orders",
                        ValidRequest);

                    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
                }
                """),
            "401 rejects identity; 403 rejects permission", "401 rechaza identidad; 403 rechaza permiso",
            "A missing, expired, or invalid token cannot authenticate. A valid identity without orders.write authenticates but fails authorization.", "Un token ausente, expirado o inválido no autentica. Una identidad válida sin orders.write autentica, pero falla autorización.",
            "invalid identity → 401 | missing permission → 403", "identidad inválida → 401 | permiso faltante → 403"),
        Step(language, "Integrate", "Integra", "Prove the protected success path through HTTP", "Demuestra por HTTP la ruta protegida exitosa",
            Pane("AuthorizedOrderTests.cs", T(language, "Authorized contract", "Contrato autorizado"), """
                [Fact]
                public async Task PostWithWriteClaim_ReturnsCreated()
                {
                    client.DefaultRequestHeaders.Authorization =
                        TestTokens.ValidBearer(permission: "orders.write");

                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        "/api/orders",
                        ValidRequest);

                    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                    Assert.NotNull(response.Headers.Location);
                }
                """),
            Pane("SecurityContract.cs", T(language, "Observable matrix", "Matriz observable"), """
                // Invalid signature, issuer, audience, or expiration → 401.
                // Valid identity without orders.write              → 403.
                // Valid identity with orders.write                 → 201.
                // Tokens and internal validation details never enter the response.
                """),
            "Security behavior is part of the public API contract", "La conducta de seguridad forma parte del contrato público",
            "Integration tests exercise authentication, policy evaluation, endpoint metadata, and the successful order response without logging bearer tokens.", "Las pruebas de integración ejercen autenticación, evaluación de políticas, metadata del endpoint y la respuesta exitosa sin registrar tokens bearer.",
            "validated token + required claim → protected use case", "token validado + claim requerido → caso protegido"));
}
