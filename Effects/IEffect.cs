// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Global

namespace CUE.NET.Effects
{
    /// <summary>
    /// Represents a basic effect.
    /// </summary>
    public interface IEffect
    {
        #region Properties & Fields

        /// <summary>
        /// Gets if this effect has finished all of his work.
        /// </summary>
        bool IsDone { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the effect.
        /// </summary>
        /// <param name="deltaTime">The elapsed time (in seconds) since the last update.</param>
        void Update(float deltaTime);

        #endregion
    }

    /// <summary>
    /// Represents a basic effect.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IEffectTarget{T}"/> this effect can be attached to.</typeparam>
    public interface IEffect<in T> : IEffect
        where T : IEffectTarget<T>
    {
        #region Methods

        /// <summary>
        /// Checks if the effect can be applied to the target object.
        /// </summary>
        /// <param name="target">The <see cref="IEffectTarget{T}"/> this effect is attached to.</param>
        /// <returns><c>true</c> if the effect can be attached; otherwise, <c>false</c>.</returns>
        bool CanBeAppliedTo(T target);

        /// <summary>
        /// Hook which is called when the effect is attached to a device.
        /// </summary>
        /// <param name="target">The <see cref="IEffectTarget{T}"/> this effect is attached to.</param>
        void OnAttach(T target);

        /// <summary>
        /// Hook which is called when the effect is detached from a device.
        /// </summary>
        /// <param name="target">The <see cref="IEffectTarget{T}"/> this effect is detached from.</param>
        void OnDetach(T target);

        #endregion
    }
}
