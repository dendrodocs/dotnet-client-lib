namespace DendroDocs.Extensions.Tests;

public partial class StringExtensionsTests
{
    [TestMethod]
    public void ToSentenceCase_StringWithSpaces_Should_HandleCorrectly()
    {
        // Assign
        var input = "Test Method";

        // Act
        var result = input.ToSentenceCase();

        // Assert
        result.ShouldBe("Test  Method"); // Space before capital M
    }

    [TestMethod]
    public void ToSentenceCase_StringWithSpecialCharacters_Should_HandleCorrectly()
    {
        // Assign
        var input = "Test_Method$Name";

        // Act
        var result = input.ToSentenceCase();

        // Assert
        result.ShouldBe("Test_ Method$ Name"); // Spaces before capital letters
    }

    [TestMethod]
    public void ToSentenceCase_SingleCharacter_Should_HandleCorrectly()
    {
        // Assign
        var input = "a";

        // Act
        var result = input.ToSentenceCase();

        // Assert
        result.ShouldBe("A");
    }

    [TestMethod]
    public void ToSentenceCase_SingleUpperCharacter_Should_HandleCorrectly()
    {
        // Assign
        var input = "A";

        // Act
        var result = input.ToSentenceCase();

        // Assert
        result.ShouldBe("A");
    }

    [TestMethod]
    public void GenericTypes_NonGenericType_Should_ReturnEmptyList()
    {
        // Assign
        var input = "System.String";

        // Act
        var result = input.GenericTypes();

        // Assert
        result.ShouldBeEmpty();
    }

    [TestMethod]
    public void IsGeneric_NonGenericTypeEndingWithGreaterThan_Should_ReturnFalse()
    {
        // Assign
        var input = "SomeType>NotGeneric";

        // Act
        var result = input.IsGeneric();

        // Assert
        result.ShouldBeFalse();
    }

    [TestMethod]
    public void IsGeneric_TypeWithAngleBracketsButNotAtEnd_Should_ReturnFalse()
    {
        // Assign
        var input = "Some<Type>NotAtEnd";

        // Act
        var result = input.IsGeneric();

        // Assert
        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ForDiagram_NonGenericSimpleType_Should_ReturnAsIs()
    {
        // Assign
        var input = "String";

        // Act
        var result = input.ForDiagram();

        // Assert
        result.ShouldBe("String");
    }

    [TestMethod]
    public void ForDiagram_TypeWithoutDots_Should_ReturnAsIs()
    {
        // Assign
        var input = "SimpleType";

        // Act
        var result = input.ForDiagram();

        // Assert
        result.ShouldBe("SimpleType");
    }
}