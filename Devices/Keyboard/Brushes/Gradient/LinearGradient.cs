using System.Drawing;
using System.Linq;

namespace CUE.NET.Devices.Keyboard.Brushes.Gradient
{
    public class LinearGradient : AbstractGradient
    {
        #region Constructors

        public LinearGradient()
        { }

        public LinearGradient(params GradientStop[] gradientStops)
            : base(gradientStops)
        { }

        #endregion

        #region Methods

        public override Color GetColor(float offset)
        {
            if (!GradientStops.Any()) return Color.Transparent;
            if (GradientStops.Count == 1) return GradientStops.First().Color;

            offset = ClipOffset(offset);

            GradientStop gsBefore = GradientStops.Where(n => n.Offset <= offset).OrderBy(n => n.Offset).Last();
            GradientStop gsAfter = GradientStops.Where(n => n.Offset >= offset).OrderBy(n => n.Offset).First();

            float blendFactor = 0f;
            if (!gsBefore.Offset.Equals(gsAfter.Offset))
                blendFactor = ((offset - gsBefore.Offset) / (gsAfter.Offset - gsBefore.Offset));

            byte colA = (byte)((gsAfter.Color.A - gsBefore.Color.A) * blendFactor + gsBefore.Color.A);
            byte colR = (byte)((gsAfter.Color.R - gsBefore.Color.R) * blendFactor + gsBefore.Color.R);
            byte colG = (byte)((gsAfter.Color.G - gsBefore.Color.G) * blendFactor + gsBefore.Color.G);
            byte colB = (byte)((gsAfter.Color.B - gsBefore.Color.B) * blendFactor + gsBefore.Color.B);

            return Color.FromArgb(colA, colR, colG, colB);
        }

        #endregion
    }
}
