namespace DendroDocs.Extensions;

public static class IEnumerableStringExtensions
{
    public static IReadOnlyList<string> StartsWith(this IEnumerable<string> list, string partialName)
    {
        ArgumentNullException.ThrowIfNull(list);
        ArgumentNullException.ThrowIfNull(partialName);

        return [.. list.Where(bt => bt.StartsWith(partialName, StringComparison.Ordinal))];
    }
}
