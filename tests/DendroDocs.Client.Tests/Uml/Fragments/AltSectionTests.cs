namespace DendroDocs.Uml.Fragments.Tests;

[TestClass]
public class AltSectionTests
{
    [TestMethod]
    public void Constructor_Should_InitializeWithNullProperties()
    {
        // Act
        var section = new AltSection();

        // Assert
        section.GroupType.ShouldBeNull();
        section.Label.ShouldBeNull();
        section.Fragments.ShouldBeEmpty();
    }

    [TestMethod]
    public void GroupType_SetAndGet_Should_WorkCorrectly()
    {
        // Arrange
        var section = new AltSection();
        const string groupType = "alt";

        // Act
        section.GroupType = groupType;

        // Assert
        section.GroupType.ShouldBe(groupType);
    }

    [TestMethod]
    public void Label_SetAndGet_Should_WorkCorrectly()
    {
        // Arrange
        var section = new AltSection();
        const string label = "condition1";

        // Act
        section.Label = label;

        // Assert
        section.Label.ShouldBe(label);
    }

    [TestMethod]
    public void GroupType_SetToNull_Should_AcceptNull()
    {
        // Arrange
        var section = new AltSection { GroupType = "alt" };

        // Act
        section.GroupType = null;

        // Assert
        section.GroupType.ShouldBeNull();
    }

    [TestMethod]
    public void Label_SetToNull_Should_AcceptNull()
    {
        // Arrange
        var section = new AltSection { Label = "condition" };

        // Act
        section.Label = null;

        // Assert
        section.Label.ShouldBeNull();
    }

    [TestMethod]
    public void AddFragment_Should_WorkAsInteractionFragment()
    {
        // Arrange
        var section = new AltSection { GroupType = "alt", Label = "condition" };
        var arrow = new Arrow { Source = "A", Target = "B", Name = "message" };

        // Act
        section.AddFragment(arrow);

        // Assert
        section.Fragments.ShouldHaveSingleItem();
        section.Fragments[0].ShouldBe(arrow);
        arrow.Parent.ShouldBe(section);
    }

    [TestMethod]
    public void InheritsFromInteractionFragment_Should_HaveFragmentBehavior()
    {
        // Arrange
        var alt = new Alt();
        var section = new AltSection { GroupType = "alt", Label = "condition" };

        // Act
        alt.AddSection(section);

        // Assert
        section.Parent.ShouldBe(alt);
        alt.Sections.ShouldHaveSingleItem();
        alt.Sections[0].ShouldBe(section);
    }

    [TestMethod]
    public void DebuggerDisplay_Should_ShowCorrectFormat()
    {
        // Arrange
        var section = new AltSection { GroupType = "alt", Label = "x > 0" };

        // Act & Assert
        // Note: We can't directly test DebuggerDisplay attribute, but we can verify the properties exist
        section.GroupType.ShouldBe("alt");
        section.Label.ShouldBe("x > 0");
    }

    [TestMethod]
    public void AddFragments_MultipleLevels_Should_BuildHierarchy()
    {
        // Arrange
        var section = new AltSection { GroupType = "alt", Label = "condition" };
        var nestedAlt = new Alt();
        var nestedSection = new AltSection { GroupType = "opt", Label = "nested" };
        var arrow = new Arrow { Source = "A", Target = "B" };

        // Act
        nestedSection.AddFragment(arrow);
        nestedAlt.AddSection(nestedSection);
        section.AddFragment(nestedAlt);

        // Assert
        section.Fragments.ShouldHaveSingleItem();
        nestedAlt.Parent.ShouldBe(section);
        nestedSection.Parent.ShouldBe(nestedAlt);
        arrow.Parent.ShouldBe(nestedSection);
    }
}
