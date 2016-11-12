using System;
using Example_Ambilight_full.TakeAsIs.Model;

namespace Example_Ambilight_full.TakeAsIs
{
    public class AmbilightSettings
    {
        private AmbienceCreatorType _ambienceCreatorType = AmbienceCreatorType.Mirror;

        #region Properties & Fields

        public AmbienceCreatorType AmbienceCreatorType
        {
            get { return _ambienceCreatorType; }
            set
            {
                if (_ambienceCreatorType != value)
                {
                    _ambienceCreatorType = value;
                    AmbienceCreatorTypeChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public int OffsetLeft { get; set; } = 0;
        public int OffsetRight { get; set; } = 0;
        public int OffsetTop { get; set; } = 0;
        public int OffsetBottom { get; set; } = 0;

        public int Downsampling { get; set; } = 2;
        public double MirroredAmount { get; set; } = 10;

        public SmoothMode SmoothMode { get; set; } = SmoothMode.Low;
        public BlackBarDetectionMode BlackBarDetectionMode { get; set; } = BlackBarDetectionMode.Bottom;
        public FlipMode FlipMode { get; set; } = FlipMode.Vertical;

        public double MinLightness { get; set; } = 0;

        #endregion

        #region Events

        public event EventHandler AmbienceCreatorTypeChanged;

        #endregion
    }
}
