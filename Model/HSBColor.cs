namespace ColorSelectorSample.Model;

/// <summary>
/// Provides a Hue-Saturation-Brightness color space value.  
/// </summary>
public readonly struct HSBColor
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    /// <param name="color">The color to use to initialize this instance.</param>
    public HSBColor(Color color)
    {
        double red = color.Red;
        double green = color.Green;
        double blue = color.Blue;
        double hue;

        double min = Min(red, green, blue);
        double brightness = Max(red, green, blue);
        double chroma = brightness - min;
        double saturation = chroma == 0 ? 0 : chroma / brightness;

        if (chroma == 0)
        {
            hue = 0;
        }
        else if (brightness == red)
        {
            hue = ((green - blue) / chroma + 6) % 6;
        }
        else if (brightness == green)
        {
            hue = 2 + (blue - red) / chroma;
        }
        else
        {
            hue = 4 + (red - green) / chroma;
        }
        hue *= 60;
        if (hue < 0)
        {
            hue += 360;
        }
        if (double.IsNaN(hue))
        {
            hue = 0;
        }

        Hue = hue;
        Saturation = saturation;
        Brightness = brightness;
        Alpha = color.Alpha;
    }

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    /// <param name="alpha">The color's alpha channel.</param>
    /// <param name="hue">The <see cref="Hue"/> of the color.</param>
    /// <param name="saturation">The <see cref="Saturation"/> of the color.</param>
    /// <param name="brightness">The <see cref="Brightness"/> of the color.</param>
    public HSBColor(float alpha, double hue, double saturation, double brightness)
    {
        Hue = hue;
        Saturation = saturation;
        Brightness = brightness;
        Alpha = alpha;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// The hue, in degrees, of the color.
    /// </summary>
    /// <remarks>
    /// The hue is measured in degrees, ranging from 0.0 through 360.0, in HSB color space.
    /// </remarks>
    public double Hue
    {
        get;
    }

    /// <summary>
    /// Gets the saturation of the color.
    /// </summary>
    /// <remarks>
    ///  The saturation ranges from 0.0 through 1.0.
    /// </remarks>
    public double Saturation
    {
        get;
    }

    /// <summary>
    /// Gets the brightness of the color.
    /// </summary>
    /// <remarks>
    /// The brightness ranges from 0.0 through 1.0.
    /// </remarks>
    public double Brightness
    {
        get;
    }

    /// <summary>
    /// Gets or sets the alpha channel.
    /// </summary>
    /// <value>
    /// A byte value from zero to 1 where zero indicates the color is completely
    /// transparent and 1 indicates the color is completely opaque.
    /// </value>
    public float Alpha
    {
        get;
    }

    #endregion Properties

    #region Conversion

    static double Min(double a, double b, double c)
    {
        double result = a;
        if (b < result)
        {
            result = b;
        }
        if (c < result)
        {
            result = c;
        }
        return result;
    }

    static double Max(double a, double b, double c)
    {
        double result = a;
        if (b > result)
        {
            result = b;
        }
        if (c > result)
        {
            result = c;
        }
        return result;
    }

    /// <summary>
    /// Gets the <see cref="Color"/> to an <see cref="HSBColor"/>.
    /// </summary>
    /// <returns>The <see cref="Color"/> for the specified <see cref="HSBColor"/>.</returns>
    public Color ToColor()
    {
        double chroma = Brightness * Saturation;
        double hue = Hue / 60;
        double x = chroma * (1 - Math.Abs(hue % 2 - 1));
        double m = Brightness - chroma;

        double red = 0, green = 0, blue = 0;
        if (hue < 1)
        {
            red = chroma;
            green = x;
        }
        else if (hue < 2)
        {
            green = chroma;
            red = x;
        }
        else if (hue < 3)
        {
            green = chroma;
            blue = x;
        }
        else if (hue < 4)
        {
            blue = chroma;
            green = x;
        }
        else if (hue < 5)
        {
            blue = chroma;
            red = x;
        }
        else
        {
            red = chroma;
            blue = x;
        }

        float r = (float)(red + m);
        float g = (float)(green + m);
        float b = (float)(blue + m);
        return Color.FromRgba(r, g, b, Alpha);
    }


    #endregion Conversion
}
