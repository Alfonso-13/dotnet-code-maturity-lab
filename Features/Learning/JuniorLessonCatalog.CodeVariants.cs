using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    private static ModuleLesson WithCodeVariants(ModuleLesson lesson, AppLanguage language) => lesson.Slug switch
    {
        "poo" => WithClassicVariant(lesson, 4, language, OopClassic(language)),
        "modelos-y-memoria" => WithClassicVariant(lesson, 4, language, ModelsClassic(language)),
        "task-async-await" => WithClassicVariant(lesson, 4, language, AsyncClassic(language)),
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
                    Text(language, "Order API", "API de órdenes"),
                    step.Left,
                    step.Right)
            ],
            DefaultCodeVariantIndex = 1
        };
        return lesson with { Steps = steps };
    }

    private static LessonCodeVariant OopClassic(AppLanguage language) => new(
        Text(language, "Classic example", "Ejemplo clásico"),
        new(
            "Shape.Before.cs",
            Text(language, "Type checks and exposed state", "Tipos comprobados y estado expuesto"),
            LocalizedCode(language,
                "A data bag forces the caller to know every concrete shape.",
                "Una bolsa de datos obliga al consumidor a conocer cada figura concreta.",
                """
                public sealed class ShapeData
                {
                    public string Type { get; set; } = "";
                    public decimal Width { get; set; }
                    public decimal Height { get; set; }
                    public decimal Radius { get; set; }
                }

                public decimal Area(ShapeData shape) => shape.Type switch
                {
                    "rectangle" => shape.Width * shape.Height,
                    "circle" => (decimal)Math.PI * shape.Radius * shape.Radius,
                    _ => 0m
                };
                """),
            "bad-practice"),
        new(
            "Shape.After.cs",
            Text(language, "Encapsulated polymorphic objects", "Objetos polimórficos encapsulados"),
            LocalizedCode(language,
                "Each object protects its data and fulfills the IShape abstraction.",
                "Cada objeto protege sus datos y cumple la abstracción IShape.",
                """
                public interface IShape
                {
                    decimal Area { get; }
                }

                public sealed class Rectangle(decimal width, decimal height) : IShape
                {
                    public decimal Area { get; } = width * height;
                }

                public sealed class Circle(decimal radius) : IShape
                {
                    public decimal Area { get; } =
                        (decimal)Math.PI * radius * radius;
                }

                decimal total = shapes.Sum(shape => shape.Area);
                """),
            "good-practice"));

    private static LessonCodeVariant ModelsClassic(AppLanguage language) => new(
        Text(language, "Classic example", "Ejemplo clásico"),
        new(
            "Types.Before.cs",
            Text(language, "One ambiguous model for every meaning", "Un modelo ambiguo para todo significado"),
            LocalizedCode(language,
                "Strings and object hide identity, value semantics, and valid states.",
                "String y object ocultan identidad, semántica de valor y estados válidos.",
                """
                public sealed class Data
                {
                    public object Id { get; set; } = new();
                    public string Kind { get; set; } = "";
                    public string State { get; set; } = "";
                    public object Value { get; set; } = new();
                }
                """),
            "bad-practice"),
        new(
            "Types.After.cs",
            Text(language, "A type for each semantic contract", "Un tipo por cada contrato semántico"),
            LocalizedCode(language,
                "Person has identity; Point is a value; enum closes states; interface names a capability.",
                "Person posee identidad; Point es un valor; enum cierra estados; interface nombra una capacidad.",
                """
                public sealed class Person
                {
                    public Guid Id { get; init; }
                    public string Name { get; init; } = "";
                }

                public readonly record struct Point(decimal X, decimal Y);
                public enum TrafficLight { Red, Yellow, Green }

                public interface IPrintable
                {
                    string Print();
                }
                """),
            "good-practice"));

    private static LessonCodeVariant AsyncClassic(AppLanguage language) => new(
        Text(language, "Classic example", "Ejemplo clásico"),
        new(
            "Breakfast.Before.cs",
            Text(language, "Blocking each breakfast operation", "Bloqueo de cada operación del desayuno"),
            LocalizedCode(language,
                "Result blocks while independent I/O-like work could remain in flight.",
                "Result bloquea mientras trabajo independiente similar a I/O podría continuar.",
                """
                Coffee coffee = BrewCoffeeAsync().Result;
                Toast toast = ToastBreadAsync().Result;
                Juice juice = PourJuiceAsync().Result;

                Serve(coffee, toast, juice);
                """),
            "bad-practice"),
        new(
            "Breakfast.After.cs",
            Text(language, "Await independent work together", "Espera trabajo independiente en conjunto"),
            LocalizedCode(language,
                "Start independent operations, then observe completion without blocking a thread.",
                "Inicia operaciones independientes y observa su finalización sin bloquear un hilo.",
                """
                Task<Coffee> coffeeTask = BrewCoffeeAsync();
                Task<Toast> toastTask = ToastBreadAsync();
                Task<Juice> juiceTask = PourJuiceAsync();

                await Task.WhenAll(coffeeTask, toastTask, juiceTask);
                Serve(await coffeeTask, await toastTask, await juiceTask);
                """),
            "good-practice"));

    private static string LocalizedCode(
        AppLanguage language,
        string englishComment,
        string spanishComment,
        string code) =>
        $"// {Text(language, englishComment, spanishComment)}\n{code}";

    private static string Text(AppLanguage language, string english, string spanish) =>
        language == AppLanguage.Spanish ? spanish : english;
}
