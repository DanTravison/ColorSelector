using ColorSelectorSample.Model;

namespace ColorSelectorSample;

public partial class MainPage : ContentPage
{
    readonly ColorViewModel _model;

    public MainPage()
    {
        BindingContext = _model = new ColorViewModel();
        _model.PropertyChanged += OnModelPropertyChanged;
        InitializeComponent();
    }

    private void OnModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (NamedColors != null && object.ReferenceEquals(e, ColorViewModel.ColorChangedEventArgs))
        {
            NamedColor color = NamedColor.FromColor(_model.Color);
            if (color != null)
            {
                NamedColors.ScrollTo(color, ScrollToPosition.MakeVisible);
            }
        }
    }
}

