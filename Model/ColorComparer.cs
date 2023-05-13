using System.Diagnostics.CodeAnalysis;

namespace ColorSelectorSample.Model;

/// <summary>
/// Provides a custom Color <see cref="IEqualityComparer{Color}"/>.
/// </summary>
/// <remarks>
/// The <see cref="Color"/> struct does perform a strict comparison of ARGB values
/// and colors that are close will often return true.
/// 
/// Use this comparer to provide a strict comparison.
/// </remarks>
public sealed class ColorComparer : EqualityComparer<Color>
{
    /// <summary>
    /// Gets a singleton instance of this class.
    /// </summary>
    public static readonly ColorComparer Comparer = new();

    private ColorComparer()
    { }

    /// <summary>
    /// Determines whether the <see cref="Color"/> objects are equal.
    /// </summary>
    /// <param name="x">The first <see cref="Color"/> to compare.</param>
    /// <param name="y">The second <see cref="Color"/> to compare.</param>
    /// <returns>true if the specified objects are equal; otherwise, false.</returns>
    public override bool Equals(Color x, Color y)
    {
        return x.Red == y.Red && x.Blue == y.Blue && x.Green == y.Green && x.Alpha == y.Alpha;
    }

    /// <summary>
    /// Returns a hash code for the specified object.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> for which a hash code is to be returned.</param>
    /// <returns>A hash code for the specified object.</returns>
    public override int GetHashCode([DisallowNull] Color color)
    {
        return color.GetHashCode();
    }
}
