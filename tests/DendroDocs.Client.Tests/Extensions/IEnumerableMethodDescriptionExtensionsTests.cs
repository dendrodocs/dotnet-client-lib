namespace DendroDocs.Extensions.Tests;

[TestClass]
public class IEnumerableMethodDescriptionExtensionsTests
{
    [TestMethod]
    public void ExtensionMethodShouldGuardAgainstNRE()
    {
        // Assign
        IEnumerable<MethodDescription> list = default!;

        // Act
        Action act = () => list.WithName("");

        // Assert
        act.ShouldThrow<ArgumentNullException>()
            .ParamName.ShouldBe("list");
    }

    [DataRow("Method1", 1, DisplayName = "When the method exists in the list, return the exact match")]
    [DataRow("Method3", 2, DisplayName = "When the method exists multiple times in the list, return all matches")]
    [DataRow("Method4", 0, DisplayName = "When the method does not exists in the list, return no matches")]
    [DataRow("method1", 0, DisplayName = "When the method exists with a different casing in the list, return no matches")]
    [TestMethod]
    public void ExpectTheFilterToBeAppliedCorrectlyOnTheList(string value, int expectation)
    {
        // Assign
        var list = new List<MethodDescription>()
        {
            new ("void", "Method1"),
            new ("void", "Method2"),
            new ("void", "Method3"),
            new ("void", "Method3")
        };

        // Act
        var result = list.WithName(value);

        // Assert
        result.Count.ShouldBe(expectation);
    }
}
