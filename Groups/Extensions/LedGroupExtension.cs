// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;

namespace CUE.NET.Groups.Extensions
{
    /// <summary>
    /// Offers some extensions and helper-methods for ledgroup related things.
    /// </summary>
    public static class LedGroupExtension
    {
        /// <summary>
        /// Converts the given <see cref="AbstractLedGroup" /> to a <see cref="ListLedGroup" />.
        /// </summary>
        /// <param name="ledGroup">The <see cref="AbstractLedGroup" /> to convert.</param>
        /// <returns>The converted <see cref="ListLedGroup" />.</returns>
        public static ListLedGroup ToSimpleKeyGroup(this AbstractLedGroup ledGroup)
        {
            ListLedGroup simpleLedGroup = ledGroup as ListLedGroup;
            if (simpleLedGroup == null)
            {
                bool wasAttached = ledGroup.Detach();
                simpleLedGroup = new ListLedGroup(ledGroup.Device, wasAttached, ledGroup.GetLeds()) { Brush = ledGroup.Brush };
            }
            return simpleLedGroup;
        }

        /// <summary>
        /// Returns a new <see cref="ListLedGroup" /> which contains all LEDs from the given ledgroup excluding the specified ones.
        /// </summary>
        /// <param name="ledGroup">The base ledgroup.</param>
        /// <param name="ledIds">The ids of the LEDs to exclude.</param>
        /// <returns>The new <see cref="ListLedGroup" />.</returns>
        public static ListLedGroup Exclude(this AbstractLedGroup ledGroup, params CorsairLedId[] ledIds)
        {
            ListLedGroup simpleLedGroup = ledGroup.ToSimpleKeyGroup();
            foreach (CorsairLedId ledId in ledIds)
                simpleLedGroup.RemoveLed(ledId);
            return simpleLedGroup;
        }

        /// <summary>
        /// Returns a new <see cref="ListLedGroup" /> which contains all LEDs from the given ledgroup excluding the specified ones.
        /// </summary>
        /// <param name="ledGroup">The base ledgroup.</param>
        /// <param name="ledIds">The LEDs to exclude.</param>
        /// <returns>The new <see cref="ListLedGroup" />.</returns>
        public static ListLedGroup Exclude(this AbstractLedGroup ledGroup, params CorsairLed[] ledIds)
        {
            ListLedGroup simpleLedGroup = ledGroup.ToSimpleKeyGroup();
            foreach (CorsairLed led in ledIds)
                simpleLedGroup.RemoveLed(led);
            return simpleLedGroup;
        }

        // ReSharper disable once UnusedMethodReturnValue.Global
        /// <summary>
        /// Attaches the given ledgroup to the device.
        /// </summary>
        /// <param name="ledGroup">The ledgroup to attach.</param>
        /// <returns><c>true</c> if the ledgroup could be attached; otherwise, <c>false</c>.</returns>
        public static bool Attach(this AbstractLedGroup ledGroup)
        {
            return ledGroup.Device?.AttachLedGroup(ledGroup) ?? false;
        }

        /// <summary>
        /// Detaches the given ledgroup from the device.
        /// </summary>
        /// <param name="ledGroup">The ledgroup to attach.</param>
        /// <returns><c>true</c> if the ledgroup could be detached; otherwise, <c>false</c>.</returns>
        public static bool Detach(this AbstractLedGroup ledGroup)
        {
            return ledGroup.Device?.DetachLedGroup(ledGroup) ?? false;
        }
    }
}
