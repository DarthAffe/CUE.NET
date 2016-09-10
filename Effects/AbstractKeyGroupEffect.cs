// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using CUE.NET.Devices.Keyboard.Keys;
using CUE.NET.Groups;

namespace CUE.NET.Effects
{
    /// <summary>
    /// Represents a basic effect targeting an <see cref="ILedGroup"/>.
    /// </summary>
    public abstract class AbstractKeyGroupEffect : IEffect<ILedGroup>
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets if this effect has finished all of his work.
        /// </summary>
        public bool IsDone { get; protected set; }

        /// <summary>
        /// Gets the <see cref="ILedGroup"/> this effect is targeting.
        /// </summary>
        protected ILedGroup LedGroup { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the effect.
        /// </summary>
        /// <param name="deltaTime">The elapsed time (in seconds) since the last update.</param>
        public abstract void Update(float deltaTime);

        /// <summary>
        /// Hook which is called when the effect is attached to a device.
        /// </summary>
        /// <param name="target">The <see cref="ILedGroup"/> this effect is attached to.</param>
        public virtual void OnAttach(ILedGroup target)
        {
            LedGroup = target;
        }

        /// <summary>
        /// Hook which is called when the effect is detached from a device.
        /// </summary>
        /// <param name="target">The <see cref="ILedGroup"/> this effect is detached from.</param>
        public virtual void OnDetach(ILedGroup target)
        {
            LedGroup = null;
        }

        #endregion
    }
}
