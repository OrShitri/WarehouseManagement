using System;
using System.Collections.Generic;

namespace PackagingBoxes
{
    /// <summary>
    /// Class BoxesManagement
    /// </summary>
    public class BoxesManagement
    {
        #region Properties
        public Queue<Box> QueueBox = new Queue<Box>();
        public Box Box { get; set; }
        public int Quantity { get; set; }
        public DateTime ValidInDays { get; set; } = DateTime.Now;
        public bool BoxBought { get; set; } = false;
        #endregion

        #region Constructor Queue
        public BoxesManagement(int quantity, Box box)
        {
            Quantity = quantity;

            for (int i = 0; i < Quantity; i++)
                QueueBox.Enqueue(box);

            Box = box;
        }
        #endregion

        #region Override ToString
        public override string ToString()
        {
            return Box.ToString();
        }
        #endregion
    }
}
