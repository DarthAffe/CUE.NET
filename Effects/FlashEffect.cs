// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Drawing;
using CUE.NET.Brushes;

namespace CUE.NET.Effects
{
    /// <summary>
    /// Represents an effect which allows to flash an brush by modifying his opacity.
    /// </summary>
    public class FlashEffect : AbstractEffect
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the brush which is drawn by the effect.
        /// </summary>
        public override IBrush EffectBrush { get; }

        /// <summary>
        /// Gets or sets the attack-time (in seconds) of the effect. (default: 0.2f)<br />
        /// This is close to a synthesizer envelope. (See <see cref="http://en.wikipedia.org/wiki/Synthesizer#ADSR_envelope" /> as reference)
        /// </summary>
        public float Attack { get; set; } = 0.2f;

        /// <summary>
        /// Gets or sets the decay-time (in seconds) of the effect. (default: 0f)<br />
        /// This is close to a synthesizer envelope. (See <see cref="http://en.wikipedia.org/wiki/Synthesizer#ADSR_envelope" /> as reference)
        /// </summary>
        public float Decay { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the sustain-time (in seconds) of the effect. (default: 0.3f)<br />
        /// This is close to a synthesizer envelope. (See <see cref="http://en.wikipedia.org/wiki/Synthesizer#ADSR_envelope" /> as reference)<br />
        /// Note that this value for naming reasons represents the time NOT the level.
        /// </summary>
        public float Sustain { get; set; } = 0.3f;

        /// <summary>
        /// Gets or sets the release-time (in seconds) of the effect. (default: 0.2f)<br />
        /// This is close to a synthesizer envelope. (See <see cref="http://en.wikipedia.org/wiki/Synthesizer#ADSR_envelope" /> as reference)
        /// </summary>
        public float Release { get; set; } = 0.2f;

        /// <summary>
        /// Gets or sets the level to which the oppacity (percentage) should raise in the attack-cycle. (default: 1f);
        /// </summary>
        public float AttackValue { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the level at which the oppacity (percentage) should stay in the sustain-cycle. (default: 1f);
        /// </summary>
        public float SustainValue { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the interval (in seconds) in which the effect should repeat (if repetition is enabled). (default: 1f)
        /// </summary>
        public float Interval { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the amount of repetitions the effect should do until it's finished. Zero means infinite. (default: 0)
        /// </summary>
        public int Repetitions { get; set; } = 0;

        private ADSRPhase _currentPhase;
        private float _currentPhaseValue;
        private int _repetitionCount;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FlashEffect"/> class.
        /// </summary>
        /// <param name="flashColor">The color from which a <see cref="SolidColorBrush" /> should be created and used by this effect.</param>
        public FlashEffect(Color flashColor)
            : this(new SolidColorBrush(flashColor))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlashEffect"/> class.
        /// </summary>
        /// <param name="effectBrush">The brush which should be used by this effect,</param>
        public FlashEffect(IBrush effectBrush)
        {
            this.EffectBrush = effectBrush;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the effect.
        /// </summary>
        /// <param name="deltaTime">The elapsed time (in seconds) since the last update.</param>
        public override void Update(float deltaTime)
        {
            _currentPhaseValue -= deltaTime;

            // Using ifs instead of a switch allows to skip phases with time 0.

            if (_currentPhase == ADSRPhase.Attack)
                if (_currentPhaseValue > 0f)
                    EffectBrush.Opacity = Math.Min(1f, (Attack - _currentPhaseValue) / Attack) * AttackValue;
                else
                {
                    _currentPhaseValue = Decay;
                    _currentPhase = ADSRPhase.Decay;
                }

            if (_currentPhase == ADSRPhase.Decay)
                if (_currentPhaseValue > 0f)
                    EffectBrush.Opacity = SustainValue + (Math.Min(1f, _currentPhaseValue / Decay) * (AttackValue - SustainValue));
                else
                {
                    _currentPhaseValue = Sustain;
                    _currentPhase = ADSRPhase.Sustain;
                }

            if (_currentPhase == ADSRPhase.Sustain)
                if (_currentPhaseValue > 0f)
                    EffectBrush.Opacity = SustainValue;
                else
                {
                    _currentPhaseValue = Release;
                    _currentPhase = ADSRPhase.Release;
                }

            if (_currentPhase == ADSRPhase.Release)
                if (_currentPhaseValue > 0f)
                    EffectBrush.Opacity = Math.Min(1f, _currentPhaseValue / Release) * SustainValue;
                else
                {
                    _currentPhaseValue = Interval;
                    _currentPhase = ADSRPhase.Pause;
                }

            if (_currentPhase == ADSRPhase.Pause)
                if (_currentPhaseValue > 0f)
                    EffectBrush.Opacity = 0f;
                else
                {
                    if (++_repetitionCount >= Repetitions && Repetitions > 0)
                        IsDone = true;
                    _currentPhaseValue = Attack;
                    _currentPhase = ADSRPhase.Attack;
                }
        }

        /// <summary>
        /// Resets the effect.
        /// </summary>
        public override void OnAttach()
        {
            base.OnAttach();

            _currentPhase = ADSRPhase.Attack;
            _currentPhaseValue = Attack;
            _repetitionCount = 0;
            EffectBrush.Opacity = 0f;
        }

        #endregion

        // ReSharper disable once InconsistentNaming
        private enum ADSRPhase
        {
            Attack,
            Decay,
            Sustain,
            Release,
            Pause
        }
    }
}
