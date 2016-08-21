using System;
using System.Drawing;
using CUE.NET.Brushes;

namespace CUE.NET.Effects
{
    public class RippleEffect : AbstractEffect
    {
        #region Properties & Fields

        private RippleBrush _brush = new RippleBrush();
        public override IBrush EffectBrush => _brush;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public override void Update(float deltaTime)
        {
            throw new NotImplementedException();
        }

        #endregion

        private class RippleBrush : AbstractBrush
        {
            public override Color GetColorAtPoint(RectangleF rectangle, PointF point)
            {
                return FinalizeColor(Color.Black);
            }
        }
    }
}
