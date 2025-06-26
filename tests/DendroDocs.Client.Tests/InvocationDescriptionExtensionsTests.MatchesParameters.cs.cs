namespace DendroDocs.Tests;

[TestClass]
public partial class InvocationDescriptionExtensionsTests
{
    [TestMethod]
    public void MatchesParameters_NullMethod_Should_Throw()
    {
        // Assign
        var invocation = new InvocationDescription("System.Object", "Method");

        // Act
        Action action = () => invocation.MatchesParameters(default!);

        // Assert
        action.ShouldThrow<ArgumentNullException>()
            .ParamName.ShouldBe("method");
    }

    [TestMethod]
    public void MatchesParameters_NullInvocation_Should_Throw()
    {
        // Assign
        var method = new MethodDescription("void", "Method");

        // Act
        Action action = () => ((InvocationDescription)default!).MatchesParameters(method);

        // Assert
        action.ShouldThrow<ArgumentNullException>()
            .ParamName.ShouldBe("invocation");
    }

    [TestMethod]
    public void MatchesParameters_InvocationWithoutArguments_And_MethodWithoutParameters_Should_Match()
    {
        // Assign
        var method = new MethodDescription("void", "Method");

        var invocation = new InvocationDescription("System.Object", "Method");

        // Act
        var result = invocation.MatchesParameters(method);

        // Assert
        result.ShouldBeTrue();
    }

    [TestMethod]
    public void MatchesParameters_InvocationWithNoArguments_And_MethodWithParameters_Should_NotMatch()
    {
        // Assign
        var method = new MethodDescription("void", "Method");
        method.Parameters.Add(new ParameterDescription("string", "parameter1"));

        var invocation = new InvocationDescription("System.Object", "Method");

        // Act
        var result = invocation.MatchesParameters(method);

        // Assert
        result.ShouldBeFalse();
    }

    [TestMethod]
    public void MatchesParameters_InvocationWithArguments_And_MethodWithMoreParameters_Should_NotMatch()
    {
        // Assign
        var method = new MethodDescription("void", "Method");
        method.Parameters.Add(new ParameterDescription("string", "parameter1"));
        method.Parameters.Add(new ParameterDescription("string", "parameter2"));

        var invocation = new InvocationDescription("System.Object", "Method");
        invocation.Arguments.Add(new ArgumentDescription("string", "attribute1"));

        // Act
        var result = invocation.MatchesParameters(method);

        // Assert
        result.ShouldBeFalse();
    }

    [TestMethod]
    public void MatchesParameters_InvocationWithArguments_And_MethodWithLessParameters_Should_NotMatch()
    {
        // Assign
        var method = new MethodDescription("void", "Method");
        method.Parameters.Add(new ParameterDescription("string", "parameter1"));

        var invocation = new InvocationDescription("System.Object", "Method");
        invocation.Arguments.Add(new ArgumentDescription("string", "attribute1"));
        invocation.Arguments.Add(new ArgumentDescription("string", "attribute2"));

        // Act
        var result = invocation.MatchesParameters(method);

        // Assert
        result.ShouldBeFalse();
    }

    [TestMethod]
    public void MatchesParameters_InvocationWithSameArguments_And_MethodParameters_Should_Match()
    {
        // Assign
        var method = new MethodDescription("void", "Method");
        method.Parameters.Add(new ParameterDescription("string", "parameter1"));

        var invocation = new InvocationDescription("System.Object", "Method");
        invocation.Arguments.Add(new ArgumentDescription("string", "attribute1"));

        // Act
        var result = invocation.MatchesParameters(method);

        // Assert
        result.ShouldBeTrue();
    }

    [TestMethod]
    public void MatchesParameters_InvocationWithDifferentArguments_And_MethodParameters_Should_NotMatch()
    {
        // Assign
        var method = new MethodDescription("void", "Method");
        method.Parameters.Add(new ParameterDescription("string", "parameter1"));

        var invocation = new InvocationDescription("System.Object", "Method");
        invocation.Arguments.Add(new ArgumentDescription("int", "attribute1"));

        // Act
        var result = invocation.MatchesParameters(method);

        // Assert
        result.ShouldBeFalse();
    }

    [TestMethod]
    public void MatchesParameters_InvocationWithArguments_And_MethodWithMoreOptionalParameters_Should_Match()
    {
        // Assign
        var method = new MethodDescription("void", "Method");
        method.Parameters.Add(new ParameterDescription("string", "parameter1"));
        method.Parameters.Add(new ParameterDescription("string", "parameter2") { HasDefaultValue = true });

        var invocation = new InvocationDescription("System.Object", "Method");
        invocation.Arguments.Add(new ArgumentDescription("string", "attribute1"));

        // Act
        var result = invocation.MatchesParameters(method);

        // Assert
        result.ShouldBeTrue();
    }
}
