namespace DendroDocs.Extensions.Tests;

public partial class IEnumerableTypeDescriptionExtensionsTests
{
    [TestMethod]
    public void TraverseStatement_NullTypes_Should_Throw()
    {
        // Assign
        IEnumerable<TypeDescription> types = default!;
        var statement = new InvocationDescription("TestType", "TestMethod");

        // Act
        Action action = () => types.TraverseStatement(statement);

        // Assert
        action.ShouldThrow<ArgumentNullException>()
            .ParamName.ShouldBe("types");
    }

    [TestMethod]
    public void TraverseStatement_InvocationDescription_Should_ReturnConsequenceStatements()
    {
        // Assign
        var testType = new TypeDescription(TypeType.Class, "TestType");
        var method = new MethodDescription("void", "TestMethod");
        testType.AddMember(method);

        var types = new[] { testType };
        var invocation = new InvocationDescription("TestType", "TestMethod");

        // Act
        var result = types.TraverseStatement(invocation);

        // Assert
        result.ShouldHaveSingleItem();
        result[0].ShouldBe(invocation);
    }

    [TestMethod]
    public void TraverseStatement_SwitchStatement_Should_TraverseAllSections()
    {
        // Assign
        var testType = new TypeDescription(TypeType.Class, "TestType");
        var method = new MethodDescription("void", "TestMethod");
        testType.AddMember(method);

        var types = new[] { testType };
        
        var switchStatement = new Switch { Expression = "value" };
        var section1 = new SwitchSection();
        section1.Labels.Add("case1");
        section1.Statements.Add(new InvocationDescription("TestType", "TestMethod"));
        
        var section2 = new SwitchSection();
        section2.Labels.Add("case2");
        section2.Statements.Add(new InvocationDescription("TestType", "TestMethod"));
        
        switchStatement.Sections.Add(section1);
        switchStatement.Sections.Add(section2);

        // Act
        var result = types.TraverseStatement(switchStatement);

        // Assert
        result.ShouldHaveSingleItem();
        var resultSwitch = result[0].ShouldBeOfType<Switch>();
        resultSwitch.Sections.Count.ShouldBe(2);
        resultSwitch.Expression.ShouldBe("value");
        
        // Check that each section has traversed statements
        resultSwitch.Sections[0].Statements.ShouldHaveSingleItem();
        resultSwitch.Sections[1].Statements.ShouldHaveSingleItem();
    }

    [TestMethod]
    public void TraverseStatement_IfStatement_Should_TraverseAllSections()
    {
        // Assign
        var testType = new TypeDescription(TypeType.Class, "TestType");
        var method = new MethodDescription("void", "TestMethod");
        testType.AddMember(method);

        var types = new[] { testType };
        
        var ifStatement = new If();
        var section1 = new IfElseSection { Condition = "condition1" };
        section1.Statements.Add(new InvocationDescription("TestType", "TestMethod"));
        
        var section2 = new IfElseSection { Condition = "condition2" };
        section2.Statements.Add(new InvocationDescription("TestType", "TestMethod"));
        
        ifStatement.Sections.Add(section1);
        ifStatement.Sections.Add(section2);

        // Act
        var result = types.TraverseStatement(ifStatement);

        // Assert
        result.ShouldHaveSingleItem();
        var resultIf = result[0].ShouldBeOfType<If>();
        resultIf.Sections.Count.ShouldBe(2);
        
        // Check that each section has traversed statements and correct conditions
        resultIf.Sections[0].Statements.ShouldHaveSingleItem();
        resultIf.Sections[0].Condition.ShouldBe("condition1");
        resultIf.Sections[1].Statements.ShouldHaveSingleItem();
        resultIf.Sections[1].Condition.ShouldBe("condition2");
    }

    [TestMethod]
    public void TraverseStatement_UnknownStatementType_Should_ReturnEmptyList()
    {
        // Assign
        var types = new[] { new TypeDescription(TypeType.Class, "TestType") };
        var unknownStatement = new TestStatement(); // Create a mock statement type

        // Act
        var result = types.TraverseStatement(unknownStatement);

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

    // Helper class for testing unknown statement types
    private class TestStatement : Statement
    {
        // This represents any statement type not handled by TraverseStatement
    }
}