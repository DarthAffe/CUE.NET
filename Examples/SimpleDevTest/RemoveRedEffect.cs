using CUE.NET.Devices.Generic;
using CUE.NET.Effects;

namespace SimpleDevTest
{
    public class RemoveRedEffect : AbstractBrushEffect
    {
        public override void Update(float deltaTime)
        {
            foreach (CorsairColor colors in Brush.RenderedTargets.Values)
                colors.R = 0;
        }
    }
}
