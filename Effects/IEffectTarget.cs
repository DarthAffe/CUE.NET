namespace CUE.NET.Effects
{
    /// <summary>
    /// Represents a basic effect-target.
    /// </summary>
    /// <typeparam name="T">The type this target represents.</typeparam>
    public interface IEffectTarget<out T>
        where T : IEffectTarget<T>
    {
        #region Methods

        /// <summary>
        /// Updates all effects added to this target.
        /// </summary>
        void UpdateEffects();

        /// <summary>
        /// Adds an affect.
        /// </summary>
        /// <param name="effect">The effect to add.</param>
        void AddEffect(IEffect<T> effect);

        /// <summary>
        /// Removes an effect
        /// </summary>
        /// <param name="effect">The effect to remove.</param>
        void RemoveEffect(IEffect<T> effect);

        #endregion
    }
}
