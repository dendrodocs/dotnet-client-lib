namespace DendroDocs.Extensions.Tests;

[TestClass]
public partial class IEnumerableTypeDescriptionExtensionsTests
{
    [TestMethod]
    public void First_NullTypes_Should_Throw()
    {
        // Assign
        IEnumerable<TypeDescription> types = default!;

        // Act
        Action action = () => types.First("System.Object");

        // Assert
        action.ShouldThrow<ArgumentNullException>()
            .ParamName.ShouldBe("types");
    }

    [TestMethod]
    public void First_TypeExists_Should_ReturnType()
    {
        // Assign
        var types = new[]
        {
            new TypeDescription(TypeType.Class, "TestType1"),
            new TypeDescription(TypeType.Class, "TestType2")
        };

        // Act
        var result = types.First("TestType1");

        // Assert
        result.ShouldNotBeNull();
        result.FullName.ShouldBe("TestType1");
    }

    [TestMethod]
    public void First_TypeDoesNotExist_Should_Throw()
    {
        // Assign
        var types = new[]
        {
            new TypeDescription(TypeType.Class, "TestType1"),
            new TypeDescription(TypeType.Class, "TestType2")
        };

        // Act
        Action action = () => types.First("NonExistentType");

        // Assert
        action.ShouldThrow<InvalidOperationException>();
    }

    [TestMethod]
    public void First_EmptyCollection_Should_Throw()
    {
        // Assign
        var types = Array.Empty<TypeDescription>();

        // Act
        Action action = () => types.First("TestType");

        // Assert
        action.ShouldThrow<InvalidOperationException>();
    }

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
}