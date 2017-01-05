using CUE.NET.Brushes;
using CUE.NET.Gradients;

namespace CUE.NET.Effects
{
    /// <summary>
    /// Represents an effect which allows to move an gradient by modifying his offset.
    /// </summary>
    public class MoveGradientEffect : AbstractBrushEffect<IGradientBrush>
    {
        #region Properties & Fields
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
        // ReSharper disable MemberCanBePrivate.Global

        /// <summary>
        /// Gets or sets the direction the gradient is moved.
        /// True leads to an offset-increment (normaly moving to the right), false to an offset-decrement (normaly moving to the left).
        /// </summary>
        public bool Direction { get; set; }

        /// <summary>
        /// Gets or sets the speed of the movement in units per second.
        /// The meaning of units differs for the different gradients, but 360 units will always be one complete cycle:
        ///   LinearGradient: 360 unit = 1 offset.
        ///   RainbowGradient: 1 unit = 1 degree.
        /// </summary>
        public float Speed { get; set; }

        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="direction"></param>
        public MoveGradientEffect(float speed = 180f, bool direction = true)
        {
            this.Speed = speed;
            this.Direction = direction;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Updates the effect.
        /// </summary>
        /// <param name="deltaTime">The elapsed time (in seconds) since the last update.</param>
        public override void Update(float deltaTime)
        {
            float movement = Speed * deltaTime;

            if (!Direction)
                movement = -movement;

            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (Brush.Gradient is LinearGradient)
            {
                LinearGradient linearGradient = (LinearGradient)Brush.Gradient;

                movement /= 360f;

                foreach (GradientStop gradientStop in linearGradient.GradientStops)
                {
                    gradientStop.Offset = gradientStop.Offset + movement;

                    if (gradientStop.Offset > 1f)
                        gradientStop.Offset -= 1f;
                    else if (gradientStop.Offset < 0)
                        gradientStop.Offset += 1f;
                }
            }
            else if (Brush.Gradient is RainbowGradient)
            {
                RainbowGradient rainbowGradient = (RainbowGradient)Brush.Gradient;

                // RainbowGradient is calculated inverse but the movement should be the same for all.
                movement *= -1;

                rainbowGradient.StartHue += movement;
                rainbowGradient.EndHue += movement;
                
                if (rainbowGradient.StartHue > 360f && rainbowGradient.EndHue > 360f)
                {
                    rainbowGradient.StartHue -= 360f;
                    rainbowGradient.EndHue -= 360f;
                }
                else if (rainbowGradient.StartHue < -360f && rainbowGradient.EndHue < -360f)
                {
                    rainbowGradient.StartHue += 360f;
                    rainbowGradient.EndHue += 360f;
                }
            }
        }

        #endregion
    }
}
