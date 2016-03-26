using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Effects;
using Example_AudioAnalyzer_full.TakeAsIs;

namespace Example_AudioAnalyzer_full
{
    public class SongBeatEffect : AbstractEffect
    {
        #region Constants

        // This value is really hard to tweak since it highly depends on the music played ... But hey, this is a example after all - feel free to change this until it looks good for you
        private const float SONG_BEAT_THRESHOLD = 0.9f;

        private const float FLASH_DURATION = 0.15f;

        #endregion

        #region Properties & Fields

        private SoundDataProcessor _dataProcessor;

        private float _currentEffect = -1f;

        private SolidColorBrush _brush;
        public override IBrush EffectBrush => _brush;

        #endregion

        #region Constructors

        public SongBeatEffect(SoundDataProcessor dataProcessor, Color color)
        {
            this._dataProcessor = dataProcessor;
            _brush = new SolidColorBrush(color);
        }

        #endregion

        #region Methods

        public override void Update(float deltaTime)
        {
            // ... update the effect-data ...
            if (_dataProcessor.SongBeat >= SONG_BEAT_THRESHOLD)
                _currentEffect = FLASH_DURATION;
            else
                _currentEffect -= deltaTime;

            // and set the current brush-opacity
            _brush.Opacity = _currentEffect > 0f ? (_currentEffect / FLASH_DURATION) : 0f;
        }

        #endregion
    }
}
