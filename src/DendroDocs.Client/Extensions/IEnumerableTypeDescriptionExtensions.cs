namespace DendroDocs.Extensions;

/// <summary>
/// Provides extension methods for working with collections of type descriptions, including type lookup, 
/// inheritance analysis, method invocation tracing, and member population.
/// </summary>
public static class IEnumerableTypeDescriptionExtensions
{
    /// <summary>
    /// Finds the first type description with the specified full name.
    /// </summary>
    /// <param name="types">The collection of type descriptions to search.</param>
    /// <param name="typeName">The full name of the type to find.</param>
    /// <returns>The first type description with the specified name.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="types"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no type with the specified name is found.</exception>
    public static TypeDescription First(this IEnumerable<TypeDescription> types, string typeName)
    {
        ArgumentNullException.ThrowIfNull(types);

        return types.First(t => string.Equals(t.FullName, typeName, StringComparison.Ordinal));
    }

    /// <summary>
    /// Finds the first type description with the specified full name, or returns <c>null</c> if not found.
    /// </summary>
    /// <param name="types">The collection of type descriptions to search.</param>
    /// <param name="typeName">The full name of the type to find.</param>
    /// <returns>The first type description with the specified name, or <c>null</c> if not found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="types"/> is <c>null</c>.</exception>
    public static TypeDescription? FirstOrDefault(this IEnumerable<TypeDescription> types, string typeName)
    {
        ArgumentNullException.ThrowIfNull(types);

        return types.FirstOrDefault(t => string.Equals(t.FullName, typeName, StringComparison.Ordinal));
    }

    /// <summary>
    /// Finds method bodies that match the specified method invocation.
    /// </summary>
    /// <param name="types">The collection of type descriptions to search.</param>
    /// <param name="invocation">The method invocation to match against.</param>
    /// <returns>A read-only list of method bodies that match the invocation. Returns an empty list if no matches are found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="types"/> is <c>null</c>.</exception>
    public static IReadOnlyList<IHaveAMethodBody> GetInvokedMethod(this IEnumerable<TypeDescription> types, InvocationDescription invocation)
    {
        ArgumentNullException.ThrowIfNull(types);

        var type = types.FirstOrDefault(invocation.ContainingType);
        if (type is null)
        {
            return [];
        }

        return [.. type.MethodBodies().Where(invocation.MatchesMethod)];
    }

    /// <summary>
    /// Recursively traces all method invocations that result from executing the specified invocation.
    /// </summary>
    /// <param name="types">The collection of type descriptions to search.</param>
    /// <param name="invocation">The initial method invocation to trace.</param>
    /// <returns>A read-only list of all invocations in the call chain, including the original invocation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="types"/> is <c>null</c>.</exception>
    public static IReadOnlyList<InvocationDescription> GetInvocationConsequences(this IEnumerable<TypeDescription> types, InvocationDescription invocation)
    {
        ArgumentNullException.ThrowIfNull(types);

        var consequences = types.GetInvokedMethod(invocation)
            .SelectMany(m => m.Statements.OfType<InvocationDescription>())
            .SelectMany(im => types.GetInvocationConsequences(im))
            .Prepend(invocation)
            .ToList();

        return consequences;
    }

    /// <summary>
    /// Gets all statements that result from executing the specified method invocation, including nested statements from called methods.
    /// </summary>
    /// <param name="types">The collection of type descriptions to search.</param>
    /// <param name="invocation">The method invocation to analyze.</param>
    /// <returns>A read-only list of all statements that result from the invocation, including the invocation itself.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="types"/> is <c>null</c>.</exception>
    public static IReadOnlyList<Statement> GetInvocationConsequenceStatements(this IEnumerable<TypeDescription> types, InvocationDescription invocation)
    {
        ArgumentNullException.ThrowIfNull(types);

        var consequences = types.GetInvokedMethod(invocation)
            .SelectMany(m => m.Statements)
            .SelectMany(im => TraverseStatement(types, im))
            .Prepend(invocation)
            .ToList();

        return consequences;
    }

    /// <summary>
    /// Recursively traverses and expands complex statements (like switches and conditional statements) to include all nested statements.
    /// </summary>
    /// <param name="types">The collection of type descriptions to use for method resolution.</param>
    /// <param name="sourceStatement">The statement to traverse and expand.</param>
    /// <returns>A read-only list of statements with expanded nested structures. For simple statements, returns an empty list.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="types"/> is <c>null</c>.</exception>
    public static IReadOnlyList<Statement> TraverseStatement(this IEnumerable<TypeDescription> types, Statement sourceStatement)
    {
        ArgumentNullException.ThrowIfNull(types);

        switch (sourceStatement)
        {
            case InvocationDescription invocationDescription:
                return types.GetInvocationConsequenceStatements(invocationDescription);

            case Switch sourceSwitch:
                var destinationSwitch = new Switch();
                foreach (var switchSection in sourceSwitch.Sections)
                {
                    var section = new SwitchSection();
                    section.Labels.AddRange(switchSection.Labels);

                    foreach (var statement in switchSection.Statements)
                    {
                        section.Statements.AddRange(types.TraverseStatement(statement));
                    }

                    destinationSwitch.Sections.Add(section);
                }

                destinationSwitch.Expression = sourceSwitch.Expression;

                return [destinationSwitch];

            case If sourceIf:
                var destinationÍf = new If();

                foreach (var ifElseSection in sourceIf.Sections)
                {
                    var section = new IfElseSection();

                    foreach (var statement in ifElseSection.Statements)
                    {
                        section.Statements.AddRange(types.TraverseStatement(statement));
                    }

                    section.Condition = ifElseSection.Condition;

                    destinationÍf.Sections.Add(section);
                }

                return [destinationÍf];

            default:
                return [];
        }
    }

    /// <summary>
    /// Populates the inheritance hierarchy for all types by adding inherited base types recursively.
    /// This method modifies the BaseTypes collection of each type to include all inherited types from the inheritance chain.
    /// </summary>
    /// <param name="types">The collection of type descriptions to process.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="types"/> is <c>null</c>.</exception>
    public static void PopulateInheritedBaseTypes(this IEnumerable<TypeDescription> types)
    {
        ArgumentNullException.ThrowIfNull(types);

        foreach (var type in types)
        {
            for (var i = 0; i < type.BaseTypes.Count; i++)
            {
                string baseType = type.BaseTypes[i];

                types.PopulateInheritedBaseTypes(baseType, type.BaseTypes);
            }
        }
    }

    private static void PopulateInheritedBaseTypes(this IEnumerable<TypeDescription> types, string fullName, List<string> baseTypes)
    {
        var type = types.FirstOrDefault(fullName);
        if (type is null)
        {
            return;
        }

        foreach (var baseType in type.BaseTypes)
        {
            if (!baseTypes.Contains(baseType))
            {
                baseTypes.Add(baseType);
            }

            types.PopulateInheritedBaseTypes(baseType, baseTypes);
        }
    }

    /// <summary>
    /// Populates inherited members (fields, properties, methods, constructors, enum members, and events) from base types into derived types.
    /// This method adds non-private members from base types to derived types if they are not already present.
    /// </summary>
    /// <param name="types">The collection of type descriptions to process.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="types"/> is <c>null</c>.</exception>
    /// <remarks>
    /// This is a simplified inheritance implementation that does not handle complex scenarios like method overrides or hiding.
    /// </remarks>
    public static void PopulateInheritedMembers(this IEnumerable<TypeDescription> types)
    {
        ArgumentNullException.ThrowIfNull(types);

        foreach (var type in types)
        {
            foreach (string baseType in type.BaseTypes)
            {
                var inheretedType = types.FirstOrDefault(baseType);
                if (inheretedType is null)
                {
                    continue;
                }

                InheritMember(type, type.Fields, inheretedType.Fields);
                InheritMember(type, type.Constructors, inheretedType.Constructors);
                InheritMember(type, type.Properties, inheretedType.Properties);
                InheritMember(type, type.Methods, inheretedType.Methods);
                InheritMember(type, type.EnumMembers, inheretedType.EnumMembers);
                InheritMember(type, type.Events, inheretedType.Events);
            }
        }
    }

    private static void InheritMember(TypeDescription type, IReadOnlyList<MemberDescription> typeMembers, IReadOnlyList<MemberDescription> baseTypeMembers)
    {
        foreach (var member in baseTypeMembers.Where(f => !f.IsPrivate()))
        {
            // TODO: More complex support for overrides, etc.
            if (!typeMembers.Contains(member))
            {
                // TODO: Clone?
                type.AddMember(member);
            }
        }
    }
}
