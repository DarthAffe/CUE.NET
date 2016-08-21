// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.Linq;

namespace CUE.NET.Effects
{
    public abstract class AbstractEffectTarget<T> : IEffectTarget<T>
        where T : IEffectTarget<T>
    {
        #region Properties & Fields

        private IList<EffectTimeContainer> _effectTimes = new List<EffectTimeContainer>();
        protected IList<IEffect<T>> Effects => _effectTimes.Select(x => x.Effect).Cast<IEffect<T>>().ToList();
        protected abstract T EffectTarget { get; }

        #endregion

        #region Methods

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

        public void AddEffect(IEffect<T> effect)
        {
            if (_effectTimes.All(x => x.Effect != effect))
            {
                effect.OnAttach(EffectTarget);
                _effectTimes.Add(new EffectTimeContainer(effect, -1));
            }
        }

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
