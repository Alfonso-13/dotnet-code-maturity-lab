namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    private static ModuleLesson TestingAaaSpanish() => Module("pruebas-aaa",
        Step("Entiende", "AAA", "Preparar, ejecutar y verificar una conducta",
            Pane("CreateOrderServiceTests.cs", "Arrange / Act / Assert", """
                [Fact]
                public async Task Create_WithZeroQuantity_ReturnsError()
                {
                    // Arrange
                    var request = new CreateOrderRequest("MONITOR", 0, 50m);

                    // Act
                    CreateOrderResult result = await service.CreateAsync(request);

                    // Assert
                    Assert.Contains("Quantity", result.Errors);
                }
                """), null,
            "AAA organiza el escenario", "Arrange prepara, Act ejecuta una sola conducta y Assert verifica el resultado observable.", "Arrange → Act → Assert"),
        Step("Compara", "Enfoque", "Probar comportamiento frente a detalles internos",
            Bad("OrderTests.Before.cs", "Detalle privado", """
                [Fact]
                public void Confirm_CallsPrivateMethod()
                {
                    // La prueba depende de cómo está implementada la clase.
                    Assert.True(CallPrivateValidation(order));
                }
                """),
            Good("OrderTests.After.cs", "Resultado observable", """
                [Fact]
                public void Confirm_DraftOrder_ChangesStatus()
                {
                    var order = new Order("MONITOR");

                    order.Confirm();

                    Assert.Equal(OrderStatus.Confirmed, order.Status);
                }
                """),
            "La prueba debe sobrevivir una refactorización", "Comprueba lo que el consumidor puede observar, no los métodos privados usados para conseguirlo.", "entrada → conducta → resultado"),
        Step("Casos", "Límites", "Cubrir válido, inválido y frontera",
            Pane("ValidCases.cs", "Casos válidos", """
                [Theory]
                [InlineData(1)]
                [InlineData(10)]
                public async Task Create_WithPositiveQuantity_Succeeds(int quantity)
                {
                    CreateOrderResult result = await service.CreateAsync(Request(quantity));
                    Assert.True(result.IsSuccess);
                }
                """),
            Pane("InvalidCases.cs", "Casos inválidos", """
                [Theory]
                [InlineData(0)]
                [InlineData(-1)]
                public async Task Create_WithNonPositiveQuantity_Fails(int quantity)
                {
                    CreateOrderResult result = await service.CreateAsync(Request(quantity));
                    Assert.False(result.IsSuccess);
                }
                """),
            "Los límites revelan errores frecuentes", "No pruebes solo el camino feliz: incluye cero, negativos y valores que cambian una regla.", "válido | inválido | frontera"),
        Step("Integración", "API real", "Comprobar que los componentes colaboran",
            Pane("ServiceTest.cs", "Unitaria", """
                [Fact]
                public async Task Create_InvalidQuantity_ReturnsError()
                {
                    var service = new CreateOrderService(fakeStore);
                    CreateOrderResult result = await service.CreateAsync(Request(0));
                    Assert.False(result.IsSuccess);
                }
                """),
            Pane("ApiTest.cs", "Integración", """
                [Fact]
                public async Task PostOrder_ValidRequest_ReturnsCreated()
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        "/api/orders",
                        Request(2));

                    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                }
                """),
            "El tipo de prueba depende del riesgo", "La unitaria enfoca una regla; la integración comprueba endpoint, serialización y colaboración real.", "unidad | integración"),
        Step("Integra", "Prueba útil", "Un nombre claro y una sola razón para fallar",
            Bad("Tests.Before.cs", "Código deficiente", """
                [Fact]
                public async Task Test1()
                {
                    Assert.NotNull(await service.CreateAsync(Request(2)));
                    Assert.NotNull(await service.CreateAsync(Request(0)));
                }
                """),
            Good("Tests.After.cs", "Código correcto", """
                [Fact]
                public async Task Create_WithZeroQuantity_ReturnsQuantityError()
                {
                    // Arrange
                    CreateOrderRequest request = Request(0);

                    // Act
                    CreateOrderResult result = await service.CreateAsync(request);

                    // Assert
                    Assert.Contains("Quantity", result.Errors);
                }
                """),
            "La prueba explica la conducta", "Nombre, preparación, acción y aserción deben permitir entender el fallo sin depurar primero.", "escenario → acción → expectativa"));

    private static ModuleLesson TestingAaaEnglish() => Module("pruebas-aaa",
        Step("Understand", "AAA", "Arrange, act, and assert one observable behavior",
            Pane("OrderTests.cs", "Arrange / Act / Assert", """
                [Fact]
                public void Confirm_DraftOrder_ChangesStatus()
                {
                    // Arrange
                    var order = new Order("MONITOR");

                    // Act
                    order.Confirm();

                    // Assert
                    Assert.Equal(OrderStatus.Confirmed, order.Status);
                }
                """), null,
            "A test tells a behavioral story", "AAA separates setup, the action under test, and the observable expectation.", "arrange → act → assert"),
        Step("Compare", "Coupling", "Test behavior instead of implementation details",
            Bad("OrderTests.Before.cs", "Brittle test", """
                [Fact]
                public void Confirm_CallsPrivateMethod()
                {
                    Assert.True(InvokePrivate(order, "CanConfirm"));
                }
                """),
            Good("OrderTests.After.cs", "Observable result", """
                [Fact]
                public void Confirm_DraftOrder_ChangesStatus()
                {
                    var order = new Order("MONITOR");
                    order.Confirm();
                    Assert.Equal(OrderStatus.Confirmed, order.Status);
                }
                """),
            "Tests should survive refactoring", "Assert what a consumer can observe, not which private method produced it.", "input → behavior → result"),
        Step("Cases", "Boundaries", "Cover valid, invalid, and boundary values",
            Pane("ValidCases.cs", "Valid cases", """
                [Theory]
                [InlineData(1)]
                [InlineData(10)]
                public async Task Create_WithPositiveQuantity_Succeeds(int quantity)
                {
                    CreateOrderResult result = await service.CreateAsync(Request(quantity));
                    Assert.True(result.IsSuccess);
                }
                """),
            Pane("InvalidCases.cs", "Invalid cases", """
                [Theory]
                [InlineData(0)]
                [InlineData(-1)]
                public async Task Create_WithNonPositiveQuantity_Fails(int quantity)
                {
                    CreateOrderResult result = await service.CreateAsync(Request(quantity));
                    Assert.False(result.IsSuccess);
                }
                """),
            "Boundaries expose common defects", "Do not test only the happy path. Include zero, negatives, and values where rules change.", "valid | invalid | boundary"),
        Step("Integration", "Real API", "Verify that components collaborate",
            Pane("ServiceTest.cs", "Unit", """
                [Fact]
                public async Task Create_InvalidQuantity_ReturnsError()
                {
                    var service = new CreateOrderService(fakeStore);
                    CreateOrderResult result = await service.CreateAsync(Request(0));
                    Assert.False(result.IsSuccess);
                }
                """),
            Pane("ApiTest.cs", "Integration", """
                [Fact]
                public async Task PostOrder_ValidRequest_ReturnsCreated()
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        "/api/orders",
                        Request(2));

                    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                }
                """),
            "Choose the test type by risk", "A unit test focuses on one rule; an integration test verifies the endpoint, serialization, and real collaboration.", "unit | integration"),
        Step("Integrate", "Useful test", "Give the test one clear reason to fail",
            Bad("Tests.Before.cs", "Deficient code", """
                [Fact]
                public async Task Test1()
                {
                    Assert.NotNull(await service.CreateAsync(Request(2)));
                    Assert.NotNull(await service.CreateAsync(Request(0)));
                }
                """),
            Good("Tests.After.cs", "Correct code", """
                [Fact]
                public async Task Create_WithZeroQuantity_ReturnsQuantityError()
                {
                    // Arrange
                    CreateOrderRequest request = Request(0);

                    // Act
                    CreateOrderResult result = await service.CreateAsync(request);

                    // Assert
                    Assert.Contains("Quantity", result.Errors);
                }
                """),
            "The test explains the behavior", "Its name, setup, action, and assertion should explain a failure before debugging.", "scenario → action → expectation"));
}
