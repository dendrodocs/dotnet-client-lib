namespace DendroDocs.Extensions;

/// <summary>
/// Provides extension methods for string operations related to type analysis and formatting.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Determines whether the specified type name represents an enumerable collection type.
    /// </summary>
    /// <param name="type">The full type name to check.</param>
    /// <returns><c>true</c> if the type is an enumerable collection; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is <c>null</c>.</exception>
    public static bool IsEnumerable(this string type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (!type.StartsWith("System.Collections.", StringComparison.Ordinal))
        {
            return false;
        }
        else if (type.StartsWith("System.Collections.Generic.", StringComparison.Ordinal))
        {
            return !type.Contains("Enumerator") && !type.Contains("Compar") && !type.Contains("Exception");
        }
        else if (type.StartsWith("System.Collections.Concurrent.", StringComparison.Ordinal))
        {
            return !type.Contains("Partition");
        }

        return !type.Contains("Enumerator") && !type.Contains("Compar") && !type.Contains("Structural") && !type.Contains("Provider");
    }

    /// <summary>
    /// Determines whether the specified type name represents a generic type.
    /// </summary>
    /// <param name="type">The type name to check for generic type indicators.</param>
    /// <returns><c>true</c> if the type is generic (contains angle brackets); otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is <c>null</c>.</exception>
    public static bool IsGeneric(this string type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return type.IndexOf('>') > -1 && type.TrimEnd().EndsWith('>');
    }

    /// <summary>
    /// Extracts the generic type arguments from a generic type name.
    /// </summary>
    /// <param name="type">The generic type name to parse.</param>
    /// <returns>A read-only list containing the generic type arguments. Returns an empty list if the type is not generic.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is <c>null</c>.</exception>
    /// <example>
    /// <code>
    /// var genericTypes = "System.Collections.Generic.List&lt;System.String&gt;".GenericTypes();
    /// // Returns: ["System.String"]
    /// </code>
    /// </example>
    public static IReadOnlyList<string> GenericTypes(this string type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (!type.IsGeneric())
        {
            return [];
        }

        type = type.Trim();

        var typeParts = type.Substring(type.IndexOf('<') + 1, type.Length - type.IndexOf('<') - 2).Split(',');
        var types = new List<string>();

        foreach (var part in typeParts)
        {
            if (part.IndexOf('>') > -1 && types.Count > 0 && types.Last().ToCharArray().Count(c => c == '<') > types.Last().ToCharArray().Count(c => c == '>'))
            {
                types[^1] = types[^1] + "," + part.Trim();
            }
            else
            {
                types.Add(part.Trim());
            }
        }

        return types;
    }

    /// <summary>
    /// Formats a type name for display in diagrams by removing namespace qualifiers and preserving generic type structure.
    /// </summary>
    /// <param name="type">The full type name to format.</param>
    /// <returns>A simplified type name suitable for diagram display.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is <c>null</c>.</exception>
    /// <example>
    /// <code>
    /// var diagramName = "System.Collections.Generic.List&lt;System.String&gt;".ForDiagram();
    /// // Returns: "List&lt;String&gt;"
    /// </code>
    /// </example>
    public static string ForDiagram(this string type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (type.IsGeneric())
        {
            var a = type[..type.IndexOf('<')].ForDiagram();
            var b = type.GenericTypes().Select(s => s.ForDiagram());
            return $"{a}<{string.Join(", ", b)}>";
        }
        else if (type.IndexOf('.') > -1)
        {
            return type[(type.LastIndexOf('.') + 1)..];
        }
        else
        {
            return type;
        }
    }

    /// <summary>
    /// Converts a string to sentence case by adding spaces before uppercase letters and digits.
    /// </summary>
    /// <param name="type">The string to convert to sentence case.</param>
    /// <returns>The string converted to sentence case with appropriate spacing.</returns>
    /// <example>
    /// <code>
    /// var sentence = "MyPropertyName".ToSentenceCase();
    /// // Returns: "My Property Name"
    /// </code>
    /// </example>
    public static string ToSentenceCase(this string type)
    {
        if (string.IsNullOrEmpty(type))
        {
            return type;
        }

        var stringBuilder = new StringBuilder();

        stringBuilder.Append(char.ToUpper(type[0]));

        for (var i = 1; i < type.Length; i++)
        {
            if ((char.IsUpper(type[i]) && (!char.IsUpper(type[i - 1]) || ((i + 1 < type.Length) && !char.IsUpper(type[i + 1])))) || (char.IsDigit(type[i]) && !char.IsDigit(type[i - 1])))
            {
                stringBuilder.Append(' ');
            }

            stringBuilder.Append(type[i]);
        }

        return stringBuilder.ToString();
    }
}
