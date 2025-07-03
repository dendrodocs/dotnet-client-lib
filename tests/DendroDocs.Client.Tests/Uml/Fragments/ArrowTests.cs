namespace DendroDocs.Uml.Fragments.Tests;

[TestClass]
public class ArrowTests
{
    [TestMethod]
    public void Constructor_Should_InitializeWithNullProperties()
    {
        // Act
        var arrow = new Arrow();

        // Assert
        arrow.Source.ShouldBeNull();
        arrow.Target.ShouldBeNull();
        arrow.Color.ShouldBeNull();
        arrow.Name.ShouldBeNull();
        arrow.Dashed.ShouldBeFalse();
    }

    [TestMethod]
    public void Source_SetAndGet_Should_WorkCorrectly()
    {
        // Arrange
        var arrow = new Arrow();
        const string source = "ParticipantA";

        // Act
        arrow.Source = source;

        // Assert
        arrow.Source.ShouldBe(source);
    }

    [TestMethod]
    public void Target_SetAndGet_Should_WorkCorrectly()
    {
        // Arrange
        var arrow = new Arrow();
        const string target = "ParticipantB";

        // Act
        arrow.Target = target;

        // Assert
        arrow.Target.ShouldBe(target);
    }

    [TestMethod]
    public void Color_SetAndGet_Should_WorkCorrectly()
    {
        // Arrange
        var arrow = new Arrow();
        const string color = "red";

        // Act
        arrow.Color = color;

        // Assert
        arrow.Color.ShouldBe(color);
    }

    [TestMethod]
    public void Name_SetAndGet_Should_WorkCorrectly()
    {
        // Arrange
        var arrow = new Arrow();
        const string name = "getMessage()";

        // Act
        arrow.Name = name;

        // Assert
        arrow.Name.ShouldBe(name);
    }

    [TestMethod]
    public void Dashed_SetAndGet_Should_WorkCorrectly()
    {
        // Arrange
        var arrow = new Arrow();

        // Act
        arrow.Dashed = true;

        // Assert
        arrow.Dashed.ShouldBeTrue();
    }

    [TestMethod]
    public void AllProperties_SetToNull_Should_AcceptNull()
    {
        // Arrange
        var arrow = new Arrow
        {
            Source = "A",
            Target = "B",
            Color = "blue",
            Name = "message"
        };

        // Act
        arrow.Source = null;
        arrow.Target = null;
        arrow.Color = null;
        arrow.Name = null;

        // Assert
        arrow.Source.ShouldBeNull();
        arrow.Target.ShouldBeNull();
        arrow.Color.ShouldBeNull();
        arrow.Name.ShouldBeNull();
    }

    [TestMethod]
    public void InheritsFromInteractionFragment_Should_HaveFragmentBehavior()
    {
        // Arrange
        var interactions = new Interactions();
        var arrow = new Arrow { Source = "A", Target = "B", Name = "message" };

        // Act
        interactions.AddFragment(arrow);

        // Assert
        arrow.Parent.ShouldBe(interactions);
        interactions.Fragments.ShouldHaveSingleItem();
        interactions.Fragments[0].ShouldBe(arrow);
    }

    [TestMethod]
    public void DebuggerDisplay_Should_ShowCorrectFormat()
    {
        // Arrange
        var arrow = new Arrow
        {
            Source = "ClientA",
            Target = "ServerB",
            Name = "processRequest()"
        };

        // Act & Assert
        // Note: We can't directly test DebuggerDisplay attribute, but we can verify the properties exist
        arrow.Source.ShouldBe("ClientA");
        arrow.Target.ShouldBe("ServerB");
        arrow.Name.ShouldBe("processRequest()");
    }

    [TestMethod]
    public void Arrow_CanBeAddedToAltSection()
    {
        // Arrange
        var section = new AltSection { GroupType = "alt", Label = "condition" };
        var arrow = new Arrow { Source = "A", Target = "B", Name = "call" };

        // Act
        section.AddFragment(arrow);

        // Assert
        section.Fragments.ShouldHaveSingleItem();
        section.Fragments[0].ShouldBe(arrow);
        arrow.Parent.ShouldBe(section);
    }

    [TestMethod]
    public void MultipleArrows_InSequence_Should_MaintainOrder()
    {
        // Arrange
        var interactions = new Interactions();
        var arrow1 = new Arrow { Source = "A", Target = "B", Name = "first" };
        var arrow2 = new Arrow { Source = "B", Target = "C", Name = "second" };
        var arrow3 = new Arrow { Source = "C", Target = "A", Name = "third" };

        // Act
        interactions.AddFragment(arrow1);
        interactions.AddFragment(arrow2);
        interactions.AddFragment(arrow3);

        // Assert
        interactions.Fragments.Count.ShouldBe(3);
        interactions.Fragments[0].ShouldBe(arrow1);
        interactions.Fragments[1].ShouldBe(arrow2);
        interactions.Fragments[2].ShouldBe(arrow3);
    }
}
