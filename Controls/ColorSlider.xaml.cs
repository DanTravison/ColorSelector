namespace ColorSelectorSample.Controls;

public partial class ColorSlider : ContentView
{
    public ColorSlider()
    {
        InitializeComponent();
    }

    #region Value

    /// <summary>
    /// Gets or sets the value for the <see cref="ColorSlider"/>.
    /// </summary>
    public byte Value
    {
        get => (byte)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Provides the<see cref="BindableProperty"/> for <see cref="Value"/>.
    /// </summary>
    public static readonly BindableProperty ValueProperty = BindableProperty.Create
    (
        nameof(Value),
        typeof(byte),
        typeof(ColorSlider),
        null,
        BindingMode.TwoWay
    );

    #endregion Value

    #region Text

    /// <summary>
    /// Gets or sets the text for the <see cref="ColorSlider"/>.
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Provides the<see cref="BindableProperty"/> for <see cref="Text"/>.
    /// </summary>
    public static readonly BindableProperty TextProperty = BindableProperty.Create
    (
        nameof(Text),
        typeof(string),
        typeof(ColorSlider),
        null,
        BindingMode.OneWay
    );

    #endregion Text

    private void OnDecrementTapped(object sender, TappedEventArgs e)
    {
        byte value = Value;
        if (value > 0)
        {
            Value = --value;
        }
    }

    private void OnIncrementTapped(object sender, TappedEventArgs e)
    {
        byte value = Value;
        if (value < 255)
        {
            Value = ++value;
        }
    }
}