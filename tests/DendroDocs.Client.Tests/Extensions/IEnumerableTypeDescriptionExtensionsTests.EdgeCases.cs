namespace DendroDocs.Extensions.Tests;

public partial class IEnumerableTypeDescriptionExtensionsTests
{
    [TestMethod]
    public void First_CaseSensitiveSearch_Should_NotMatch()
    {
        // Assign
        var types = new[]
        {
            new TypeDescription(TypeType.Class, "testtype"),
            new TypeDescription(TypeType.Class, "AnotherType")
        };

        // Act
        Action action = () => types.First("TestType");

        // Assert
        action.ShouldThrow<InvalidOperationException>();
    }

    [TestMethod]
    public void FirstOrDefault_CaseSensitiveSearch_Should_ReturnNull()
    {
        // Assign
        var types = new[]
        {
            new TypeDescription(TypeType.Class, "testtype"),
            new TypeDescription(TypeType.Class, "AnotherType")
        };

        // Act
        var result = types.FirstOrDefault("TestType");

        // Assert
        result.ShouldBeNull();
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
    public void TraverseStatement_SwitchWithNoSections_Should_ReturnEmptySwitch()
    {
        // Assign
        var types = new[] { new TypeDescription(TypeType.Class, "TestType") };
        var switchStatement = new Switch { Expression = "value" };

        // Act
        var result = types.TraverseStatement(switchStatement);

        // Assert
        result.ShouldHaveSingleItem();
        var resultSwitch = result[0].ShouldBeOfType<Switch>();
        resultSwitch.Sections.ShouldBeEmpty();
        resultSwitch.Expression.ShouldBe("value");
    }

    [TestMethod]
    public void TraverseStatement_IfWithNoSections_Should_ReturnEmptyIf()
    {
        // Assign
        var types = new[] { new TypeDescription(TypeType.Class, "TestType") };
        var ifStatement = new If();

        // Act
        var result = types.TraverseStatement(ifStatement);

        // Assert
        result.ShouldHaveSingleItem();
        var resultIf = result[0].ShouldBeOfType<If>();
        resultIf.Sections.ShouldBeEmpty();
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

    [TestMethod]
    public void TraverseStatement_NestedSwitchInIf_Should_TraverseRecursively()
    {
        // Assign
        var testType = new TypeDescription(TypeType.Class, "TestType");
        var method = new MethodDescription("void", "TestMethod");
        testType.AddMember(method);

        var types = new[] { testType };
        
        var ifStatement = new If();
        var ifSection = new IfElseSection { Condition = "condition" };
        
        var nestedSwitch = new Switch { Expression = "nested" };
        var switchSection = new SwitchSection();
        switchSection.Labels.Add("case1");
        switchSection.Statements.Add(new InvocationDescription("TestType", "TestMethod"));
        nestedSwitch.Sections.Add(switchSection);
        
        ifSection.Statements.Add(nestedSwitch);
        ifStatement.Sections.Add(ifSection);

        // Act
        var result = types.TraverseStatement(ifStatement);

        // Assert
        result.ShouldHaveSingleItem();
        var resultIf = result[0].ShouldBeOfType<If>();
        resultIf.Sections.ShouldHaveSingleItem();
        
        var resultIfSection = resultIf.Sections[0];
        resultIfSection.Statements.ShouldHaveSingleItem();
        var nestedSwitchResult = resultIfSection.Statements[0].ShouldBeOfType<Switch>();
        nestedSwitchResult.Sections.ShouldHaveSingleItem();
        nestedSwitchResult.Sections[0].Statements.ShouldHaveSingleItem();
    }
}