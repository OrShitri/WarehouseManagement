namespace PackagingBoxes
{
    /// <summary>
    /// Class Link
    /// </summary>
    public class Link
    {
        #region Properties
        public BoxesManagement Data { get; set; }
        public string Key { get; set; }
        public Link? Next { get; set; }

        private const double FROM_CENTIMETER_TO_METER = 100;
        #endregion

        #region Constructor Link
        public Link(BoxesManagement queueBox)
        {
            Data = queueBox;

            Key = ((queueBox.Box.X / FROM_CENTIMETER_TO_METER).ToString("F") +
                "*" + (queueBox.Box.Y / FROM_CENTIMETER_TO_METER).ToString("F"));
        }
        #endregion

        #region Empty Constructor Link - For Json
        public Link()
        {

        }
        #endregion

        #region Override ToString
        public override string ToString()
        {
            return Data.ToString();
        }
        #endregion
    }
}
