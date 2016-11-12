using System;
using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using Example_Ambilight_full.TakeAsIs;
using Example_Ambilight_full.TakeAsIs.Model;
using Example_Ambilight_full.TakeAsIs.ScreenCapturing;

namespace Example_Ambilight_full
{
    public class AmbilightMirrorBrush : AbstractAmbilightBrush
    {
        #region Constructors

        public AmbilightMirrorBrush(IScreenCapture screenCapture, AmbilightSettings settings)
            : base(screenCapture, settings)
        { }

        #endregion

        #region Methods

        protected override CorsairColor GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
        {
            //TODO DarthAffe 05.11.2016: The Key-Rectangle is missing in the render-target, I'll fix that with a future version of CUE.NET. Until then we consider the size of each key to be 10x10mm.
            float keyWidth = 10;
            float keyHeight = 10;

            int widthPixels = Math.Max(1, (int)(KeyWidthProportion * keyWidth));
            int heightPixels = Math.Max(1, (int)(KeyHeightProportion * keyHeight));

            int r = 0;
            int g = 0;
            int b = 0;
            int counter = 0;

            int widthOffset = Settings.FlipMode.HasFlag(FlipMode.Horizontal)
                ? ((SourceWidth - OffsetRight) - (int)(KeyWidthProportion * (renderTarget.Point.X + (keyWidth / 2f))))
                : (OffsetLeft + (int)(KeyWidthProportion * (renderTarget.Point.X - (keyWidth / 2f))));
            int heightOffset = Settings.FlipMode.HasFlag(FlipMode.Vertical)
                ? ((SourceHeight - OffsetBottom) - (int)(KeyHeightProportion * (renderTarget.Point.Y + (keyHeight / 2f))))
                : ((SourceHeight - OffsetBottom - EffectiveSourceHeight) + (int)(KeyHeightProportion * (renderTarget.Point.Y - (keyHeight / 2f))));

            // DarthAffe 06.11.2016: Validate offsets - rounding errors might cause problems
            widthOffset = Math.Max(0, Math.Min(SourceWidth - widthPixels, widthOffset));
            heightOffset = Math.Max(0, Math.Min(SourceHeight - heightPixels, heightOffset));

            for (int y = 0; y < heightPixels; y += Increment)
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
