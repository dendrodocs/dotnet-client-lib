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

    [TestMethod]
    public void GetInvokedMethod_CaseSensitiveContainingType_Should_ReturnEmpty()
    {
        // Assign
        var testType = new TypeDescription(TypeType.Class, "testtype");
        var method = new MethodDescription("void", "TestMethod");
        testType.AddMember(method);

        var types = new[] { testType };
        var invocation = new InvocationDescription("TestType", "TestMethod"); // Different case

        // Act
        var result = types.GetInvokedMethod(invocation);

        // Assert
        result.ShouldBeEmpty();
    }

    [TestMethod]
    public void GetInvokedMethod_MultipleTypesWithSameName_Should_ReturnFromFirstMatch()
    {
        // Assign
        var testType1 = new TypeDescription(TypeType.Class, "TestType");
        var method1 = new MethodDescription("void", "TestMethod");
        testType1.AddMember(method1);

        var testType2 = new TypeDescription(TypeType.Class, "TestType"); // Same name
        var method2 = new MethodDescription("int", "TestMethod");
        testType2.AddMember(method2);

        var types = new[] { testType1, testType2 };
        var invocation = new InvocationDescription("TestType", "TestMethod");

        // Act
        var result = types.GetInvokedMethod(invocation);

        // Assert
        result.ShouldHaveSingleItem();
        var methodResult = result[0] as MethodDescription;
        methodResult.ShouldNotBeNull();
        methodResult.ReturnType.ShouldBe("void"); // Should get the first match
    }
}