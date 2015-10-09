// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System;
using System.Drawing;
using CUE.NET.Devices.Keyboard.Brushes;

namespace CUE.NET.Devices.Keyboard.Effects
{
    public class FlashEffect : AbstractEffect
    {
        #region Properties & Fields

        public override IBrush EffectBrush { get; }

        // Settings are close to a synthesizer envelope (sustain is different for consequent naming): https://en.wikipedia.org/wiki/Synthesizer#ADSR_envelope
        public float Attack { get; set; } = 0.2f;
        public float Decay { get; set; } = 0f;
        public float Sustain { get; set; } = 0.3f;
        public float Release { get; set; } = 0.2f;

        public float SustainValue { get; set; } = 1f;
        public float AttackValue { get; set; } = 1f;

        public float Interval { get; set; } = 1f;

        public int Repetitions { get; set; } = 0;

        private ADSRPhase _currentPhase;
        private float _currentPhaseValue;
        private int _repetitionCount;

        #endregion

        #region Constructors

        public FlashEffect(Color flashColor)
            : this(new SolidColorBrush(flashColor))
        { }

        public FlashEffect(IBrush effectBrush)
        {
            this.EffectBrush = effectBrush;
        }

        #endregion

        #region Methods

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
