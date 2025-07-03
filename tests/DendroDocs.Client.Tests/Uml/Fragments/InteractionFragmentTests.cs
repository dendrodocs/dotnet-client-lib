namespace DendroDocs.Uml.Fragments.Tests;

[TestClass]
public class InteractionFragmentTests
{
    [TestMethod]
    public void AddFragment_ValidFragment_Should_AddToFragments()
    {
        // Arrange
        var parent = new TestInteractionFragment();
        var child = new TestInteractionFragment();

        // Act
        parent.AddFragment(child);

        // Assert
        parent.Fragments.ShouldHaveSingleItem();
        parent.Fragments[0].ShouldBe(child);
        child.Parent.ShouldBe(parent);
    }

    [TestMethod]
    public void AddFragment_NullFragment_Should_Throw()
    {
        // Arrange
        var parent = new TestInteractionFragment();

        // Act
        Action action = () => parent.AddFragment(null!);

        // Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [TestMethod]
    public void AddFragments_ValidFragments_Should_AddAllToFragments()
    {
        // Arrange
        var parent = new TestInteractionFragment();
        var child1 = new TestInteractionFragment();
        var child2 = new TestInteractionFragment();
        var children = new[] { child1, child2 };

        // Act
        parent.AddFragments(children);

        // Assert
        parent.Fragments.Count.ShouldBe(2);
        parent.Fragments[0].ShouldBe(child1);
        parent.Fragments[1].ShouldBe(child2);
        child1.Parent.ShouldBe(parent);
        child2.Parent.ShouldBe(parent);
    }

    [TestMethod]
    public void AddFragments_NullFragments_Should_Throw()
    {
        // Arrange
        var parent = new TestInteractionFragment();

        // Act
        Action action = () => parent.AddFragments(null!);

        // Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [TestMethod]
    public void AddFragments_EmptyCollection_Should_NotThrow()
    {
        // Arrange
        var parent = new TestInteractionFragment();

        // Act
        Action action = () => parent.AddFragments(Array.Empty<InteractionFragment>());

        // Assert
        action.ShouldNotThrow();
        parent.Fragments.ShouldBeEmpty();
    }

    [TestMethod]
    public void Parent_SetAndGet_Should_WorkCorrectly()
    {
        // Arrange
        var parent = new TestInteractionFragment();
        var child = new TestInteractionFragment();

        // Act
        child.Parent = parent;

        // Assert
        child.Parent.ShouldBe(parent);
    }

    [TestMethod]
    public void Fragments_InitialState_Should_BeEmpty()
    {
        // Arrange & Act
        var fragment = new TestInteractionFragment();

        // Assert
        fragment.Fragments.ShouldNotBeNull();
        fragment.Fragments.ShouldBeEmpty();
    }

    [TestMethod]
    public void AddFragment_MultipleFragments_Should_MaintainOrder()
    {
        // Arrange
        var parent = new TestInteractionFragment();
        var child1 = new TestInteractionFragment();
        var child2 = new TestInteractionFragment();
        var child3 = new TestInteractionFragment();

        // Act
        parent.AddFragment(child1);
        parent.AddFragment(child2);
        parent.AddFragment(child3);

        // Assert
        parent.Fragments.Count.ShouldBe(3);
        parent.Fragments[0].ShouldBe(child1);
        parent.Fragments[1].ShouldBe(child2);
        parent.Fragments[2].ShouldBe(child3);
    }

    [TestMethod]
    public void AddFragment_ReparentingFragment_Should_UpdateParent()
    {
        // Arrange
        var parent1 = new TestInteractionFragment();
        var parent2 = new TestInteractionFragment();
        var child = new TestInteractionFragment();

        parent1.AddFragment(child);

        // Act
        parent2.AddFragment(child);

        // Assert
        child.Parent.ShouldBe(parent2);
        parent1.Fragments.ShouldHaveSingleItem();
        parent2.Fragments.ShouldHaveSingleItem();
    }

    // Helper class for testing the abstract InteractionFragment
    private class TestInteractionFragment : InteractionFragment
    {
    }
}