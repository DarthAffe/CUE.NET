namespace CUE.NET.Devices.Generic.EventArgs
{
    public class UpdatingEventArgs : System.EventArgs
    {
        #region Properties & Fields

        public float DeltaTime { get; }

        #endregion

        #region Constructors

        public UpdatingEventArgs(float deltaTime)
        {
            this.DeltaTime = deltaTime;
        }

        #endregion
    }
}
