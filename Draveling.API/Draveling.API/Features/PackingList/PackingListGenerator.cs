namespace Draveling.API.Features.PackingList;

/// <summary>An item that applies to the trip, with its quantity when one is meaningful.</summary>
public sealed record PackingListEntry(string Key, PackingCategory Category, int? Quantity);

/// <summary>Pure logic that turns trip details into a checklist. No UI or I/O dependencies.</summary>
public static class PackingListGenerator
{
    /// <summary>Items for the trip, grouped by category in catalog order. Days must be at least 1.</summary>
    public static IReadOnlyList<(PackingCategory Category, IReadOnlyList<PackingListEntry> Entries)> Generate(PackingListInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentOutOfRangeException.ThrowIfLessThan(input.Days ?? 0, 1, nameof(input.Days));

        return PackingCatalog.Items
            .Where(item => item.When(input))
            .Select(item => new PackingListEntry(item.Key, item.Category, item.Quantity(input)))
            .GroupBy(entry => entry.Category)
            .OrderBy(group => group.Key)
            .Select(group => (group.Key, (IReadOnlyList<PackingListEntry>)group.ToList()))
            .ToList();
    }
}
