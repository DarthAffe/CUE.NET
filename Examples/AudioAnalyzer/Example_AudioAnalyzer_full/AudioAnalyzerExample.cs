using System;
using System.Collections.Generic;
using System.Drawing;
using CUE.NET;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Keyboard;
using CUE.NET.Devices.Keyboard.Keys;
using CUE.NET.Exceptions;
using CUE.NET.Gradients;
using CUE.NET.Groups;
using Example_AudioAnalyzer_full.TakeAsIs;

namespace Example_AudioAnalyzer_full
{
    /* ##################################################################################################
     * #                                                                                                #
     * #  This example demonstrates how to write a simple Spectrograph for your keyboard using CUE.NET  #
     * #  making extensive use of CUE.NET features (mostly own effects and brushes).                    #
     * #  Code in the TakeAsIs-folder and -regions is logic not related to CUE.NET -> ignore it         #
     * #                                                                                                #
     * #  Most of the audio-analysis stuff is taken from AterialDawn's project:                         #
     * #  https://github.com/AterialDawn/CUEAudioVisualizer/tree/master/CUEAudioVisualizer              #
     * #                                                                                                #
     * ##################################################################################################
     */
    public class AudioAnalyzerExample
    {
        #region Constants

        // This value will vertical scale the spectrum. Since it highly depends on the music which value looks good, feel free to tweak it until you like it.
        private const float VOLUME_SCALAR = 0.5f;

        #endregion

        #region Properties & Fields

        private SoundDataProcessor _soundDataProcessor; // Take as is

        private CorsairKeyboard _keyboard;

        #endregion

        #region Methods

        public void Initialize()
        {
            // Initialize everything
            CueSDK.Initialize();

            _keyboard = CueSDK.KeyboardSDK;
            if (_keyboard == null)
                throw new WrapperException("No keyboard found ...");

            // With this you could set the update frequency - the default (30 updates per second) should be fine for the most things
            //_keyboard.UpdateFrequency = 1f / 60f; // 60 updates per second

            // This is useful for debugging-purposes
            _keyboard.Exception += (sender, args) => Console.WriteLine(args.Exception.Message);

            Console.WriteLine("CUE.NET initialized!");
        }

        public void Run()
        {
            _keyboard.UpdateMode = UpdateMode.Continuous;
            // Add a black background. We want this to be semi-transparent to add some sort of fade-effect - this will smooth everything out a bit
            // Note that this isn't a 'real effect' since it's update-rate dependent. A real effect would do always the same thing not mather how fast the keyboard updates.
            _keyboard.Brush = new SolidColorBrush(Color.FromArgb(96, 0, 0, 0));
            // Add our song-beat-effect. Remember to uncomment the update in the spectrum effect if you want to remove this.
            ListLedGroup songBeatGroup = new ListLedGroup(_keyboard, _keyboard);
            songBeatGroup.Brush = new SolidColorBrush(Color.FromArgb(127, 164, 164, 164));
            songBeatGroup.Brush.AddEffect(new SongBeatEffect(_soundDataProcessor));

            // Add our spectrum-effect using the soundDataProcessor and a rainbow from purple to red as gradient
            ListLedGroup spectrumGroup = new ListLedGroup(_keyboard, _keyboard);
            spectrumGroup.Brush = new AudioSpectrumBrush(_soundDataProcessor, new RainbowGradient(300, -14));

            // Hook onto the keyboard update and process data
            _keyboard.Updating += (sender, args) => _soundDataProcessor.Process();

            // If you don't like rainbows replace the gradient with anything you like. For example:
            //_keyboard.AttachEffect(new AudioSpectrumEffect(_soundDataProcessor, new LinearGradient(new GradientStop(0f, Color.Blue), new GradientStop(1f, Color.Red))));

            // We need something to block since the keyboard-effect-update-loop is running async and the console-app would exit otherwise.
            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
        }

        #endregion

        #region TakeAsIs

        private void InitBass()
        {
            BassHelper.InitializeBass();

            // Ask for device to use
            List<Tuple<string, int>> bassDevices = BassHelper.GetBassDevices();
            int? selectedIndex = null;
            do
            {
                Console.Clear();
                Console.WriteLine("Select device:");

                for (int i = 0; i < bassDevices.Count && i < 10; i++)
                    Console.WriteLine($"  {i}. {bassDevices[i].Item1}");

                char input = Console.ReadKey().KeyChar;
                int tmp;
                if (int.TryParse(input.ToString(), out tmp) && tmp < bassDevices.Count)
                    selectedIndex = tmp;

            } while (!selectedIndex.HasValue);
            Console.WriteLine();

            _soundDataProcessor = new SoundDataProcessor(bassDevices[selectedIndex.Value].Item2) { VolumeScalar = VOLUME_SCALAR };
        }

        public static void Main(string[] args)
        {
            try
            {
                AudioAnalyzerExample aae = new AudioAnalyzerExample();
                aae.InitBass();
                aae.Initialize();
                aae.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while running the Audio-Analyzer-Example: {ex.Message}");
                Console.ReadKey();
            }
        }

        #endregion
    }
}
