namespace DendroDocs.Extensions;

/// <summary>
/// Provides extension methods for working with collections of attribute descriptions.
/// </summary>
public static class IEnumerableIAttributeDescriptionExtensions
{
    /// <summary>
    /// Filters the collection to return only attribute descriptions of the specified type.
    /// </summary>
    /// <param name="list">The collection of attribute descriptions to filter.</param>
    /// <param name="fullname">The full type name of the attribute to match.</param>
    /// <returns>A read-only list containing only the attribute descriptions of the specified type.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is <c>null</c>.</exception>
    public static IReadOnlyList<AttributeDescription> OfType(this IEnumerable<AttributeDescription> list, string fullname)
    {
        ArgumentNullException.ThrowIfNull(list);

        return [.. list.Where(ad => string.Equals(ad.Type, fullname, StringComparison.Ordinal))];
    }

    /// <summary>
    /// Determines whether the collection contains an attribute of the specified type.
    /// </summary>
    /// <param name="list">The collection of attribute descriptions to check.</param>
    /// <param name="fullname">The full type name of the attribute to search for.</param>
    /// <returns><c>true</c> if an attribute of the specified type is found; otherwise, <c>false</c>.</returns>
    public static bool HasAttribute(this IEnumerable<AttributeDescription> list, string fullname)
    {
        return list.OfType(fullname).Any();
    }
}
