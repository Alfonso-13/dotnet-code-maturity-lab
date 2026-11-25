using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class MiddleLessonCatalog
{
    private static ModuleLesson WithAdditionalCodeVariants(ModuleLesson lesson, AppLanguage language) => lesson.Slug switch
    {
        "inyeccion-dependencias" => WithClassicVariant(lesson, 1, language, DependencyInjectionClassic(language)),
        "strategy-patrones" => WithClassicVariant(lesson, 1, language, StrategyClassic(language)),
        _ => lesson
    };

    private static ModuleLesson WithClassicVariant(
        ModuleLesson lesson,
        int perspectiveIndex,
        AppLanguage language,
        LessonCodeVariant classic)
    {
        var steps = lesson.Steps.ToArray();
        var step = steps[perspectiveIndex];
        steps[perspectiveIndex] = step with
        {
            CodeVariants =
            [
                classic,
                new(
                    T(language, "Order API", "API de órdenes"),
                    step.Left,
                    step.Right)
            ],
            DefaultCodeVariantIndex = 1
        };
        return lesson with { Steps = steps };
    }

    private static LessonCodeVariant DependencyInjectionClassic(AppLanguage language) => new(
        T(language, "Classic example", "Ejemplo clásico"),
        Bad(
            "LightSwitch.Before.cs",
            T(language, "Hidden concrete dependency", "Dependencia concreta oculta"),
            ClassicCode(language,
                "LightSwitch creates the device it controls and cannot be tested or reused with a Fan.",
                "LightSwitch crea el dispositivo que controla y no puede probarse ni reutilizarse con un Fan.",
                """
                public sealed class LightSwitch
                {
                    private readonly LightBulb bulb = new();

                    public void Toggle() => bulb.TurnOn();
                }
                """)),
        Good(
            "LightSwitch.After.cs",
            T(language, "Explicit constructor dependency", "Dependencia explícita por constructor"),
            ClassicCode(language,
                "Composition chooses the device; LightSwitch declares only the capability it needs.",
                "La composición elige el dispositivo; LightSwitch declara solo la capacidad necesaria.",
                """
                public interface ISwitchable
                {
                    void TurnOn();
                }

                public sealed class LightSwitch(ISwitchable device)
                {
                    public void Toggle() => device.TurnOn();
                }

                var control = new LightSwitch(new LightBulb());
                """)));

    private static LessonCodeVariant StrategyClassic(AppLanguage language) => new(
        T(language, "Classic example", "Ejemplo clásico"),
        Bad(
            "ShippingCalculator.Before.cs",
            T(language, "Growing shipping conditional", "Condicional creciente de envíos"),
            ClassicCode(language,
                "Every carrier adds another branch and another reason to edit the calculator.",
                "Cada transportista agrega otra rama y otra razón para modificar el calculador.",
                """
                public decimal Calculate(string carrier, Package package)
                {
                    if (carrier == "standard") return package.Weight * 1.2m;
                    if (carrier == "express") return package.Weight * 2.4m;
                    throw new NotSupportedException(carrier);
                }
                """)),
        Good(
            "ShippingStrategy.After.cs",
            T(language, "Replaceable shipping behavior", "Conducta de envío reemplazable"),
            ClassicCode(language,
                "The calculator executes a selected strategy without knowing carrier details.",
                "El calculador ejecuta una estrategia seleccionada sin conocer detalles del transportista.",
                """
                public interface IShippingStrategy
                {
                    decimal Calculate(Package package);
                }

                public sealed class ExpressShipping : IShippingStrategy
                {
                    public decimal Calculate(Package package) =>
                        package.Weight * 2.4m;
                }

                decimal price = strategy.Calculate(package);
                """)));
}
