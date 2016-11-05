using System;
using System.Drawing;
using CUE.NET;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic.Enums;
using Example_Ambilight_full.TakeAsIs;
using Example_Ambilight_full.TakeAsIs.Model;
using Example_Ambilight_full.TakeAsIs.ScreenCapturing;

namespace Example_Ambilight_full
{
    public class Ambilight
    {
        #region Properties & Fields

        private readonly IScreenCapture _screenCapture;
        private readonly AmbilightSettings _settings;

        #endregion

        #region Constructors

        public Ambilight(IScreenCapture screenCapture, AmbilightSettings settings)
        {
            this._screenCapture = screenCapture;
            this._settings = settings;
        }

        #endregion

        #region Methods

        public bool Initialize()
        {
            try
            {
                CueSDK.Initialize();
                CueSDK.UpdateMode = UpdateMode.Continuous;
                CueSDK.UpdateFrequency = 1 / 20f;

                SetAmbilightBrush();
                _settings.AmbienceCreatorTypeChanged += (sender, args) => SetAmbilightBrush();
            }
            catch { return false; }
            return true;
        }

        private void SetAmbilightBrush()
        {
            IBrush ambilightBrush;
            switch (_settings.AmbienceCreatorType)
            {
                case AmbienceCreatorType.Mirror:
                    ambilightBrush = new AmbilightMirrorBrush(_screenCapture, _settings);
                    break;
                case AmbienceCreatorType.Extend:
                    ambilightBrush = new AmbilightExtendBrush(_screenCapture, _settings);
                    break;
                default:
                    ambilightBrush = new SolidColorBrush(Color.Black);
                    break;
            }

            CueSDK.KeyboardSDK.Brush = ambilightBrush;
        }

        #endregion
    }
}
