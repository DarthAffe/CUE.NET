using System;
using System.Windows;
using Example_Ambilight_full.TakeAsIs;
using Example_Ambilight_full.TakeAsIs.Helper;
using Example_Ambilight_full.TakeAsIs.ScreenCapturing;
using Example_Ambilight_full.TakeAsIs.UI;
using Hardcodet.Wpf.TaskbarNotification;

namespace Example_Ambilight_full
{
    public partial class App : Application
    {
        #region Constants

        private const string PATH_SETTINGS = "Settings.xaml";

        #endregion

        #region Properties & Fields

        private TaskbarIcon _taskBar;
        private AmbilightSettings _settings;
        private Ambilight _ambilight;

        #endregion

        #region Methods

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                _settings = SerializationHelper.LoadObjectFromFile<AmbilightSettings>(PATH_SETTINGS) ??
                    new AmbilightSettings();
                IScreenCapture screenCapture = new DX9ScreenCapture();

                // DarthAffe 05.11.2016: This could be done way cleaner ...
                _taskBar = FindResource("Taskbar") as TaskbarIcon;
                FrameworkElement configView = _taskBar?.TrayPopup as ConfigView;
                if (configView == null)
                    Shutdown();
                else
                    configView.DataContext = new ConfigViewModel(_settings);

                _ambilight = new Ambilight(screenCapture, _settings);
                if (!_ambilight.Initialize())
                    throw new ApplicationException();
            }
            catch
            {
                MessageBox.Show("An error occured while starting the Keyboard-Ambilight.\r\nPlease double check if CUE is running and 'Enable SDK' is checked.", "Can't start Keyboard-Ambilight");
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            SerializationHelper.SaveObjectToFile(_settings, PATH_SETTINGS);
        }

        #endregion
    }
}
