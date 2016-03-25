namespace CUE.NET.Devices.Keyboard.Enums
{
    /// <summary>
    /// Contains list of all brush calculation modes.
    /// </summary>
    public enum BrushCalculationMode
    {
        /// <summary>
        /// The calculation rectangle for brushes will be the rectangle around the keygroup the brush is applied to.
        /// </summary>
        Relative,
        /// <summary>
        /// The calculation rectangle for brushes will always be the whole keyboard.
        /// </summary>
        Absolute
    }
}
