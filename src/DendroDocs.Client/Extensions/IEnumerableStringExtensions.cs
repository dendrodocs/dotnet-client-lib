namespace DendroDocs.Extensions;

/// <summary>
/// Provides extension methods for working with collections of string values.
/// </summary>
public static class IEnumerableStringExtensions
{
    /// <summary>
    /// Filters the collection to return only strings that start with the specified prefix.
    /// </summary>
    /// <param name="list">The collection of strings to filter.</param>
    /// <param name="partialName">The prefix to match against.</param>
    /// <returns>A read-only list containing only the strings that start with the specified prefix.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> or <paramref name="partialName"/> is <c>null</c>.</exception>
    public static IReadOnlyList<string> StartsWith(this IEnumerable<string> list, string partialName)
    {
        ArgumentNullException.ThrowIfNull(list);
        ArgumentNullException.ThrowIfNull(partialName);

        return [.. list.Where(bt => bt.StartsWith(partialName, StringComparison.Ordinal))];
    }
}
