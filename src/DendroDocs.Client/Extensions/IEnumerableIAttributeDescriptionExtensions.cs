namespace DendroDocs.Extensions;

public static class IEnumerableIAttributeDescriptionExtensions
{
    public static IReadOnlyList<AttributeDescription> OfType(this IEnumerable<AttributeDescription> list, string fullname)
    {
        ArgumentNullException.ThrowIfNull(list);

        return [.. list.Where(ad => string.Equals(ad.Type, fullname, StringComparison.Ordinal))];
    }

    public static bool HasAttribute(this IEnumerable<AttributeDescription> list, string fullname)
    {
        return list.OfType(fullname).Any();
    }
}
