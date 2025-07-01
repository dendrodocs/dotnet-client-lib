namespace DendroDocs.Extensions;

/// <summary>
/// Provides extension methods for working with collections of method descriptions.
/// </summary>
public static class IEnumerableMethodDescriptionExtensions
{
    /// <summary>
    /// Filters the collection to return only method descriptions with the specified name.
    /// </summary>
    /// <param name="list">The collection of method descriptions to filter.</param>
    /// <param name="name">The method name to match.</param>
    /// <returns>A read-only list containing only the method descriptions with the specified name.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is <c>null</c>.</exception>
    public static IReadOnlyList<MethodDescription> WithName(this IEnumerable<MethodDescription> list, string name)
    {
        ArgumentNullException.ThrowIfNull(list);

        return [.. list.Where(m => string.Equals(m.Name, name, StringComparison.Ordinal))];
    }
}
