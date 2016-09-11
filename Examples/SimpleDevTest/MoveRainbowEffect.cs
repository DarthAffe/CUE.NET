using CUE.NET.Brushes;
using CUE.NET.Effects;
using CUE.NET.Gradients;

namespace SimpleDevTest
{
    public class MoveRainbowEffect : AbstractBrushEffect<IGradientBrush>
    {
        #region Properties & Fields

        public float DegreePerSecond { get; set; } = 30f;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public override void Update(float deltaTime)
        {
            float value = DegreePerSecond * deltaTime;

            //DarthAffe 11.09.2016: Only to test! This will overflow if run for a longer time!!!
            ((RainbowGradient)Brush.Gradient).StartHue += value;
            ((RainbowGradient)Brush.Gradient).EndHue += value;
        }

        public override bool CanBeAppliedTo(IBrush target)
        {
            return (target as IGradientBrush)?.Gradient is RainbowGradient;
        }

        #endregion
    }
}
