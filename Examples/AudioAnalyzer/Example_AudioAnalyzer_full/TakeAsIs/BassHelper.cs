using System;
using System.Collections.Generic;
using Un4seen.Bass;
using Un4seen.BassWasapi;

namespace Example_AudioAnalyzer_full.TakeAsIs
{
    public static class BassHelper
    {
        public static void InitializeBass()
        {
            BassNet.Registration("trial@trial.com", "2X1837515183722");
            BassNet.OmitCheckVersion = true;
            if (!Bass.LoadMe())
                throw new WASAPIInitializationException("Unable to load bass.dll!");

            if (!Bass.BASS_Init(0, 48000, 0, IntPtr.Zero))
                throw new WASAPIInitializationException("Unable to initialize the BASS library!");

            if (!BassWasapi.LoadMe())
                throw new WASAPIInitializationException("Unable to load BassWasapi.dll!");

            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 0);
        }

        public static List<Tuple<string, int>> GetBassDevices()
        {
            List<Tuple<string, int>> deviceList = new List<Tuple<string, int>>();

            BASS_WASAPI_DEVICEINFO[] devices = BassWasapi.BASS_WASAPI_GetDeviceInfos();
            for (int i = 0; i < devices.Length; i++)
            {
                BASS_WASAPI_DEVICEINFO device = devices[i];
                if (!device.IsEnabled || !device.SupportsRecording) continue;

                string deviceName = device.name;
                if (device.IsLoopback)
                    deviceName += " (Loopback)";

                deviceList.Add(new Tuple<string, int>(deviceName, i));
            }

            return deviceList;
        }
    }
}
