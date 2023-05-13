using System.Reflection;

namespace ColorSelectorSample.Model;

/// <summary>
/// Provides a named <see cref="Color"/>
/// </summary>
public class NamedColor
{
    #region Static 

    static readonly Dictionary<Color, NamedColor> _fromColor = new(ColorComparer.Comparer);
    static readonly Dictionary<string, NamedColor> _fromName = new();

    static NamedColor()
    {
        List<NamedColor> all = new();
        foreach (FieldInfo info in typeof(Microsoft.Maui.Graphics.Colors).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            if (info.FieldType == typeof(Color))
            {
                object infoValue = info.GetValue(null);
                if (infoValue == null)
                {
                    continue;
                }
                NamedColor color = new NamedColor(info.Name, (Color)infoValue);
                _fromName.Add(color.Name, color);
                if (!_fromColor.ContainsKey(color.Color))
                {
                    // NOTE: some colors have duplicate names (e.g., Aqua and Cyan)
                    _fromColor.Add(color.Color, color);
                }
                all.Add(color);
            }
        }
        all.Sort(NamedColorComparer.Comparer);
        All = all;
    }

    #region Static Properties

    /// <summary>
    /// Gets all <see cref="NamedColor"/> instances.
    /// </summary>
    public static IList<NamedColor> All
    {
        get;
    }

    #endregion Static Properties

    #region Static Members

    /// <summary>
    /// Gets <see cref="NamedColor"/> for a given <see cref="Color"/>.
    /// </summary>
    public static NamedColor FromColor(Color color)
    {
        _fromColor.TryGetValue(color, out NamedColor namedColor);
        return namedColor;
    }

    /// <summary>
    /// Gets <see cref="NamedColor"/> for a given <paramref name="name"/>.
    /// </summary>
    public static NamedColor FromName(string name)
    {
        _fromName.TryGetValue(name, out NamedColor value);
        return value;
    }

    #endregion Static Members

    #endregion Static

    private NamedColor(string name, Color color)
    {
        Name = name;
        Color = color;
    }

    #region Properties

    /// <summary>
    /// Gets the color's Name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the color's value.
    /// </summary>
    public Color Color { get; private set; }

    #endregion Properties

    #region Equals

    /// <summary>
    /// Determines if the specified <paramref name="namedColor"/> is equal to this instance..
    /// </summary>
    /// <param name="namedColor">The <see cref="Color"/> to compare.</param>
    /// <returns>true if the specified <paramref name="namedColor"/> is equal to this instance;
    /// otherwise, false.</returns>
    /// <remarks>
    /// This method compares both the <see cref="Name"/> and <see cref="Color"/> properties for equality.
    /// </remarks>
    public bool Equals(NamedColor namedColor)
    {
        if (namedColor != null)
        {
            return Equals(namedColor.Color) && StringComparer.Ordinal.Compare(Name, namedColor.Name) == 0;
        }
        return false;
    }


    /// <summary>
    /// Determines if the specified <paramref name="color"/> is equal to this instance's <see cref="Color"/>.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to compare.</param>
    /// <returns>true if the specified <paramref name="color"/> is equal to this instance's <see cref="Color"/>; 
    /// otherwise, false.</returns>
    public bool Equals(Color color)
    {
        if (color != null)
        {
            return ColorComparer.Comparer.Equals(Color, color);
        }
        return false;
    }

    /// <summary>
    /// Determines if the specified object is equal to this instance.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns>
    /// true if <paramref name="obj"/> is a <see cref="Microsoft.Maui.Graphics.Color"/> that equals
    /// this instance's <see cref="Color"/>
    /// -or-
    /// <paramref name="obj"/> is a <see cref="NamedColor"/> equal to this instance;
    /// otherwise, false.
    /// </returns>
    public override bool Equals(object obj)
    {
        if (obj is NamedColor namedColor)
        {
            return Equals(namedColor);
        }
        else if (obj is Color color)
        {
            return Equals(color);
        }
        return false;
    }

    /// <summary>
    ///  Gets a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance.</returns>
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    #endregion Equals
}
