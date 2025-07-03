
using PlantUml.Builder;

namespace DendroDocs.Uml.Extensions.Tests;

[TestClass]
public class IHaveModifiersExtensionsTests
{
    [TestMethod]
    public void ToPlantUmlVisibility_NullModifiers_Should_Throw()
    {
        // Arrange
        IHaveModifiers modifiers = null!;

        // Act
        Action action = () => modifiers.ToPlantUmlVisibility();

        // Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [TestMethod]
    public void ToPlantUmlVisibility_PublicModifier_Should_ReturnPublic()
    {
        // Arrange
        var modifiers = new TestModifiers { Modifiers = Modifier.Public };

        // Act
        var result = modifiers.ToPlantUmlVisibility();

        // Assert
        result.ShouldBe(VisibilityModifier.Public);
    }

    [TestMethod]
    public void ToPlantUmlVisibility_InternalModifier_Should_ReturnPackagePrivate()
    {
        // Arrange
        var modifiers = new TestModifiers { Modifiers = Modifier.Internal };

        // Act
        var result = modifiers.ToPlantUmlVisibility();

        // Assert
        result.ShouldBe(VisibilityModifier.PackagePrivate);
    }

    [TestMethod]
    public void ToPlantUmlVisibility_ProtectedModifier_Should_ReturnProtected()
    {
        // Arrange
        var modifiers = new TestModifiers { Modifiers = Modifier.Protected };

        // Act
        var result = modifiers.ToPlantUmlVisibility();

        // Assert
        result.ShouldBe(VisibilityModifier.Protected);
    }

    [TestMethod]
    public void ToPlantUmlVisibility_PrivateModifier_Should_ReturnPrivate()
    {
        // Arrange
        var modifiers = new TestModifiers { Modifiers = Modifier.Private };

        // Act
        var result = modifiers.ToPlantUmlVisibility();

        // Assert
        result.ShouldBe(VisibilityModifier.Private);
    }

    [TestMethod]
    public void ToPlantUmlVisibility_NoVisibilityModifier_Should_ReturnNone()
    {
        // Arrange
        var modifiers = new TestModifiers { Modifiers = Modifier.Static };

        // Act
        var result = modifiers.ToPlantUmlVisibility();

        // Assert
        result.ShouldBe(VisibilityModifier.None);
    }

    [TestMethod]
    public void ToPlantUmlVisibility_NoModifiers_Should_ReturnNone()
    {
        // Arrange
        var modifiers = new TestModifiers();

        // Act
        var result = modifiers.ToPlantUmlVisibility();

        // Assert
        result.ShouldBe(VisibilityModifier.None);
    }

    [TestMethod]
    public void ToPlantUmlVisibility_CombinedModifiers_Should_ReturnFirstVisibilityMatch()
    {
        // Arrange
        var modifiers = new TestModifiers { Modifiers = Modifier.Public | Modifier.Static };

        // Act
        var result = modifiers.ToPlantUmlVisibility();

        // Assert
        result.ShouldBe(VisibilityModifier.Public);
    }

    [TestMethod]
    public void ToPlantUmlVisibility_PriorityOrder_Should_ReturnPublicFirst()
    {
        // Arrange - This scenario shouldn't occur in real code, but tests the method's priority
        var modifiers = new TestModifiers { Modifiers = Modifier.Public | Modifier.Private };

        // Act
        var result = modifiers.ToPlantUmlVisibility();

        // Assert
        result.ShouldBe(VisibilityModifier.Public);
    }

    [TestMethod]
    public void ToPlantUmlVisibility_InternalOverProtected_Should_ReturnPackagePrivate()
    {
        // Arrange
        var modifiers = new TestModifiers { Modifiers = Modifier.Internal | Modifier.Protected };

        // Act
        var result = modifiers.ToPlantUmlVisibility();

        // Assert
        result.ShouldBe(VisibilityModifier.PackagePrivate);
    }

    [TestMethod]
    public void ToPlantUmlVisibility_ProtectedOverPrivate_Should_ReturnProtected()
    {
        // Arrange
        var modifiers = new TestModifiers { Modifiers = Modifier.Protected | Modifier.Private };

        // Act
        var result = modifiers.ToPlantUmlVisibility();

        // Assert
        result.ShouldBe(VisibilityModifier.Protected);
    }

    // Helper class for testing IHaveModifiers
    private class TestModifiers : IHaveModifiers
    {
        public Modifier Modifiers { get; set; }
    }
}
