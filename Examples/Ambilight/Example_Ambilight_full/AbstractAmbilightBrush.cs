using System;
using System.Collections.Generic;
using System.Drawing;
using CUE.NET.Brushes;
using Example_Ambilight_full.TakeAsIs;
using Example_Ambilight_full.TakeAsIs.Model;
using Example_Ambilight_full.TakeAsIs.Model.Extensions;
using Example_Ambilight_full.TakeAsIs.ScreenCapturing;

namespace Example_Ambilight_full
{
    public abstract class AbstractAmbilightBrush : AbstractBrush
    {
        #region Properties & Fields

        protected readonly IScreenCapture ScreenCapture;
        protected readonly AmbilightSettings Settings;

        protected byte[] ScreenPixelData;

        protected int SourceWidth;
        protected int SourceHeight;

        protected int OffsetLeft;
        protected int OffsetRight;
        protected int OffsetTop;
        protected int OffsetBottom;

        protected int EffectiveSourceWidth;
        protected int EffectiveSourceHeight;

        protected int Increment;
        protected float KeyWidthProportion;
        protected float KeyHeightProportion;

        #endregion

        #region Constructors

        public AbstractAmbilightBrush(IScreenCapture screenCapture, AmbilightSettings settings)
        {
            this.ScreenCapture = screenCapture;
            this.Settings = settings;
        }

        #endregion

        #region Methods

        public override void PerformRender(RectangleF rectangle, IEnumerable<BrushRenderTarget> renderTargets)
        {
            ScreenPixelData = ScreenCapture.CaptureScreen();

            SourceWidth = ScreenCapture.Width;
            SourceHeight = ScreenCapture.Height;

            OffsetLeft = Settings.OffsetLeft + (Settings.BlackBarDetectionMode.HasFlag(BlackBarDetectionMode.Left)
                                 ? ScreenPixelData.DetectBlackBarLeft(SourceWidth, SourceHeight, Settings.OffsetLeft, Settings.OffsetRight, Settings.OffsetTop, Settings.OffsetBottom)
                                 : 0);
            OffsetRight = Settings.OffsetRight + (Settings.BlackBarDetectionMode.HasFlag(BlackBarDetectionMode.Right)
                                  ? ScreenPixelData.DetectBlackBarRight(SourceWidth, SourceHeight, Settings.OffsetLeft, Settings.OffsetRight, Settings.OffsetTop, Settings.OffsetBottom)
                                  : 0);
            OffsetTop = Settings.OffsetTop + (Settings.BlackBarDetectionMode.HasFlag(BlackBarDetectionMode.Top)
                                ? ScreenPixelData.DetectBlackBarTop(SourceWidth, SourceHeight, Settings.OffsetLeft, Settings.OffsetRight, Settings.OffsetTop, Settings.OffsetBottom)
                                : 0);
            OffsetBottom = Settings.OffsetBottom + (Settings.BlackBarDetectionMode.HasFlag(BlackBarDetectionMode.Bottom)
                                   ? ScreenPixelData.DetectBlackBarBottom(SourceWidth, SourceHeight, Settings.OffsetLeft, Settings.OffsetRight, Settings.OffsetTop, Settings.OffsetBottom)
                                   : 0);

            EffectiveSourceWidth = SourceWidth - OffsetLeft - OffsetRight;
            EffectiveSourceHeight = (int)Math.Round((SourceHeight - OffsetTop - OffsetBottom) * (Settings.MirroredAmount / 100.0));

            Increment = Math.Max(1, Math.Min(20, Settings.Downsampling));

            KeyWidthProportion = EffectiveSourceWidth / rectangle.Width;
            KeyHeightProportion = EffectiveSourceHeight / rectangle.Height;
            
            Opacity = Settings.SmoothMode == SmoothMode.Low ? 0.25f : (Settings.SmoothMode == SmoothMode.Medium ? 0.075f : (Settings.SmoothMode == SmoothMode.High ? 0.025f : 1f /*None*/));

            base.PerformRender(rectangle, renderTargets);
        }

        #endregion
    }
}
