namespace DendroDocs.Uml.Fragments.Extensions;

/// <summary>
/// Provides extension methods for querying and navigating through collections of <see cref="InteractionFragment"/>
/// objects.
/// </summary>
/// <remarks>This static class includes methods for retrieving descendants, ancestors, and sibling fragments
/// within a hierarchy of <see cref="InteractionFragment"/> objects. These methods are designed to facilitate traversal
/// and filtering of interaction fragments in a structured manner.</remarks>
public static class InteractionFragmentExtensions
{
    /// <summary>
    /// Query all descendants from this level down.
    /// </summary>
    /// <typeparam name="TFragment">The type of fragment to filter on.</typeparam>
    /// <returns>Returns a readonly list of child fragments.</returns>
    public static IReadOnlyList<TFragment> Descendants<TFragment>(this IEnumerable<InteractionFragment> nodes)
        where TFragment : InteractionFragment
    {
        ArgumentNullException.ThrowIfNull(nodes);

        var result = new List<TFragment>();

        foreach (var node in nodes)
        {
            switch (node)
            {
                case TFragment t:
                    result.Add(t);
                    break;

                case Alt a:
                    result.AddRange(a.Sections.SelectMany(s => s.Fragments).Descendants<TFragment>());
                    break;

                default:
                    break;
            }
        }

        return result;
    }

    /// <summary>
    /// Query all parent fragments from this fragment up.
    /// </summary>
    /// <returns>Returns a list of parent fragments.</returns>
    public static IReadOnlyList<InteractionFragment> Ancestors(this InteractionFragment fragment)
    {
        ArgumentNullException.ThrowIfNull(fragment);

        var result = new List<InteractionFragment>();

        var parent = fragment.Parent;
        while (parent is not null)
        {
            result.Add(parent);

            parent = parent.Parent;
        }

        return result;
    }

    /// <summary>
    /// Query all sibling before the current fragment.
    /// </summary>
    /// <returns>Returns a readonly list of siblings comming before this fragment.</returns>
    public static IReadOnlyList<InteractionFragment> StatementsBeforeSelf(this InteractionFragment fragment)
    {
        ArgumentNullException.ThrowIfNull(fragment);

        if (fragment.Parent != null)
        {
            return [.. fragment.Parent.Fragments.TakeWhile(s => s != fragment)];
        }

        return [];
    }
}
