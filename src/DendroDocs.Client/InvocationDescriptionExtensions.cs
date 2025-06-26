namespace DendroDocs;

public static class InvocationDescriptionExtensions
{
    public static bool MatchesMethod(this InvocationDescription invocation, IHaveAMethodBody method)
    {
        ArgumentNullException.ThrowIfNull(invocation);
        ArgumentNullException.ThrowIfNull(method);

        return string.Equals(invocation.Name, method.Name) && invocation.MatchesParameters(method);
    }

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
