using DotnetCodeMaturityLab.Components.Orders;

namespace DotnetCodeMaturityLab.Features.Learning;

public sealed record LaboratoryDefinition(string Level, Type ComponentType);

/// <summary>
/// Maps an available level to its capstone implementation. The catalog page
/// and navigation remain shared while each level owns its executable case.
/// </summary>
public static class LaboratoryContentCatalog
{
    private static readonly IReadOnlyDictionary<string, LaboratoryDefinition> Laboratories =
        new Dictionary<string, LaboratoryDefinition>(StringComparer.OrdinalIgnoreCase)
        {
            ["junior"] = new("junior", typeof(JuniorLaboratory))
        };

    public static bool TryGet(string? level, out LaboratoryDefinition laboratory) =>
        Laboratories.TryGetValue(level ?? string.Empty, out laboratory!);
}
