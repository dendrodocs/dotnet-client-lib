namespace DendroDocs.Uml.Fragments.Tests;

[TestClass]
public class AltTests
{
    [TestMethod]
    public void Constructor_Should_InitializeEmptySections()
    {
        // Act
        var alt = new Alt();

        // Assert
        alt.Sections.ShouldNotBeNull();
        alt.Sections.ShouldBeEmpty();
    }

    [TestMethod]
    public void AddSection_ValidSection_Should_AddToSections()
    {
        // Arrange
        var alt = new Alt();
        var section = new AltSection { GroupType = "alt", Label = "condition" };

        // Act
        alt.AddSection(section);

        // Assert
        alt.Sections.ShouldHaveSingleItem();
        alt.Sections[0].ShouldBe(section);
        section.Parent.ShouldBe(alt);
    }

    [TestMethod]
    public void AddSection_NullSection_Should_Throw()
    {
        // Arrange
        var alt = new Alt();

        // Act
        Action action = () => alt.AddSection(null!);

        // Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [TestMethod]
    public void AddSection_MultipleSections_Should_MaintainOrder()
    {
        // Arrange
        var alt = new Alt();
        var section1 = new AltSection { GroupType = "alt", Label = "condition1" };
        var section2 = new AltSection { GroupType = "else", Label = "condition2" };
        var section3 = new AltSection { GroupType = "else", Label = "condition3" };

        // Act
        alt.AddSection(section1);
        alt.AddSection(section2);
        alt.AddSection(section3);

        // Assert
        alt.Sections.Count.ShouldBe(3);
        alt.Sections[0].ShouldBe(section1);
        alt.Sections[1].ShouldBe(section2);
        alt.Sections[2].ShouldBe(section3);
    }

    [TestMethod]
    public void AddSection_SectionWithFragments_Should_WorkCorrectly()
    {
        // Arrange
        var alt = new Alt();
        var section = new AltSection { GroupType = "alt", Label = "condition" };
        var arrow = new Arrow { Source = "A", Target = "B", Name = "message" };
        section.AddFragment(arrow);

        // Act
        alt.AddSection(section);

        // Assert
        alt.Sections.ShouldHaveSingleItem();
        alt.Sections[0].Fragments.ShouldHaveSingleItem();
        alt.Sections[0].Fragments[0].ShouldBe(arrow);
        section.Parent.ShouldBe(alt);
    }

    [TestMethod]
    public void InheritsFromInteractionFragment_Should_HaveFragmentBehavior()
    {
        // Arrange
        var parentFragment = new Interactions();
        var alt = new Alt();
        var section = new AltSection { GroupType = "alt", Label = "condition" };
        alt.AddSection(section);

        // Act
        parentFragment.AddFragment(alt);

        // Assert
        alt.Parent.ShouldBe(parentFragment);
        parentFragment.Fragments.ShouldHaveSingleItem();
        parentFragment.Fragments[0].ShouldBe(alt);
    }
}
