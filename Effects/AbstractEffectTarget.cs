// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.Linq;

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
        
        private IList<EffectTimeContainer> _effectTimes = new List<EffectTimeContainer>();

        /// <summary>
        /// Gets all <see cref="IEffect{T}" /> attached to this target.
        /// </summary>
        protected IList<IEffect<T>> Effects => _effectTimes.Select(x => x.Effect).Cast<IEffect<T>>().ToList();

        /// <summary>
        /// Gets the strongly-typed target used for the effect.
        /// </summary>
        protected abstract T EffectTarget { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates all effects added to this target.
        /// </summary>
        public void UpdateEffects()
        {
            lock (Effects)
            {
                for (int i = _effectTimes.Count - 1; i >= 0; i--)
                {
                    EffectTimeContainer effectTime = _effectTimes[i];
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
                        _effectTimes.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Adds an affect.
        /// </summary>
        /// <param name="effect">The effect to add.</param>
        public void AddEffect(IEffect<T> effect)
        {
            if (_effectTimes.All(x => x.Effect != effect))
            {
                effect.OnAttach(EffectTarget);
                _effectTimes.Add(new EffectTimeContainer(effect, -1));
            }
        }

        /// <summary>
        /// Removes an effect
        /// </summary>
        /// <param name="effect">The effect to remove.</param>
        public void RemoveEffect(IEffect<T> effect)
        {
            EffectTimeContainer effectTimeToRemove = _effectTimes.FirstOrDefault(x => x.Effect == effect);
            if (effectTimeToRemove != null)
            {
                effect.OnDetach(EffectTarget);
                _effectTimes.Remove(effectTimeToRemove);
            }
        }

        #endregion
    }
}
