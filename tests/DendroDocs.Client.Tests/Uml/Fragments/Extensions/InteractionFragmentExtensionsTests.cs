namespace DendroDocs.Uml.Fragments.Extensions.Tests;

[TestClass]
public class InteractionFragmentExtensionsTests
{
    [TestMethod]
    public void Descendants_NullNodes_Should_Throw()
    {
        // Arrange
        IEnumerable<InteractionFragment> nodes = null!;

        // Act
        Action action = () => nodes.Descendants<Arrow>();

        // Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [TestMethod]
    public void Descendants_EmptyCollection_Should_ReturnEmpty()
    {
        // Arrange
        var nodes = Array.Empty<InteractionFragment>();

        // Act
        var result = nodes.Descendants<Arrow>();

        // Assert
        result.ShouldBeEmpty();
    }

    [TestMethod]
    public void Descendants_DirectArrowMatches_Should_ReturnArrows()
    {
        // Arrange
        var arrow1 = new Arrow { Source = "A", Target = "B" };
        var arrow2 = new Arrow { Source = "B", Target = "C" };
        var alt = new Alt();
        var nodes = new InteractionFragment[] { arrow1, alt, arrow2 };

        // Act
        var result = nodes.Descendants<Arrow>();

        // Assert
        result.Count.ShouldBe(2);
        result.ShouldContain(arrow1);
        result.ShouldContain(arrow2);
    }

    [TestMethod]
    public void Descendants_ArrowsInAltSections_Should_ReturnArrows()
    {
        // Arrange
        var alt = new Alt();
        var section1 = new AltSection { GroupType = "alt" };
        var section2 = new AltSection { GroupType = "else" };
        var arrow1 = new Arrow { Source = "A", Target = "B" };
        var arrow2 = new Arrow { Source = "B", Target = "C" };
        
        section1.AddFragment(arrow1);
        section2.AddFragment(arrow2);
        alt.AddSection(section1);
        alt.AddSection(section2);

        var nodes = new InteractionFragment[] { alt };

        // Act
        var result = nodes.Descendants<Arrow>();

        // Assert
        result.Count.ShouldBe(2);
        result.ShouldContain(arrow1);
        result.ShouldContain(arrow2);
    }

    [TestMethod]
    public void Descendants_NestedAltStructures_Should_ReturnAllMatches()
    {
        // Arrange
        var outerAlt = new Alt();
        var outerSection = new AltSection { GroupType = "alt" };
        var innerAlt = new Alt();
        var innerSection = new AltSection { GroupType = "opt" };
        var arrow = new Arrow { Source = "A", Target = "B" };

        innerSection.AddFragment(arrow);
        innerAlt.AddSection(innerSection);
        outerSection.AddFragment(innerAlt);
        outerAlt.AddSection(outerSection);

        var nodes = new InteractionFragment[] { outerAlt };

        // Act
        var result = nodes.Descendants<Arrow>();

        // Assert
        result.ShouldHaveSingleItem();
        result[0].ShouldBe(arrow);
    }


    [TestMethod]
    public void Descendants_NonAltFragment_Should_NotTraverseChildren()
    {
        // Arrange
        var interactions = new Interactions();
        var arrow = new Arrow { Source = "A", Target = "B" };
        interactions.AddFragment(arrow);

        var nodes = new InteractionFragment[] { interactions };

        // Act
        var result = nodes.Descendants<Arrow>();

        // Assert
        result.ShouldBeEmpty(); // Interactions is not handled in the switch, so children are not traversed
    }

    [TestMethod]
    public void Ancestors_NullFragment_Should_Throw()
    {
        // Arrange
        InteractionFragment fragment = null!;

        // Act
        Action action = () => fragment.Ancestors();

        // Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [TestMethod]
    public void Ancestors_FragmentWithoutParent_Should_ReturnEmpty()
    {
        // Arrange
        var arrow = new Arrow { Source = "A", Target = "B" };

        // Act
        var result = arrow.Ancestors();

        // Assert
        result.ShouldBeEmpty();
    }

    [TestMethod]
    public void Ancestors_FragmentWithSingleParent_Should_ReturnParent()
    {
        // Arrange
        var interactions = new Interactions();
        var arrow = new Arrow { Source = "A", Target = "B" };
        interactions.AddFragment(arrow);

        // Act
        var result = arrow.Ancestors();

        // Assert
        result.ShouldHaveSingleItem();
        result[0].ShouldBe(interactions);
    }

    [TestMethod]
    public void Ancestors_FragmentWithMultipleParents_Should_ReturnAllAncestors()
    {
        // Arrange
        var interactions = new Interactions();
        var alt = new Alt();
        var section = new AltSection { GroupType = "alt" };
        var arrow = new Arrow { Source = "A", Target = "B" };

        section.AddFragment(arrow);
        alt.AddSection(section);
        interactions.AddFragment(alt);

        // Act
        var result = arrow.Ancestors();

        // Assert
        result.Count.ShouldBe(3);
        result[0].ShouldBe(section);
        result[1].ShouldBe(alt);
        result[2].ShouldBe(interactions);
    }

    [TestMethod]
    public void StatementsBeforeSelf_NullFragment_Should_Throw()
    {
        // Arrange
        InteractionFragment fragment = null!;

        // Act
        Action action = () => fragment.StatementsBeforeSelf();

        // Assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [TestMethod]
    public void StatementsBeforeSelf_FragmentWithoutParent_Should_ReturnEmpty()
    {
        // Arrange
        var arrow = new Arrow { Source = "A", Target = "B" };

        // Act
        var result = arrow.StatementsBeforeSelf();

        // Assert
        result.ShouldBeEmpty();
    }

    [TestMethod]
    public void StatementsBeforeSelf_FirstFragment_Should_ReturnEmpty()
    {
        // Arrange
        var interactions = new Interactions();
        var arrow1 = new Arrow { Source = "A", Target = "B" };
        var arrow2 = new Arrow { Source = "B", Target = "C" };
        interactions.AddFragment(arrow1);
        interactions.AddFragment(arrow2);

        // Act
        var result = arrow1.StatementsBeforeSelf();

        // Assert
        result.ShouldBeEmpty();
    }

    [TestMethod]
    public void StatementsBeforeSelf_SecondFragment_Should_ReturnFirst()
    {
        // Arrange
        var interactions = new Interactions();
        var arrow1 = new Arrow { Source = "A", Target = "B" };
        var arrow2 = new Arrow { Source = "B", Target = "C" };
        interactions.AddFragment(arrow1);
        interactions.AddFragment(arrow2);

        // Act
        var result = arrow2.StatementsBeforeSelf();

        // Assert
        result.ShouldHaveSingleItem();
        result[0].ShouldBe(arrow1);
    }

    [TestMethod]
    public void StatementsBeforeSelf_LastFragment_Should_ReturnAllPrevious()
    {
        // Arrange
        var interactions = new Interactions();
        var arrow1 = new Arrow { Source = "A", Target = "B" };
        var arrow2 = new Arrow { Source = "B", Target = "C" };
        var arrow3 = new Arrow { Source = "C", Target = "D" };
        interactions.AddFragment(arrow1);
        interactions.AddFragment(arrow2);
        interactions.AddFragment(arrow3);

        // Act
        var result = arrow3.StatementsBeforeSelf();

        // Assert
        result.Count.ShouldBe(2);
        result[0].ShouldBe(arrow1);
        result[1].ShouldBe(arrow2);
    }

    [TestMethod]
    public void Descendants_MixedFragmentTypes_Should_FilterCorrectly()
    {
        // Arrange
        var interactions = new Interactions();
        var alt = new Alt();
        var section = new AltSection { GroupType = "alt" };
        var arrow1 = new Arrow { Source = "A", Target = "B" };
        var arrow2 = new Arrow { Source = "B", Target = "C" };

        section.AddFragment(arrow1);
        alt.AddSection(section);

        var nodes = new InteractionFragment[] { alt, arrow2 };

        // Act
        var arrows = nodes.Descendants<Arrow>();
        var alts = nodes.Descendants<Alt>();

        // Assert
        arrows.Count.ShouldBe(2);
        arrows.ShouldContain(arrow1);
        arrows.ShouldContain(arrow2);
        
        alts.ShouldHaveSingleItem();
        alts[0].ShouldBe(alt);
    }
}
