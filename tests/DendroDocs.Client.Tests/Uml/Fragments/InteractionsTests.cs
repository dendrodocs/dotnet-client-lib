namespace DendroDocs.Uml.Fragments.Tests;

[TestClass]
public class InteractionsTests
{
    [TestMethod]
    public void Constructor_Should_InitializeAsInteractionFragment()
    {
        // Act
        var interactions = new Interactions();

        // Assert
        interactions.Fragments.ShouldNotBeNull();
        interactions.Fragments.ShouldBeEmpty();
        interactions.Parent.ShouldBeNull();
    }

    [TestMethod]
    public void AddFragment_Arrow_Should_AddToFragments()
    {
        // Arrange
        var interactions = new Interactions();
        var arrow = new Arrow { Source = "A", Target = "B", Name = "message" };

        // Act
        interactions.AddFragment(arrow);

        // Assert
        interactions.Fragments.ShouldHaveSingleItem();
        interactions.Fragments[0].ShouldBe(arrow);
        arrow.Parent.ShouldBe(interactions);
    }

    [TestMethod]
    public void AddFragment_Alt_Should_AddToFragments()
    {
        // Arrange
        var interactions = new Interactions();
        var alt = new Alt();
        var section = new AltSection { GroupType = "alt", Label = "condition" };
        alt.AddSection(section);

        // Act
        interactions.AddFragment(alt);

        // Assert
        interactions.Fragments.ShouldHaveSingleItem();
        interactions.Fragments[0].ShouldBe(alt);
        alt.Parent.ShouldBe(interactions);
    }

    [TestMethod]
    public void AddFragment_NestedInteractions_Should_CreateHierarchy()
    {
        // Arrange
        var parentInteractions = new Interactions();
        var childInteractions = new Interactions();
        var arrow = new Arrow { Source = "A", Target = "B" };

        // Act
        childInteractions.AddFragment(arrow);
        parentInteractions.AddFragment(childInteractions);

        // Assert
        parentInteractions.Fragments.ShouldHaveSingleItem();
        parentInteractions.Fragments[0].ShouldBe(childInteractions);
        childInteractions.Parent.ShouldBe(parentInteractions);
        childInteractions.Fragments.ShouldHaveSingleItem();
        arrow.Parent.ShouldBe(childInteractions);
    }

    [TestMethod]
    public void InheritsFromInteractionFragment_Should_SupportAllFragmentOperations()
    {
        // Arrange
        var interactions = new Interactions();
        var arrow1 = new Arrow { Source = "A", Target = "B", Name = "first" };
        var arrow2 = new Arrow { Source = "B", Target = "C", Name = "second" };
        var fragments = new InteractionFragment[] { arrow1, arrow2 };

        // Act
        interactions.AddFragments(fragments);

        // Assert
        interactions.Fragments.Count.ShouldBe(2);
        interactions.Fragments[0].ShouldBe(arrow1);
        interactions.Fragments[1].ShouldBe(arrow2);
        arrow1.Parent.ShouldBe(interactions);
        arrow2.Parent.ShouldBe(interactions);
    }

    [TestMethod]
    public void CanContainMixedFragmentTypes()
    {
        // Arrange
        var interactions = new Interactions();
        var arrow = new Arrow { Source = "A", Target = "B" };
        var alt = new Alt();
        var section = new AltSection { GroupType = "alt" };
        alt.AddSection(section);
        var nestedInteractions = new Interactions();

        // Act
        interactions.AddFragment(arrow);
        interactions.AddFragment(alt);
        interactions.AddFragment(nestedInteractions);

        // Assert
        interactions.Fragments.Count.ShouldBe(3);
        interactions.Fragments[0].ShouldBeOfType<Arrow>();
        interactions.Fragments[1].ShouldBeOfType<Alt>();
        interactions.Fragments[2].ShouldBeOfType<Interactions>();
    }

    [TestMethod]
    public void EmptyInteractions_Should_HaveNoFragments()
    {
        // Arrange & Act
        var interactions = new Interactions();

        // Assert
        interactions.Fragments.ShouldBeEmpty();
    }
}
