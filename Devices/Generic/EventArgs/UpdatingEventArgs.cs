// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace CUE.NET.Devices.Generic.EventArgs
{
    /// <summary>
    /// Represents the information supplied with an <see cref="ICueDevice.Updating"/>-event.
    /// </summary>
    public class UpdatingEventArgs : System.EventArgs
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the elapsed time (in seconds) sonce the last update.
        /// </summary>
        public float DeltaTime { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatingEventArgs"/> class.
        /// </summary>
        /// <param name="deltaTime">The elapsed time (in seconds) sonce the last update.</param>
        public UpdatingEventArgs(float deltaTime)
        {
            this.DeltaTime = deltaTime;
        }

        #endregion
    }
}
