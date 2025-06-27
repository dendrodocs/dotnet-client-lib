namespace DendroDocs.Extensions.Tests;

public partial class IEnumerableTypeDescriptionExtensionsTests
{
    [TestMethod]
    public void GetInvokedMethod_NullTypes_Should_Throw()
    {
        // Assign
        IEnumerable<TypeDescription> types = default!;
        var invocation = new InvocationDescription("TestType", "TestMethod");

        // Act
        Action action = () => types.GetInvokedMethod(invocation);

        // Assert
        action.ShouldThrow<ArgumentNullException>()
            .ParamName.ShouldBe("types");
    }

    [TestMethod]
    public void GetInvokedMethod_TypeNotFound_Should_ReturnEmptyList()
    {
        // Assign
        var types = new[]
        {
            new TypeDescription(TypeType.Class, "TestType1")
        };
        var invocation = new InvocationDescription("NonExistentType", "TestMethod");

        // Act
        var result = types.GetInvokedMethod(invocation);

        // Assert
        result.ShouldBeEmpty();
    }

    [TestMethod]
    public void GetInvokedMethod_TypeFoundWithMatchingMethod_Should_ReturnMethods()
    {
        // Assign
        var testType = new TypeDescription(TypeType.Class, "TestType");
        var method = new MethodDescription("void", "TestMethod");
        testType.AddMember(method);

        var types = new[] { testType };
        var invocation = new InvocationDescription("TestType", "TestMethod");

        // Act
        var result = types.GetInvokedMethod(invocation);

        // Assert
        result.ShouldHaveSingleItem();
        result[0].Name.ShouldBe("TestMethod");
    }

    [TestMethod]
    public void GetInvokedMethod_TypeFoundWithNonMatchingMethod_Should_ReturnEmptyList()
    {
        // Assign
        var testType = new TypeDescription(TypeType.Class, "TestType");
        var method = new MethodDescription("void", "DifferentMethod");
        testType.AddMember(method);

        var types = new[] { testType };
        var invocation = new InvocationDescription("TestType", "TestMethod");

        // Act
        var result = types.GetInvokedMethod(invocation);

        // Assert
        result.ShouldBeEmpty();
    }

    [TestMethod]
    public void GetInvokedMethod_TypeFoundWithMultipleMatchingMethods_Should_ReturnAllMatches()
    {
        // Assign
        var testType = new TypeDescription(TypeType.Class, "TestType");
        var method1 = new MethodDescription("void", "TestMethod");
        var method2 = new MethodDescription("int", "TestMethod");
        testType.AddMember(method1);
        testType.AddMember(method2);

        var types = new[] { testType };
        var invocation = new InvocationDescription("TestType", "TestMethod");

        // Act
        var result = types.GetInvokedMethod(invocation);

        // Assert
        result.Count.ShouldBe(2);
        result.All(m => m.Name == "TestMethod").ShouldBeTrue();
    }
}