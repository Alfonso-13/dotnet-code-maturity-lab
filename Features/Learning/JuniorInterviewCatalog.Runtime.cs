using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorInterviewCatalog
{
    private static IEnumerable<InterviewQuestion> AdditionalRuntimeQuestions(AppLanguage language)
    {
        if (language == AppLanguage.Spanish)
        {
            yield return new("modelos-y-memoria", "Modelos y memoria",
                "¿Qué ocurre al asignar un int a object y después recuperarlo como int?",
                [
                    new("Se hace boxing al guardar el valor y unboxing al extraerlo con el tipo correcto", true, "La conversión a object empaqueta una copia; la extracción requiere el tipo de valor compatible."),
                    new("Ambas variables apuntan al mismo int mutable", false, "int es un tipo por valor; el boxing crea una caja independiente."),
                    new("La conversión siempre evita asignaciones", false, "El boxing puede crear una asignación y conviene evitarlo en rutas sensibles al rendimiento.")
                ],
                "¿Cuándo hay boxing y unboxing y por qué importa?",
                "Boxing convierte un valor, como int, en una referencia object o interfaz compatible; unboxing recupera el tipo de valor mediante conversión explícita. Puede asignar memoria y fallar si el tipo no coincide.");
            yield return new("linq", "LINQ",
                "¿Qué diferencia importa al filtrar órdenes en memoria frente a EF Core?",
                [
                    new("IEnumerable ejecuta operadores en memoria; IQueryable permite que EF Core traduzca una expresión al proveedor", true, "El proveedor puede ejecutar el filtro en SQL antes de materializar."),
                    new("IQueryable siempre es más rápido, incluso después de ToList", false, "Después de materializar, la consulta ya trabaja sobre objetos en memoria."),
                    new("IEnumerable e IQueryable siempre generan el mismo SQL", false, "IEnumerable no promete traducción; el origen y el punto de materialización importan.")
                ],
                "¿Cuándo usarías IQueryable y cuándo IEnumerable?",
                "Con EF Core, IQueryable permite componer filtros traducibles antes de ToListAsync. Tras materializar, IEnumerable opera sobre objetos en memoria. Expón consultas acotadas desde la capa de datos; no prometas que cualquier método C# se traduzca a SQL.");
            yield break;
        }

        yield return new("modelos-y-memoria", "Models and memory",
            "What happens when an int is assigned to object and then recovered as int?",
            [
                new("Boxing wraps a copy of the value; unboxing extracts it with the matching value type", true, "Converting to object boxes a copy; extracting it requires a compatible value type."),
                new("Both variables reference the same mutable int", false, "int is a value type; boxing creates a separate box."),
                new("The conversion always avoids allocations", false, "Boxing can allocate and matters in performance-sensitive paths.")
            ],
            "When do boxing and unboxing happen, and why does it matter?",
            "Boxing converts a value such as int to an object or compatible interface reference; unboxing explicitly recovers the value type. It can allocate, and an incompatible unbox fails.");
        yield return new("linq", "LINQ",
            "What matters when filtering orders in memory versus through EF Core?",
            [
                new("IEnumerable runs operators in memory; IQueryable lets EF Core translate an expression to its provider", true, "The provider can run the filter in SQL before materialization."),
                new("IQueryable is always faster, even after ToList", false, "After materialization, the query operates on in-memory objects."),
                new("IEnumerable and IQueryable always generate the same SQL", false, "IEnumerable does not promise translation; the source and materialization point matter.")
            ],
            "When would you use IQueryable versus IEnumerable?",
            "With EF Core, IQueryable composes translatable filters before ToListAsync. After materialization, IEnumerable operates on in-memory objects. Keep data-layer queries bounded; arbitrary C# methods may not translate to SQL.");
    }
}
