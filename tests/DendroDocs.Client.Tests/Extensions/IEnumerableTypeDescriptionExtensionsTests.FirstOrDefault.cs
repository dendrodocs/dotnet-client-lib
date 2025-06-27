namespace DendroDocs.Extensions.Tests;

public partial class IEnumerableTypeDescriptionExtensionsTests
{
    [TestMethod]
    public void FirstOrDefault_NullTypes_Should_Throw()
    {
        // Assign
        IEnumerable<TypeDescription> types = default!;

        // Act
        Action action = () => types.FirstOrDefault("System.Object");

        // Assert
        action.ShouldThrow<ArgumentNullException>()
            .ParamName.ShouldBe("types");
    }

    [TestMethod]
    public void FirstOrDefault_TypeExists_Should_ReturnType()
    {
        // Assign
        var types = new[]
        {
            new TypeDescription(TypeType.Class, "TestType1"),
            new TypeDescription(TypeType.Class, "TestType2")
        };

        // Act
        var result = types.FirstOrDefault("TestType1");

        // Assert
        result.ShouldNotBeNull();
        result.FullName.ShouldBe("TestType1");
    }

    [TestMethod]
    public void FirstOrDefault_TypeDoesNotExist_Should_ReturnNull()
    {
        // Assign
        var types = new[]
        {
            new TypeDescription(TypeType.Class, "TestType1"),
            new TypeDescription(TypeType.Class, "TestType2")
        };

        // Act
        var result = types.FirstOrDefault("NonExistentType");

        // Assert
        result.ShouldBeNull();
    }

    [TestMethod]
    public void FirstOrDefault_EmptyCollection_Should_ReturnNull()
    {
        // Assign
        var types = Array.Empty<TypeDescription>();

        // Act
        var result = types.FirstOrDefault("TestType");

        // Assert
        result.ShouldBeNull();
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
}