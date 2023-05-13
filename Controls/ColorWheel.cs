// Ignore Spelling: hsb

using ColorSelectorSample.Model;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ColorSelectorSample.Controls;

/// <summary>
/// Provides a color wheel selectable control.
/// </summary>
public sealed class ColorWheel : SKCanvasView
{
    #region Fields

#if WINDOWS
    const bool IsWindows = true;
#else
    const bool IsWindows = false;
#endif
    static readonly int MaxDiameter = Convert.ToInt32(Math.Sqrt(Int32.MaxValue)) / 4 - 4;
    SKRect _imageRect = new SKRect();

    SKBitmap _colorWheel;

    #endregion Fields

    public ColorWheel()
    {
        EnableTouchEvents = true;
        base.Touch += OnCanvasTouch;
    }

    #region Bindable Properties

    /// <summary>
    /// Gets or sets the color to use to draw the slider's track.
    /// </summary>
    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    /// <summary>
    /// Provides the<see cref="BindableProperty"/> for <see cref="Color"/>.
    /// </summary>
    public static readonly BindableProperty ColorProperty = BindableProperty.Create
    (
        nameof(Color),
        typeof(Color),
        typeof(Slider),
        Colors.White
    );

    #endregion Bindable Properties

    #region Event Handlers

    private void OnCanvasTouch(object sender, SKTouchEventArgs e)
    {
        if (IsWindows && !e.InContact)
        {
            return;
        }
        SKPoint touch = e.Location;
        if (e.MouseButton == SKMouseButton.Left && _imageRect.Contains(touch.X, touch.Y))
        {
            PickColor(touch);
        }
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        SKSurface surface = e.Surface;
        SKCanvas canvas = surface.Canvas;
        SKSize canvasSize = base.CanvasSize;
        if
        (
            _colorWheel == null
            ||
            _colorWheel.Width != canvasSize.Width
            ||
            _colorWheel.Height != canvasSize.Width
        )
        {
            _colorWheel?.Dispose();
            _colorWheel = CreateImage(canvasSize, out _imageRect);
        }
        canvas.DrawBitmap(_colorWheel, 0, 0);
    }

    #endregion Event Handlers

    void PickColor(SKPoint point)
    {
        int x = (int)point.X;
        int y = (int)point.Y;
        if (_colorWheel != null && _imageRect.Contains(x, y))
        {
            SKColor skColor = _colorWheel.GetPixel(x, y);
            Color = Color.FromRgba(skColor.Red, skColor.Green, skColor.Blue, skColor.Alpha);
        }
    }

    #region Color wheel bitmap creation

    static SKBitmap CreateImage(SKSize size, out SKRect _imageRect)
    {
        int width = (int)size.Width;
        int height = (int)size.Height;
        _imageRect = new SKRect(0, 0, width, height);

        int diameter = (int)Math.Min(width, height);
        if (diameter > MaxDiameter)
        {
            diameter = MaxDiameter;
        }
        int radius = diameter / 2;

        SKBitmap bitmap = new SKBitmap(diameter, diameter);
        SKColor[] pixels = new SKColor[diameter * diameter];

        int row = 0;
        for (int y = 0; y < diameter; y++)
        {
            for (int x = 0; x < diameter; x++)
            {
                double distance = Math.Sqrt(Math.Pow(radius - x, 2) + Math.Pow(radius - y, 2));
                double saturation = distance / radius;
                if (saturation >= 1)
                {
                    continue;
                }
                else
                {
                    double distanceOfX = x - radius;
                    double distanceOfY = y - radius;
                    double theta = Math.Atan2(distanceOfX, distanceOfY);
                    if (theta < 0)
                    {
                        theta += 2 * Math.PI;
                    }
                    double hue = theta / (Math.PI * 2) * 360.0;
                    HSBColor hsb = new HSBColor(255, hue, saturation, 1.0);
                    SKColor skColor = ToSkColor(hsb);
                    pixels[row + x] = skColor;
                }
            }
            row += diameter;
        }

        bitmap.Pixels = pixels;
        return bitmap;
    }

    static SKColor ToSkColor(HSBColor hsb)
    {
        Color color = hsb.ToColor();
        return new(
                    (byte)(color.Red * 255),
                    (byte)(color.Green * 255),
                    (byte)(color.Blue * 255),
                    (byte)(color.Alpha * 255)
                   );
    }

    #endregion Color wheel bitmap creation
}
