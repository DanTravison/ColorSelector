using ColorSelectorSample.Model;

namespace ColorSelectorSample.Controls;

/// <summary>
/// Provides a <see cref="Microsoft.Maui.Controls.Label"/> with a minimum desired text width;
/// </summary>
public class Label : Microsoft.Maui.Controls.Label
{
    Size _measuredSize;

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    public Label()
    {
    }

    /// <summary>
    /// Overrides <see cref="BindableObject.OnPropertyChanged(string)"/> to force a remeasure.
    /// </summary>
    /// <param name="propertyName"></param>
    protected override void OnPropertyChanged(string propertyName = null)
    {
        if (StringComparer.Ordinal.Compare(propertyName, nameof(Text)) == 0)
        {
            // Recalculate _measuredSize when Label.Text is set outside of MeasureOverride.
            _measuredSize = Size.Zero;
            InvalidateMeasure();
        }
        base.OnPropertyChanged(propertyName);
    }

    /// <summary>
    /// Overrides <see cref="VisualElement.MeasureOverride(double, double)"/> to set the <see cref="DesiredWidth"/>.
    /// </summary>
    /// <param name="widthConstraint">The width constraint.</param>
    /// <param name="heightConstraint">The height constraint.</param>
    /// <returns>The desired <see cref="Size"/>.</returns>
    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        Size size = base.MeasureOverride(widthConstraint, heightConstraint);
        if (DesiredWidth > 0)
        {
            /*
                To determine the desired width, set the text to a repeating string pattern,
                ask the base class to measure, then restore the original text.
                To ensure we don't remeasure 
            */
            if (_measuredSize == Size.Zero)
            {
                // create a string pattern to approximate a size for a string
                // of length DesiredWidth
                string pattern = new string('W', DesiredWidth);
                // Measure the pattern string and save it.
                _measuredSize = TextUtilities.Measure(pattern, FontFamily, FontSize, FontAttributes);
            }
            if (size.Width < _measuredSize.Width)
            {
                size.Width = _measuredSize.Width;
            }
        }
        return size;
    }

    /// <summary>
    /// Gets or sets the desired text width.
    /// </summary>
    /// <value>A zero-based value indicating the desired minimum width, in characters for the 
    /// label.
    /// </value>
    public int DesiredWidth
    {
        get => (int)GetValue(DesiredWidthProperty);
        set => SetValue(DesiredWidthProperty, value);
    }

    /// <summary>
    /// Provides the<see cref="BindableProperty"/> for <see cref="DesiredWidth"/>.
    /// </summary>
    public static readonly BindableProperty DesiredWidthProperty = BindableProperty.Create
    (
        nameof(DesiredWidth),
        typeof(int),
        typeof(Label),
        0,
        validateValue: (bindableObject, newValue) =>
        {
            if (bindableObject is Microsoft.Maui.Controls.Label label)
            {
                if ((int)newValue >= 0)
                {
                    return true;
                }
            }
            return false;
        },
        propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            if (bindableObject is Label label)
            {
                label._measuredSize = Size.Zero;
                label.InvalidateMeasure();
            }
        }
    );
}
