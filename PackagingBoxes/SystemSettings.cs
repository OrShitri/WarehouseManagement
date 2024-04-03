namespace PackagingBoxes
{
    /// <summary>
    /// Class SystemSettings
    /// </summary>
    public class SystemSettings
    {
        #region Properties
        public int MaximumAmount { get; set; } = 100;
        public double DistanceLimit { get; set; } = 100;
        public int ValidInDays { get; set; } = 24;
        #endregion
    }
}
