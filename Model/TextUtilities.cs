using Microsoft.Maui.Graphics.Skia;

namespace ColorSelectorSample.Model
{
    /// <summary>
    /// Provides utilities for measuring text.
    /// </summary>
    static public class TextUtilities
    {
        static readonly object _lock = new object();
        static IStringSizeService _stringSizeService;

        /// <summary>
        /// Gets an <see cref="IStringSizeService"/> to use to measure text.
        /// </summary>
        static public IStringSizeService StringSizeService
        {
            get
            {
                lock(_lock)
                {
                    _stringSizeService ??= new SkiaStringSizeService();
                    return _stringSizeService;
                }
            }
        }

        /// <summary>
        /// Gets the size of the bounding rectangle needed to render the specified <paramref name="text"/>.
        /// </summary>
        /// <param name="text">The text to render.</param>
        /// <param name="fontFamily">The string font family name of the font to use to render the text.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontAttributes">The <see cref="FontAttributes"/>.</param>
        /// <returns></returns>
        public static Size Measure(string text, string fontFamily, double fontSize, FontAttributes fontAttributes)
        {
            IFont font = GetFont(fontFamily, fontAttributes);
            SizeF size = StringSizeService.GetStringSize(text, font, (float)fontSize);

            return new Size(size.Width, size.Height);
        }

        /// <summary>
        /// Gets an <see cref="IFont"/> to use with <see cref="IStringSizeService.GetStringSize(string, IFont, float)"/>
        /// </summary>
        /// <returns>
        /// An <see cref="IFont"/> for the specified <paramref name="fontFamily"/> and <paramref name="fontAttributes"/>.
        /// </returns>
        static public IFont GetFont(string fontFamily, FontAttributes fontAttributes)
        {
            FontStyleType fontStyle = fontAttributes == FontAttributes.Italic
                          ? FontStyleType.Italic
                          : FontStyleType.Normal;

            var fontWeight = fontAttributes switch
            {
                FontAttributes.Bold => FontWeights.Bold,
                _ => FontWeights.Normal,
            };
            ;
            return new Microsoft.Maui.Graphics.Font(fontFamily, fontWeight, fontStyle);
        }
    }
}
