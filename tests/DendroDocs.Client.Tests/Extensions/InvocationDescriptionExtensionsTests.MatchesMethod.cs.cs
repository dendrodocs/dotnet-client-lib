namespace DendroDocs.Extensions.Tests;

public partial class InvocationDescriptionExtensionsTests
{
    [TestMethod]
    public void MatchesMethod_NullMethod_Should_Throw()
    {
        // Assign
        var invocation = new InvocationDescription("System.Object", "Method");

        // Act
        Action action = () => invocation.MatchesMethod(default!);

        // Assert
        action.ShouldThrow<ArgumentNullException>()
            .ParamName.ShouldBe("method");
    }

    [TestMethod]
    public void MatchesMethod_NullInvocation_Should_Throw()
    {
        // Assign
        var method = new MethodDescription("void", "Method");

        // Act
        Action action = () => ((InvocationDescription)default!).MatchesMethod(method);

        // Assert
        action.ShouldThrow<ArgumentNullException>()
            .ParamName.ShouldBe("invocation");
    }

    [TestMethod]
    public void MatchesMethod_InvocationWithoutArguments_And_MethodWithoutParameters_Should_Match()
    {
        // Assign
        var method = new MethodDescription("void", "Method");

        var invocation = new InvocationDescription("System.Object", "Method");

        // Act
        var result = invocation.MatchesMethod(method);

        // Assert
        result.ShouldBeTrue();
    }

    [TestMethod]
    public void MatchesMethod_InvocationNameEqualsMethodName_Should_Match()
    {
        // Assign
        var method = new MethodDescription("void", "Method");

        var invocation = new InvocationDescription("System.Object", "Method");

        // Act
        var result = invocation.MatchesMethod(method);

        // Assert
        result.ShouldBeTrue();
    }

    [TestMethod]
    public void MatchesMethod_InvocationNameNotEqualsMethodName_Should_NotMatch()
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
}
