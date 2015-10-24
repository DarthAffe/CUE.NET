/* 
 * ###################################################################################################################
 * #                                             Thanks to AterialDawn                                               #
 * #                                                                                                                 #
 * # Source: https://github.com/AterialDawn/CUEAudioVisualizer/blob/master/CUEAudioVisualizer/Exceptions/WASAPIInitializationException.cs  #
 * #   Date: 23.10.2015                                                                                              #
 * # Commit: d415fd1dda2e7fd98391ac83c000d7e4a781558e                                                                #
 * #                                                                                                                 #
 * ###################################################################################################################
 * 
 * > Note < This code is refactored and all parts not used in this tutorial are removed.
 */

using System;
using Un4seen.Bass;

namespace Example_AudioAnalyzer_full.TakeAsIs
{
    public class WASAPIInitializationException : Exception
    {
        public BASSError? OptionalError { get; private set; }

        public WASAPIInitializationException(string message) : base(message) { OptionalError = null; }
        public WASAPIInitializationException(string message, BASSError error) : base(message) { OptionalError = error; }
    }
}
