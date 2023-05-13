namespace ColorSelectorSample.Model;

/// <summary>
/// Provides an <see cref="IComparer{NamedColor}"/> for comparing two <see cref="NamedColor"/>
/// instances by <see cref="NamedColor.Name"/>.
/// </summary>
public class NamedColorComparer : IComparer<NamedColor>
{
    /// <summary>
    /// Gets the singleton instance of this class.
    /// </summary>
    public static readonly NamedColorComparer Comparer = new();

    private NamedColorComparer()
    {
    }

    /// <summary>
    /// Compares two <see cref="NamedColor"/> instances.
    /// </summary>
    /// <param name="x">The first <see cref="NamedColor"/> to compare.</param>
    /// <param name="y">The second <see cref="NamedColor"/> to compare.</param>
    /// <returns>
    /// <list type="table">
    ///     <listheader>
    ///         <term>Value</term>
    ///         <description>Description</description>
    ///     </listheader>
    ///     <item>
    ///         <term>Less than zero</term>
    ///         <description><paramref name="x"/> is less than <paramref name="y"/>.</description>
    ///     </item>
    ///     <item>
    ///         <term>Zero</term>
    ///         <description>This <paramref name="x"/> is equal to <paramref name="y"/>.</description>
    ///     </item>
    ///     <item>
    ///         <term>Greater than zero.</term>
    ///         <description><paramref name="x"/> is greater than <paramref name="y"/>.</description>
    ///     </item>
    /// </list>
    /// </returns>		
    public int Compare(NamedColor x, NamedColor y)
    {
        int result;
        if (x == null && y == null)
        {
            result = 0;
        }
        else if (x == null)
        {
            result = -1;
        }
        else if (y == null)
        {
            result = 1;
        }
        else
        {
            result = StringComparer.CurrentCulture.Compare(x.Name, y.Name);
        }
        return result;
    }
}
