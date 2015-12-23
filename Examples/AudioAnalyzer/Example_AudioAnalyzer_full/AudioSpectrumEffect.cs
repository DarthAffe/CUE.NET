using CUE.NET.Brushes;
using CUE.NET.Effects;
using CUE.NET.Gradients;
using Example_AudioAnalyzer_full.TakeAsIs;

namespace Example_AudioAnalyzer_full
{
    public class AudioSpectrumEffect : AbstractEffect
    {
        #region Properties & Fields

        private SoundDataProcessor _dataProcessor;

        private AudioSpectrumBrush _audioSpectrumBrush;
        public override IBrush EffectBrush => _audioSpectrumBrush;

        #endregion

        #region Constructors

        public AudioSpectrumEffect(SoundDataProcessor dataProcessor, IGradient gradient)
        {
            this._dataProcessor = dataProcessor;
            _audioSpectrumBrush = new AudioSpectrumBrush(gradient);

            // Give this effect a high Z-Index to keep it in the foreground
            ZIndex = 10;
        }

        #endregion

        #region Methods

        public override void Update(float deltaTime)
        {
            // calculate new data ... - we don't need to do this, since we know that the song beat-effect already calculated them
            //_dataProcessor.Process();

            // ... and update the brush
            _audioSpectrumBrush.BarData = _dataProcessor.BarValues;
        }

        #endregion
    }
}
