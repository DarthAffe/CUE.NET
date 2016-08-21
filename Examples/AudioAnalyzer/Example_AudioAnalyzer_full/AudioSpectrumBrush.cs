using System;
using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Gradients;
using Example_AudioAnalyzer_full.TakeAsIs;

namespace Example_AudioAnalyzer_full
{
    // Extending from LinearGradientBrush since this is all we want to draw - of course you could implement everything on your own
    public class AudioSpectrumBrush : LinearGradientBrush
    {
        #region Properties & Fields

        private SoundDataProcessor _soundDataProcessor;

        #endregion

        #region Constructors

        // default values for start/end are fine
        public AudioSpectrumBrush(SoundDataProcessor soundDataProcessor, IGradient gradient)
            : base(gradient)
        {
            this._soundDataProcessor = soundDataProcessor;
        }

        #endregion

        #region Methods

        protected override Color GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
        {
            if (_soundDataProcessor?.BarValues == null) return Color.Transparent;

            // This logic is also stolen from AterialDawn
            int barSampleIndex = (int)Math.Floor(_soundDataProcessor.BarValues.Length * (renderTarget.Point.X / (rectangle.X + rectangle.Width))); // Calculate bar sampling index
            float curBarHeight = 1f - Utility.Clamp(_soundDataProcessor.BarValues[barSampleIndex], 0f, 1f); // Invert this value since the keyboard is laid out with topleft being point 0,0
            float verticalPos = (renderTarget.Point.Y / rectangle.Height);

            // If the barHeight is lower than the vertical pos currently calculated return the brush value. Otherwise do nothing by returning transparent.
            return curBarHeight <= verticalPos ? base.GetColorAtPoint(rectangle, renderTarget) : Color.Transparent;
        }

        #endregion
    }
}
