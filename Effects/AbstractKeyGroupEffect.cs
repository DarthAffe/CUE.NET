// ReSharper disable MemberCanBePrivate.Global

using CUE.NET.Devices.Keyboard.Keys;

namespace CUE.NET.Effects
{
    /// <summary>
    /// Represents a basic effect targeting an <see cref="IKeyGroup"/>.
    /// </summary>
    public abstract class AbstractKeyGroupEffect : IEffect<IKeyGroup>
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets if this effect has finished all of his work.
        /// </summary>
        public bool IsDone { get; protected set; }

        /// <summary>
        /// Gets the <see cref="IKeyGroup"/> this effect is targeting.
        /// </summary>
        protected IKeyGroup KeyGroup { get; private set; }

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
        /// <param name="target">The <see cref="IKeyGroup"/> this effect is attached to.</param>
        public virtual void OnAttach(IKeyGroup target)
        {
            KeyGroup = target;
        }

        /// <summary>
        /// Hook which is called when the effect is detached from a device.
        /// </summary>
        /// <param name="target">The <see cref="IKeyGroup"/> this effect is detached from.</param>
        public virtual void OnDetach(IKeyGroup target)
        {
            KeyGroup = null;
        }

        #endregion
    }
}
