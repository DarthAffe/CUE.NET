using System;
using System.Windows;
using CUE.NET;

namespace Example_Ambilight_full.TakeAsIs.UI
{
    public class ConfigViewModel
    {
        #region Properties & Fields

        public AmbilightSettings Settings { get; }

        public int UpdateRate
        {
            get => (int)Math.Round(1f / CueSDK.UpdateFrequency);
            set
            {
                Settings.UpdateRate = value;
                CueSDK.UpdateFrequency = 1f / value;
            }
        }

        #endregion

        #region Commands

        private ActionCommand _exitCommand;
        public ActionCommand ExitCommand => _exitCommand ?? (_exitCommand = new ActionCommand(Exit));

        #endregion

        #region Constructors

        public ConfigViewModel(AmbilightSettings settings)
        {
            this.Settings = settings;
        }

        #endregion

        #region Methods

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}
