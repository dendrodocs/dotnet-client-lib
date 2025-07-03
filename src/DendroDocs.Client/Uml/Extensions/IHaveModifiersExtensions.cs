using DendroDocs.Extensions;
using PlantUml.Builder;

namespace DendroDocs.Uml.Extensions;

/// <summary>
/// Provides extension methods for objects implementing the <see cref="IHaveModifiers"/> interface, enabling conversion
/// of visibility modifiers to their PlantUML equivalents.
/// </summary>
/// <remarks>This class contains methods that simplify the process of mapping visibility modifiers from objects
/// implementing <see cref="IHaveModifiers"/> to corresponding UML visibility representations, such as public,
/// protected, private, and internal.</remarks>
public static class IHaveModifiersExtensions
{
    /// <summary>
    /// Converts the visibility modifiers of the specified object to a PlantUML visibility modifier.
    /// </summary>
    /// <param name="modifiers">An object implementing <see cref="IHaveModifiers"/> that provides access to visibility modifiers.</param>
    /// <returns>A <see cref="VisibilityModifier"/> value representing the UML visibility equivalent of the object's modifiers.
    /// Returns <see cref="VisibilityModifier.Public"/> for public visibility,  <see
    /// cref="VisibilityModifier.PackagePrivate"/> for internal visibility,  <see cref="VisibilityModifier.Protected"/>
    /// for protected visibility,  <see cref="VisibilityModifier.Private"/> for private visibility,  or <see
    /// cref="VisibilityModifier.None"/> if no visibility modifier is applicable.</returns>
    public static VisibilityModifier ToPlantUmlVisibility(this IHaveModifiers modifiers)
    {
        ArgumentNullException.ThrowIfNull(modifiers);

        if (modifiers.IsPublic())
        {
            return VisibilityModifier.Public;
        }

        if (modifiers.IsInternal())
        {
            return VisibilityModifier.PackagePrivate;
        }

        if (modifiers.IsProtected())
        {
            return VisibilityModifier.Protected;
        }

        if (modifiers.IsPrivate())
        {
            return VisibilityModifier.Private;
        }

        return VisibilityModifier.None;
    }
}
