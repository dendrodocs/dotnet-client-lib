namespace DendroDocs.Extensions.Tests;

public partial class IEnumerableTypeDescriptionExtensionsTests
{
    [TestMethod]
    public void GetInvocationConsequences_NullTypes_Should_Throw()
    {
        // Assign
        IEnumerable<TypeDescription> types = default!;
        var invocation = new InvocationDescription("TestType", "TestMethod");

        // Act
        Action action = () => types.GetInvocationConsequences(invocation);

        // Assert
        action.ShouldThrow<ArgumentNullException>()
            .ParamName.ShouldBe("types");
    }

    [TestMethod]
    public void GetInvocationConsequences_NoMatchingMethod_Should_ReturnOnlyOriginalInvocation()
    {
        // Assign
        var types = new[]
        {
            new TypeDescription(TypeType.Class, "TestType")
        };
        var invocation = new InvocationDescription("TestType", "NonExistentMethod");

        // Act
        var result = types.GetInvocationConsequences(invocation);

        // Assert
        result.ShouldHaveSingleItem();
        result[0].ShouldBe(invocation);
    }

    [TestMethod]
    public void GetInvocationConsequences_MethodWithNoStatements_Should_ReturnOnlyOriginalInvocation()
    {
        // Assign
        var testType = new TypeDescription(TypeType.Class, "TestType");
        var method = new MethodDescription("void", "TestMethod");
        testType.AddMember(method);

        var types = new[] { testType };
        var invocation = new InvocationDescription("TestType", "TestMethod");

        // Act
        var result = types.GetInvocationConsequences(invocation);

        // Assert
        result.ShouldHaveSingleItem();
        result[0].ShouldBe(invocation);
    }

    [TestMethod]
    public void GetInvocationConsequences_MethodWithInvocationStatements_Should_ReturnAllConsequences()
    {
        // Assign
        var testType1 = new TypeDescription(TypeType.Class, "TestType1");
        var method1 = new MethodDescription("void", "Method1");
        var nestedInvocation = new InvocationDescription("TestType2", "Method2");
        method1.Statements.Add(nestedInvocation);
        testType1.AddMember(method1);

        var testType2 = new TypeDescription(TypeType.Class, "TestType2");
        var method2 = new MethodDescription("void", "Method2");
        testType2.AddMember(method2);

        var types = new[] { testType1, testType2 };
        var originalInvocation = new InvocationDescription("TestType1", "Method1");

        // Act
        var result = types.GetInvocationConsequences(originalInvocation);

        // Assert
        result.Count.ShouldBe(2);
        result.ShouldContain(originalInvocation);
        result.ShouldContain(nestedInvocation);
    }

    [TestMethod]
    public void GetInvocationConsequences_DeepNestedInvocations_Should_ReturnAllLevels()
    {
        // Assign
        var testType1 = new TypeDescription(TypeType.Class, "TestType1");
        var method1 = new MethodDescription("void", "Method1");
        var invocation2 = new InvocationDescription("TestType2", "Method2");
        method1.Statements.Add(invocation2);
        testType1.AddMember(method1);

        var testType2 = new TypeDescription(TypeType.Class, "TestType2");
        var method2 = new MethodDescription("void", "Method2");
        var invocation3 = new InvocationDescription("TestType3", "Method3");
        method2.Statements.Add(invocation3);
        testType2.AddMember(method2);

        var testType3 = new TypeDescription(TypeType.Class, "TestType3");
        var method3 = new MethodDescription("void", "Method3");
        testType3.AddMember(method3);

        var types = new[] { testType1, testType2, testType3 };
        var originalInvocation = new InvocationDescription("TestType1", "Method1");

        // Act
        var result = types.GetInvocationConsequences(originalInvocation);

        // Assert
        result.Count.ShouldBe(3);
        result.ShouldContain(originalInvocation);
        result.ShouldContain(invocation2);
        result.ShouldContain(invocation3);
    }
}