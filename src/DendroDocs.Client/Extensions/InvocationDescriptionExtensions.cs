namespace DendroDocs.Extensions;

/// <summary>
/// Provides extension methods for working with method invocation descriptions and matching them against method definitions.
/// </summary>
public static class InvocationDescriptionExtensions
{
    /// <summary>
    /// Determines whether the specified invocation matches the given method definition.
    /// </summary>
    /// <param name="invocation">The method invocation to check.</param>
    /// <param name="method">The method definition to match against.</param>
    /// <returns><c>true</c> if the invocation matches the method name and parameters; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="invocation"/> or <paramref name="method"/> is <c>null</c>.</exception>
    public static bool MatchesMethod(this InvocationDescription invocation, IHaveAMethodBody method)
    {
        ArgumentNullException.ThrowIfNull(invocation);
        ArgumentNullException.ThrowIfNull(method);

        return string.Equals(invocation.Name, method.Name) && invocation.MatchesParameters(method);
    }

    /// <summary>
    /// Determines whether the parameters of the specified invocation match the given method definition.
    /// </summary>
    /// <param name="invocation">The method invocation to check.</param>
    /// <param name="method">The method definition to match against.</param>
    /// <returns><c>true</c> if the invocation parameters match the method parameters, considering optional parameters; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="invocation"/> or <paramref name="method"/> is <c>null</c>.</exception>
    public static bool MatchesParameters(this InvocationDescription invocation, IHaveAMethodBody method)
    {
        ArgumentNullException.ThrowIfNull(invocation);
        ArgumentNullException.ThrowIfNull(method);

        if (invocation.Arguments.Count == 0)
        {
            return method.Parameters.Count == 0;
        }

        var invokedWithTypes = invocation.Arguments.Select(a => a.Type).ToList();
        if (invokedWithTypes.Count > method.Parameters.Count)
        {
            return false;
        }

        var optionalArguments = method.Parameters.Count(p => p.HasDefaultValue);
        if (optionalArguments == 0)
        {
            return method.Parameters.Select(p => p.Type).SequenceEqual(invokedWithTypes);
        }

        return method.Parameters.Take(invokedWithTypes.Count).Select(p => p.Type).SequenceEqual(invokedWithTypes);
    }
}
