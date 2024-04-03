namespace PackagingBoxes
{
    /// <summary>
    /// Class Box
    /// </summary>
    public class Box
    {
        #region Properties
        public int X { get; set; }
        public int Y { get; set; }
        #endregion

        #region Constructor Box
        public Box(int x, int y)
        {
            X = x;
            Y = y;
        }
        #endregion

        #region Override ToString
        public override string ToString()
        {
            return X + "*" + Y;
        }
        #endregion
    }
}
