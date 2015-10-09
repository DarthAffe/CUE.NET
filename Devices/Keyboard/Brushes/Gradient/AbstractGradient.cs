// ReSharper disable MemberCanBeProtected.Global

using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CUE.NET.Devices.Keyboard.Brushes.Gradient
{
    public abstract class AbstractGradient : IGradient
    {
        #region Properties & Fields

        public IList<GradientStop> GradientStops { get; } = new List<GradientStop>();

        #endregion

        #region Constructors

        protected AbstractGradient()
        { }

        protected AbstractGradient(params GradientStop[] gradientStops)
        {
            foreach (GradientStop gradientStop in gradientStops)
                GradientStops.Add(gradientStop);
        }

        #endregion

        #region Methods

        protected float ClipOffset(float offset)
        {
            float max = GradientStops.Max(n => n.Offset);
            if (offset > max)
                return max;

            float min = GradientStops.Min(n => n.Offset);
            if (offset < min)
                return min;

            return offset;
        }

        public abstract Color GetColor(float offset);

        #endregion
    }
}
