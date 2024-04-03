using System;

namespace PackagingBoxes
{
    /// <summary>
    /// Class ListViewData
    /// </summary>
    public class ListViewData
    {
        #region Properties
        public int No { get; set; }
        public Box Box { get; set; }
        public DateTime DateAdded { get; set; }
        public int Quantity { get; set; }
        #endregion

        #region Constructor ListViewData
        public ListViewData(int no, Box box, DateTime dateAdded, int quantity)
        {
            No = no;
            Box = box;
            DateAdded = dateAdded;
            Quantity = quantity;
        }
        #endregion
    }
}
