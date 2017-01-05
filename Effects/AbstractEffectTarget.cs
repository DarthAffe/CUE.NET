// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.Linq;
using CUE.NET.Exceptions;

namespace CUE.NET.Effects
{
    /// <summary>
    /// Represents an generic effect-target.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractEffectTarget<T> : IEffectTarget<T>
        where T : IEffectTarget<T>
    {
        #region Properties & Fields

        /// <summary>
        /// Gets a list of <see cref="EffectTimeContainer"/> storing the attached effects.
        /// </summary>
        protected IList<EffectTimeContainer> EffectTimes { get; } = new List<EffectTimeContainer>();

        /// <summary>
        /// Gets all <see cref="IEffect{T}" /> attached to this target.
        /// </summary>
        protected IList<IEffect<T>> Effects => EffectTimes.Select(x => x.Effect).Cast<IEffect<T>>().ToList();

        /// <summary>
        /// Gets the strongly-typed target used for the effect.
        /// </summary>
        protected abstract T EffectTarget { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates all effects added to this target.
        /// </summary>
        public virtual void UpdateEffects()
        {
            lock (Effects)
            {
                for (int i = EffectTimes.Count - 1; i >= 0; i--)
                {
                    EffectTimeContainer effectTime = EffectTimes[i];
                    long currentTicks = DateTime.Now.Ticks;

                    float deltaTime;
                    if (effectTime.TicksAtLastUpdate < 0)
                    {
                        effectTime.TicksAtLastUpdate = currentTicks;
                        deltaTime = 0f;
                    }
                    else
                        deltaTime = (currentTicks - effectTime.TicksAtLastUpdate) / 10000000f;

                    effectTime.TicksAtLastUpdate = currentTicks;
                    effectTime.Effect.Update(deltaTime);


                    if (effectTime.Effect.IsDone)
                        EffectTimes.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Adds an affect.
        /// </summary>
        /// <param name="effect">The effect to add.</param>
        public virtual void AddEffect(IEffect<T> effect)
        {
            if (EffectTimes.Any(x => x.Effect == effect)) return;

            if (!effect.CanBeAppliedTo(EffectTarget))
                throw new WrapperException($"Failed to add effect.\r\nThe effect of type '{effect.GetType()}' can't be applied to the target of type '{EffectTarget.GetType()}'.");

            effect.OnAttach(EffectTarget);
            EffectTimes.Add(new EffectTimeContainer(effect, -1));
        }

        /// <summary>
        /// Removes an effect
        /// </summary>
        /// <param name="effect">The effect to remove.</param>
        public virtual void RemoveEffect(IEffect<T> effect)
        {
            EffectTimeContainer effectTimeToRemove = EffectTimes.FirstOrDefault(x => x.Effect == effect);
            if (effectTimeToRemove == null) return;

            effect.OnDetach(EffectTarget);
            EffectTimes.Remove(effectTimeToRemove);
        }

        #endregion
    }
}
