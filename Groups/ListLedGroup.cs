// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using CUE.NET.Devices;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;

namespace CUE.NET.Groups
{
    /// <summary>
    /// Represents a ledgroup containing arbitrary LEDs.
    /// </summary>
    public class ListLedGroup : AbstractLedGroup
    {
        #region Properties & Fields

        protected override ILedGroup EffectTarget => this;

        /// <summary>
        /// Gets the list containing the LEDs of this ledgroup.
        /// </summary>
        protected IList<CorsairLed> GroupLeds { get; } = new List<CorsairLed>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListLedGroup"/> class.
        /// </summary>
        /// <param name="device">The device this ledgroup belongs to.</param>
        /// <param name="autoAttach">Specifies whether this ledgroup should be automatically attached or not.</param>
        public ListLedGroup(ICueDevice device, bool autoAttach = true)
            : base(device, autoAttach)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListLedGroup"/> class.
        /// </summary>
        /// <param name="device">The device this ledgroup belongs to.</param>
        /// <param name="leds">The initial LEDs of this ledgroup.</param>
        public ListLedGroup(ICueDevice device, params CorsairLed[] leds)
            : this(device, true, leds)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListLedGroup"/> class.
        /// </summary>
        /// <param name="device">The device this ledgroup belongs to.</param>
        /// <param name="leds">The initial LEDs of this ledgroup.</param>
        public ListLedGroup(ICueDevice device, IEnumerable<CorsairLed> leds)
            : this(device, true, leds)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListLedGroup"/> class.
        /// </summary>
        /// <param name="device">The device this ledgroup belongs to.</param>
        /// <param name="autoAttach">Specifies whether this ledgroup should be automatically attached or not.</param>
        /// <param name="leds">The initial LEDs of this ledgroup.</param>
        public ListLedGroup(ICueDevice device, bool autoAttach, IEnumerable<CorsairLed> leds)
            : base(device, autoAttach)
        {
            AddLeds(leds);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListLedGroup"/> class.
        /// </summary>
        /// <param name="device">The device this ledgroup belongs to.</param>
        /// <param name="autoAttach">Specifies whether this ledgroup should be automatically attached or not.</param>
        /// <param name="leds">The initial LEDs of this ledgroup.</param>
        public ListLedGroup(ICueDevice device, bool autoAttach, params CorsairLed[] leds)
            : base(device, autoAttach)
        {
            AddLeds(leds);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListLedGroup"/> class.
        /// </summary>
        /// <param name="device">The device this ledgroup belongs to.</param>
        /// <param name="leds">The IDs of the initial LEDs of this ledgroup.</param>
        public ListLedGroup(ICueDevice device, params CorsairLedId[] leds)
            : this(device, true, leds)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListLedGroup"/> class.
        /// </summary>
        /// <param name="device">The device this ledgroup belongs to.</param>
        /// <param name="leds">The IDs of the initial LEDs of this ledgroup.</param>
        public ListLedGroup(ICueDevice device, IEnumerable<CorsairLedId> leds)
            : this(device, true, leds)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListLedGroup"/> class.
        /// </summary>
        /// <param name="device">The device this ledgroup belongs to.</param>
        /// <param name="autoAttach">Specifies whether this ledgroup should be automatically attached or not.</param>
        /// <param name="leds">The IDs of the initial LEDs of this ledgroup.</param>
        public ListLedGroup(ICueDevice device, bool autoAttach, params CorsairLedId[] leds)
            : base(device, autoAttach)
        {
            AddLeds(leds);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListLedGroup"/> class.
        /// </summary>
        /// <param name="device">The device this ledgroup belongs to.</param>
        /// <param name="autoAttach">Specifies whether this ledgroup should be automatically attached or not.</param>
        /// <param name="leds">The IDs of the initial LEDs of this ledgroup.</param>
        public ListLedGroup(ICueDevice device, bool autoAttach, IEnumerable<CorsairLedId> leds)
            : base(device, autoAttach)
        {
            AddLeds(leds);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the given LED(s) to the ledgroup.
        /// </summary>
        /// <param name="LEDs">The LED(s) to add.</param>
        public void AddLed(params CorsairLed[] LEDs)
        {
            AddLeds(LEDs);
        }

        /// <summary>
        /// Adds the given LED(s) to the ledgroup.
        /// </summary>
        /// <param name="ledIds">The ID(s) of the LED(s) to add.</param>
        public void AddLed(params CorsairLedId[] ledIds)
        {
            AddLeds(ledIds);
        }

        /// <summary>
        /// Adds the given LEDs to the ledgroup.
        /// </summary>
        /// <param name="leds">The LEDs to add.</param>
        public void AddLeds(IEnumerable<CorsairLed> leds)
        {
            if (leds != null)
                foreach (CorsairLed led in leds)
                    if (led != null && !ContainsLed(led))
                        GroupLeds.Add(led);
        }

        /// <summary>
        /// Adds the given LEDs to the ledgroup.
        /// </summary>
        /// <param name="ledIds">The IDs of the LEDs to add.</param>
        public void AddLeds(IEnumerable<CorsairLedId> ledIds)
        {
            if (ledIds != null)
                foreach (CorsairLedId ledId in ledIds)
                    AddLed(Device[ledId]);
        }

        /// <summary>
        /// Removes the given LED(s) from the ledgroup.
        /// </summary>
        /// <param name="leds">The LED(s) to remove.</param>
        public void RemoveLed(params CorsairLed[] leds)
        {
            RemoveLeds(leds);
        }

        /// <summary>
        /// Removes the given LED(s) from the ledgroup.
        /// </summary>
        /// <param name="ledIds">The ID(s) of the LED(s) to remove.</param>
        public void RemoveLed(params CorsairLedId[] ledIds)
        {
            RemoveLeds(ledIds);
        }

        /// <summary>
        /// Removes the given LEDs from the ledgroup.
        /// </summary>
        /// <param name="leds">The LEDs to remove.</param>
        public void RemoveLeds(IEnumerable<CorsairLed> leds)
        {
            if (leds != null)
                foreach (CorsairLed led in leds)
                    if (led != null)
                        GroupLeds.Remove(led);
        }

        /// <summary>
        /// Removes the given LEDs from the ledgroup.
        /// </summary>
        /// <param name="ledIds">The IDs of the LEDs to remove.</param>
        public void RemoveLeds(IEnumerable<CorsairLedId> ledIds)
        {
            if (ledIds != null)
                foreach (CorsairLedId ledId in ledIds)
                    RemoveLed(Device[ledId]);
        }

        /// <summary>
        /// Checks if a given LED is contained by this ledgroup.
        /// </summary>
        /// <param name="led">The LED which should be checked.</param>
        /// <returns><c>true</c> if the LED is contained by this ledgroup; otherwise, <c>false</c>.</returns>
        public bool ContainsLed(CorsairLed led)
        {
            return led != null && GroupLeds.Contains(led);
        }

        /// <summary>
        /// Checks if a given LED is contained by this ledgroup.
        /// </summary>
        /// <param name="ledId">The ID of the LED which should be checked.</param>
        /// <returns><c>true</c> if the LED is contained by this ledgroup; otherwise, <c>false</c>.</returns>
        public bool ContainsLed(CorsairLedId ledId)
        {
            return ContainsLed(Device[ledId]);
        }

        /// <summary>
        /// Merges the LEDs from the given ledgroup in this ledgroup. 
        /// </summary>
        /// <param name="groupToMerge">The ledgroup to merge.</param>
        public void MergeLeds(ILedGroup groupToMerge)
        {
            foreach (CorsairLed led in groupToMerge.GetLeds())
                if (!GroupLeds.Contains(led))
                    GroupLeds.Add(led);
        }

        /// <summary>
        /// Gets a list containing the LEDs from this group.
        /// </summary>
        /// <returns>The list containing the LEDs.</returns>
        public override IEnumerable<CorsairLed> GetLeds()
        {
            return GroupLeds;
        }

        #endregion
    }
}
