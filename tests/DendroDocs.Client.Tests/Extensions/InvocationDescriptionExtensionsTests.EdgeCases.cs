namespace DendroDocs.Extensions.Tests;

public partial class InvocationDescriptionExtensionsTests
{
    [TestMethod]
    public void MatchesMethod_InvocationNameCaseSensitive_Should_NotMatch()
    {
        // Assign
        var method = new MethodDescription("void", "method");
        var invocation = new InvocationDescription("System.Object", "Method");

        // Act
        var result = invocation.MatchesMethod(method);

        // Assert
        result.ShouldBeFalse();
    }

    [TestMethod]
    public void MatchesParameters_InvocationWithExtraArgumentsAndNoOptionalParameters_Should_NotMatch()
    {
        // Assign
        var method = new MethodDescription("void", "Method");
        method.Parameters.Add(new ParameterDescription("string", "param1"));

        var invocation = new InvocationDescription("System.Object", "Method");
        invocation.Arguments.Add(new ArgumentDescription("string", "arg1"));
        invocation.Arguments.Add(new ArgumentDescription("int", "arg2"));

        // Act
        var result = invocation.MatchesParameters(method);

        // Assert
        result.ShouldBeFalse();
    }

    [TestMethod]
    public void MatchesParameters_InvocationWithExactArgumentsAndSomeOptionalParameters_Should_Match()
    {
        // Assign
        var method = new MethodDescription("void", "Method");
        method.Parameters.Add(new ParameterDescription("string", "param1"));
        method.Parameters.Add(new ParameterDescription("int", "param2") { HasDefaultValue = true });
        method.Parameters.Add(new ParameterDescription("bool", "param3") { HasDefaultValue = true });

        var invocation = new InvocationDescription("System.Object", "Method");
        invocation.Arguments.Add(new ArgumentDescription("string", "arg1"));

        // Act
        var result = invocation.MatchesParameters(method);

        // Assert
        result.ShouldBeTrue();
    }

    [TestMethod]
    public void MatchesParameters_InvocationWithPartialArgumentsMatchingOptionalParameters_Should_Match()
    {
        // Assign
        var method = new MethodDescription("void", "Method");
        method.Parameters.Add(new ParameterDescription("string", "param1"));
        method.Parameters.Add(new ParameterDescription("int", "param2") { HasDefaultValue = true });
        method.Parameters.Add(new ParameterDescription("bool", "param3") { HasDefaultValue = true });

        var invocation = new InvocationDescription("System.Object", "Method");
        invocation.Arguments.Add(new ArgumentDescription("string", "arg1"));
        invocation.Arguments.Add(new ArgumentDescription("int", "arg2"));

        // Act
        var result = invocation.MatchesParameters(method);

        // Assert
        result.ShouldBeTrue();
    }

    [TestMethod]
    public void MatchesParameters_InvocationWithAllArgumentsAndOptionalParameters_Should_Match()
    {
        // Assign
        var method = new MethodDescription("void", "Method");
        method.Parameters.Add(new ParameterDescription("string", "param1"));
        method.Parameters.Add(new ParameterDescription("int", "param2") { HasDefaultValue = true });
        method.Parameters.Add(new ParameterDescription("bool", "param3") { HasDefaultValue = true });

        var invocation = new InvocationDescription("System.Object", "Method");
        invocation.Arguments.Add(new ArgumentDescription("string", "arg1"));
        invocation.Arguments.Add(new ArgumentDescription("int", "arg2"));
        invocation.Arguments.Add(new ArgumentDescription("bool", "arg3"));

        // Act
        var result = invocation.MatchesParameters(method);

        // Assert
        result.ShouldBeTrue();
    }
}