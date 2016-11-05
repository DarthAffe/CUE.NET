using System;
using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using Example_Ambilight_full.TakeAsIs;
using Example_Ambilight_full.TakeAsIs.Model;
using Example_Ambilight_full.TakeAsIs.ScreenCapturing;

namespace Example_Ambilight_full
{
    public class AmbilightExtendBrush : AbstractAmbilightBrush
    {
        #region Constructors

        public AmbilightExtendBrush(IScreenCapture screenCapture, AmbilightSettings settings)
            : base(screenCapture, settings)
        { }

        #endregion

        #region Methods

        protected override CorsairColor GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
        {
            //TODO DarthAffe 05.11.2016: The Key-Rectangle is missing in the render-target, I'll fix that with a future version of CUE.NET. Until then we consider the size of each key to be 10x10mm.
            float keyWidth = 10;

            int widthPixels = Math.Max(1, (int)(KeyWidthProportion * keyWidth));

            int r = 0;
            int g = 0;
            int b = 0;
            int counter = 0;

            int widthOffset = Settings.FlipMode.HasFlag(FlipMode.Horizontal)
                ? (int)((SourceWidth - OffsetRight) - (KeyWidthProportion * (renderTarget.Point.X + (keyWidth / 2f))))
                : (int)(OffsetLeft + (KeyWidthProportion * (renderTarget.Point.X - (keyWidth / 2f))));
            int heightOffset = SourceHeight - OffsetBottom - EffectiveSourceHeight; // DarthAffe 05.11.2016: Vertical flipping doesn't mather in Extend-Mode

            // DarthAffe 06.11.2016: Validate offsets - rounding errors might cause problems (heightOffset is safe calculated -> no need to validate)
            widthOffset = Math.Max(0, Math.Min(SourceWidth - widthPixels, widthOffset));

            for (int y = 0; y < EffectiveSourceHeight; y += Increment)
                for (int x = 0; x < widthPixels; x += Increment)
                {
                    int offset = ((((heightOffset + y) * SourceWidth) + widthOffset + x) * 4);

                    b += ScreenPixelData[offset];
                    g += ScreenPixelData[offset + 1];
                    r += ScreenPixelData[offset + 2];
                    counter++;
                }

            return new CorsairColor((byte)(r / counter), (byte)(g / counter), (byte)(b / counter));
        }

        #endregion
    }
}
