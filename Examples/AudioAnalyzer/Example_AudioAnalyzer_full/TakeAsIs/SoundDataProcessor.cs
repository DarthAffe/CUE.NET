/* 
 * ###################################################################################################################
 * #                                             Thanks to AterialDawn                                               #
 * #                                                                                                                 #
 * # Source: https://github.com/AterialDawn/CUEAudioVisualizer/blob/master/CUEAudioVisualizer/SoundDataProcessor.cs  #
 * #   Date: 23.10.2015                                                                                              #
 * # Commit: 9106ab777d70a23254872455c6f3cefbe9bef6ca                                                                #
 * #                                                                                                                 #
 * ###################################################################################################################
 * 
 * > Note < This code is refactored and all parts not used in this tutorial are removed.
 */

using System;
using Un4seen.Bass;
using Un4seen.BassWasapi;

namespace Example_AudioAnalyzer_full.TakeAsIs
{
    public class SoundDataProcessor
    {
        private const int BAR_COUNT = 1000;

        public float[] BarValues = new float[BAR_COUNT];
        public float VolumeScalar = 1f;
        public float SongBeat = 0;

        private float _averagedVolume = 0;
        private int _wasapiDeviceIndex = -1;
        private BASSData _maxFft = (BASSData.BASS_DATA_FFT8192);
        private float[] _fftData = new float[4096];
        private float[] _freqVolScalar = new float[BAR_COUNT];
        private float[] _barValues = new float[BAR_COUNT];
        private int _sampleFrequency = 48000;
        private int _maximumFrequency = 16384;
        private int _minimumFrequency = 32;
        private int _maximumFrequencyIndex;
        private int _minimumFrequencyIndex;
        private int _deviceNumber;
        private int[] _logIndex = new int[BAR_COUNT];
        private bool _deviceInitialized = false;
        private WASAPIPROC _wasapiProc;

        public SoundDataProcessor(int deviceIndex)
        {
            this._wasapiDeviceIndex = deviceIndex;

            BuildLookupTables();
            _wasapiProc = IgnoreDataProc;
        }

        public void Process()
        {
            if (_deviceNumber != _wasapiDeviceIndex || !_deviceInitialized)
            {
                UpdateDevice();
                _deviceNumber = BassWasapi.BASS_WASAPI_GetDevice();
            }

            if (!_deviceInitialized) return;

            GetBarData();
            float curVol = BassWasapi.BASS_WASAPI_GetDeviceLevel(_deviceNumber, -1) * VolumeScalar;
            _averagedVolume = Utility.Clamp(Utility.LinearInterpolate(_averagedVolume, curVol, 0.02f), 0f, 1f);
        }

        private void UpdateDevice()
        {
            if (_wasapiDeviceIndex == -1) return;
            if (_deviceInitialized)
            {
                Console.WriteLine("Deinitializing WASAPI device");
                BassWasapi.BASS_WASAPI_Stop(true);
                BassWasapi.BASS_WASAPI_Free();
                _deviceInitialized = false;
            }
            BASS_WASAPI_DEVICEINFO devInfo = BassWasapi.BASS_WASAPI_GetDeviceInfo(_wasapiDeviceIndex);
            if (devInfo == null)
                throw new WASAPIInitializationException("Device " + _wasapiDeviceIndex + " is invalid!");

            if (!BassWasapi.BASS_WASAPI_Init(_wasapiDeviceIndex, devInfo.mixfreq, 2, BASSWASAPIInit.BASS_WASAPI_AUTOFORMAT | BASSWASAPIInit.BASS_WASAPI_BUFFER, 0f, 0f, _wasapiProc, IntPtr.Zero))
            {
                BASSError error = Bass.BASS_ErrorGetCode();
                throw new WASAPIInitializationException("Unable to initialize WASAPI device " + _wasapiDeviceIndex, error);
            }

            if (!BassWasapi.BASS_WASAPI_Start())
            {
                BASSError error = Bass.BASS_ErrorGetCode();
                throw new WASAPIInitializationException("Unable to start WASAPI!", error);
            }

            Console.WriteLine("WASAPI device initialized");
            _deviceNumber = _wasapiDeviceIndex;
            _sampleFrequency = devInfo.mixfreq;
            BuildLookupTables();
            _deviceInitialized = true;
        }

        private void BuildLookupTables()
        {
            _maximumFrequencyIndex = Math.Min(Utils.FFTFrequency2Index(_maximumFrequency, 8192, _sampleFrequency) + 1, 4095);
            _minimumFrequencyIndex = Math.Min(Utils.FFTFrequency2Index(_minimumFrequency, 8192, _sampleFrequency), 4095);
            _freqVolScalar[0] = 1f;
            for (int i = 1; i < BAR_COUNT; i++)
            {
                _logIndex[i] = (int)((Math.Log(BAR_COUNT, BAR_COUNT) - Math.Log(BAR_COUNT - i, BAR_COUNT)) * (_maximumFrequencyIndex - _minimumFrequencyIndex) + _minimumFrequencyIndex);
                _freqVolScalar[i] = 1 + (float)Math.Sqrt((double)i / (double)BAR_COUNT) * 1.25f;
            }
        }

        private void GetBarData()
        {
            //Get FFT data
            BassWasapi.BASS_WASAPI_GetData(_fftData, (int)_maxFft);
            int barIndex = 0;
            //Calculate bar values by squaring fftData from log(x) fft bin, and multiply by a few magic values to end up with a somewhat reasonable graphical representation of the sound
            for (barIndex = 0; barIndex < BAR_COUNT; barIndex++)
            {
                _barValues[barIndex] = ((float)Math.Sqrt(_fftData[_logIndex[barIndex]]) * 15f * VolumeScalar) * _freqVolScalar[barIndex];
            }
            barIndex = 0;

            //This chunk of code is supposed to do a rolling average to smooth out the lower values to look cleaner for another visualizer i'm working on
            float preScaled;

            float preScaled1 = _barValues[barIndex];
            preScaled1 += _barValues[barIndex + 1];
            preScaled1 /= 2f;
            BarValues[barIndex] = Utility.Clamp(preScaled1, 0f, 1f);

            barIndex++;

            preScaled1 = _barValues[barIndex - 1] * 0.75f;
            preScaled1 += _barValues[barIndex];
            preScaled1 += _barValues[barIndex + 1] * 0.75f;
            preScaled1 /= 2.5f;
            BarValues[barIndex] = Utility.Clamp(preScaled1, 0f, 1f);

            for (barIndex = 2; barIndex < 50; barIndex++)
            {
                preScaled = _barValues[barIndex - 2] * 0.5f;
                preScaled += _barValues[barIndex - 1] * 0.75f;
                preScaled += _barValues[barIndex];
                preScaled += _barValues[barIndex + 1] * 0.75f;
                preScaled += _barValues[barIndex + 2] * 0.5f;
                preScaled /= 3.5f;
                BarValues[barIndex] = Utility.Clamp(preScaled, 0f, 1f);
            }
            for (barIndex = 50; barIndex < 999; barIndex++)
            {
                preScaled = _barValues[barIndex - 1] * 0.75f;
                preScaled += _barValues[barIndex];
                preScaled += _barValues[barIndex + 1] * 0.75f;
                preScaled /= 2.5f;
                BarValues[barIndex] = Utility.Clamp(preScaled, 0f, 1f);
            }
            preScaled = _barValues[barIndex - 1];
            preScaled += _barValues[barIndex];
            preScaled /= 2f;
            BarValues[barIndex] = Utility.Clamp(preScaled, 0f, 1f);

            //Calculate the song beat
            float sum = 0f;
            for (int i = 2; i < 28; i++)
                sum += (float)Math.Sqrt(_barValues[i]); //Prettier scaling > Accurate scaling

            SongBeat = (sum / 25f);
        }

        private static int IgnoreDataProc(IntPtr buffer, int length, IntPtr user)
        {
            return 1;
        }
    }
}
