using System.Diagnostics;

namespace DendroDocs.Uml.Fragments;

/// <summary>
/// Represents a group with 1 or more sections, like <c>alt</c>, <c>par</c>, <c>loop</c>, etc..
/// </summary>
[DebuggerDisplay("Alt")]
public class Alt : InteractionFragment
{
    private readonly List<AltSection> sections = [];

    /// <summary>
    /// Gets all sections.
    /// </summary>
    public IReadOnlyList<AltSection> Sections => this.sections;

    /// <summary>
    /// Add a sections to this alt.
    /// </summary>
    /// <param name="section">The section to add.</param>
    public void AddSection(AltSection section)
    {
        ArgumentNullException.ThrowIfNull(section);

        section.Parent = this;

        this.sections.Add(section);
    }
}
