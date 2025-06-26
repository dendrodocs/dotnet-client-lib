namespace DendroDocs;

public static class IEnumerableMethodDescriptionExtensions
{
    public static IReadOnlyList<MethodDescription> WithName(this IEnumerable<MethodDescription> list, string name)
    {
        ArgumentNullException.ThrowIfNull(list);

        return [.. list.Where(m => string.Equals(m.Name, name, StringComparison.Ordinal))];
    }
}
