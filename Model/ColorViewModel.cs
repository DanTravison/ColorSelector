using System.ComponentModel;

namespace ColorSelectorSample.Model
{
    /// <summary>
    /// Provides a color selector view model.
    /// </summary>
    public sealed class ColorViewModel : INotifyPropertyChanged
    {
        static readonly Color DefaultColor = Microsoft.Maui.Graphics.Colors.Black;
        Color _color = DefaultColor;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ColorViewModel()
        {
            Colors = new SelectionList<NamedColor>(NamedColor.All);
            Colors.PropertyChanged += OnColorsPropertyChanged;
        }

        /// <summary>
        /// Occurs when a property on this instance changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        /// <summary>
        /// Gets the <see cref="NamedColor"/> list.
        /// </summary>
        public SelectionList<NamedColor> Colors
        {
            get;
        }

        /// <summary>
        /// Gets the selected <see cref="Microsoft.Maui.Graphics.Color"/>
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                if (!ColorComparer.Comparer.Equals(_color, value))
                {
                    Color oldColor = _color;
                    _color = value;
                    NamedColor namedColor = NamedColor.FromColor(_color);
                    // NOTE: Avoid changing the selected item unnecessarily
                    if
                    (
                        namedColor == null
                        ||
                        Colors.SelectedItem == null
                        ||
                        !Colors.SelectedItem.Equals(namedColor.Color)
                    )
                    {
                        Colors.SelectedItem = namedColor;
                    }
                    NotifyPropertyChanged(ColorChangedEventArgs);
                    NotifyColorComponentChanges(oldColor);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Color.Red"/> component of the <see cref="Color"/>.
        /// </summary>
        public byte Red
        {
            get => (byte)(_color.Red * 255f);
            set
            {
                float red = ToFloat(value);
                if (red != _color.Red)
                {
                    Color = new Color(red, _color.Green, _color.Blue, _color.Alpha);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Color.Green"/> component of the <see cref="Color"/>.
        /// </summary>
        public byte Green
        {
            get => (byte)(_color.Green * 255f);
            set
            {
                float green = ToFloat(value);
                if (green != _color.Green)
                {
                    Color = new Color(_color.Red, green, _color.Blue, _color.Alpha);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Color.Blue"/> component of the <see cref="Color"/>.
        /// </summary>
        public byte Blue
        {
            get => (byte)(_color.Blue * 255f);
            set
            {
                float blue = ToFloat(value);
                if (blue != _color.Blue)
                {
                    Color = new Color(_color.Red, _color.Green, blue, _color.Alpha);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Color.Alpha"/> component of the <see cref="Color"/>
        /// </summary>
        public byte Alpha
        {
            get => (byte)(_color.Alpha * 255f);
            set
            {
                float alpha = ToFloat(value);
                if (alpha != _color.Alpha)
                {
                    Color = new Color(_color.Red, _color.Green, _color.Blue, alpha);
                }
            }
        }

        #endregion Properties

        #region Methods

        private void OnColorsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (object.ReferenceEquals(e, SelectionList<NamedColor>.SelectedItemChangedEventArgs))
            {
                if (Colors.SelectedItem == null)
                {
                }
                else if (!Colors.SelectedItem.Equals(_color))
                {
                    Color = Colors.SelectedItem.Color;
                }
            }
        }

        void NotifyColorComponentChanges(Color oldColor)
        {
            if (oldColor.Red != _color.Red)
            {
                NotifyPropertyChanged(RedChangedEventArgs);
            }
            if (oldColor.Blue != _color.Blue)
            {
                NotifyPropertyChanged(BlueChangedEventArgs);
            }
            if (oldColor.Green != _color.Green)
            {
                NotifyPropertyChanged(GreenChangedEventArgs);
            }
            if (oldColor.Alpha != _color.Alpha)
            {
                NotifyPropertyChanged(AlphaChangedEventArgs);
            }
        }

        static float ToFloat(byte value)
        {
            return (float)value / 255f;
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> containing the event details.</param>
        void NotifyPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        #endregion Methods

        #region Cached PropertyChangedEventArgs

        /// <summary>
        /// Provides <see cref="PropertyChangedEventArgs"/> passed to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event when <see cref="Red"/> changes.
        /// </summary>
        static internal readonly PropertyChangedEventArgs RedChangedEventArgs = new PropertyChangedEventArgs(nameof(Red));
        /// <summary>
        /// Provides <see cref="PropertyChangedEventArgs"/> passed to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event when <see cref="Green"/> changes.
        /// </summary>
        static internal readonly PropertyChangedEventArgs GreenChangedEventArgs = new PropertyChangedEventArgs(nameof(Green));
        /// <summary>
        /// Provides <see cref="PropertyChangedEventArgs"/> passed to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event when <see cref="Blue"/> changes.
        /// </summary>
        static internal readonly PropertyChangedEventArgs BlueChangedEventArgs = new PropertyChangedEventArgs(nameof(Blue));
        /// <summary>
        /// Provides <see cref="PropertyChangedEventArgs"/> passed to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event when <see cref="Alpha"/> changes.
        /// </summary>
        static internal readonly PropertyChangedEventArgs AlphaChangedEventArgs = new PropertyChangedEventArgs(nameof(Blue));
        /// <summary>
        /// Provides <see cref="PropertyChangedEventArgs"/> passed to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event when <see cref="Color"/> changes.
        /// </summary>
        static internal readonly PropertyChangedEventArgs ColorChangedEventArgs = new PropertyChangedEventArgs(nameof(Color));

        #endregion Cached PropertyChangedEventArgs
    }
}
