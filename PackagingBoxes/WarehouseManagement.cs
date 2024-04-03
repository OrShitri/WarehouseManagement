using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Threading;

namespace PackagingBoxes
{
    /// <summary>
    /// Class WarehouseManagement
    /// </summary>
    public class WarehouseManagement
    {
        private SystemSettings _setDefinition;
        private LinkedList _linkList;
        private bool _pathNoFound = false;
        private DispatcherTimer _timer = new DispatcherTimer();
        private List<ListViewData> _myListOfListView = new List<ListViewData>();


        #region Constructor WarehouseManagement
        public WarehouseManagement()
        {
            _setDefinition = new SystemSettings();
            _linkList = new LinkedList();

            _linkList.UpdateMaxAmount(_setDefinition.MaximumAmount);

            _linkList.UpdateDistanceLimit(_setDefinition.DistanceLimit);

            _linkList.UpdateValidInDays(_setDefinition.ValidInDays);

            _timer.Tick += _timer_Tick;
            _timer.IsEnabled = true;
        }
        #endregion

        /// <summary>
        /// Timer that checks every T time the packages that have expired and deletes them
        /// </summary>
        private void _timer_Tick(object? sender, EventArgs e)
        {
            _timer.Interval = TimeSpan.FromDays(_setDefinition.ValidInDays);

            if (_linkList.IsNoEmpty())
                _linkList.LookForExpiredBox(TimeSpan.FromDays(_setDefinition.ValidInDays));
        }

        #region Updating The Inventory Of Boxes
        /// <summary>
        /// Adds the quantity of boxes by size to the linked list
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="quantity">Quantity</param>
        public void UpdatingTheInventoryOfBoxes(int width, int height, int quantity)
        {
            _linkList.Insert(new BoxesManagement(quantity, new Box(width, height)));
        }

        /// <summary>
        /// Gets the number for the purpose of calculating the difference between the quantity and the maximum amount
        /// </summary>
        /// <returns>Difference between the quantity and the maximum amount</returns>
        public int DistanceBetweenMaxAmountAndQuantity()
        {
            return _linkList.ReturnNumResult();
        }

        /// <summary>
        /// Validation on the quantity against the maximum quantity
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <returns>True if the amount is smaller or lower than the maximum, otherwise - False</returns>
        public bool IsValidQuantity(int quantity)
        {
            if (quantity <= _setDefinition.MaximumAmount)
                return true;

            return false;
        }
        #endregion

        #region System Settings
        /// <summary>
        /// Update system settings
        /// </summary>
        /// <param name="setSystem">Specific definition</param>
        /// <param name="number">Value to update</param>
        public void SystemSettings(string setSystem, int number)
        {
            if (setSystem == "MaxAmount")
            {
                _setDefinition.MaximumAmount = number;
                _linkList.UpdateMaxAmount(number);
            }

            if (setSystem == "DistanceLimit")
            {
                _setDefinition.DistanceLimit = number;
                _linkList.UpdateDistanceLimit(number);
            }

            if (setSystem == "ValidInDays")
            {
                _setDefinition.ValidInDays = number;
                _linkList.UpdateValidInDays(number);
            }
        }

        /// <summary>
        /// Save data of the boxes
        /// </summary>
        public void SaveData()
        {
            Save save = new Save();

            save.LinkedListSave = _linkList.GetHeadLink();
            save.Settings = _setDefinition;

            string jsonString = JsonSerializer.Serialize(save);
            File.WriteAllText("MyFile.json", jsonString);
        }

        /// <summary>
        ///  Load data of the boxes, and update system settings
        /// </summary>
        public void LoadData()
        {
            Save load = new Save();

            if (System.IO.File.Exists("MyFile.json"))
            {
                string fileName = "MyFile.json";
                string jsonString = File.ReadAllText(fileName);

                load = JsonSerializer.Deserialize<Save>(jsonString)!;

                _linkList.LoadHeadLink(load.LinkedListSave!);
                _setDefinition = load.Settings!;

                UpdateSetDefinitionInLinkedList(_setDefinition);
            }
            else
                _pathNoFound = true;
        }

        /// <summary>
        /// Checks if the file is in path
        /// </summary>
        /// <returns>True if the file is not found otherwise False</returns>
        public bool FileToLoadNoFound()
        {
            return _pathNoFound;
        }

        /// <summary>
        /// Update system settings to LinkedList after load data
        /// </summary>
        /// <param name="setDefinition">System Setting from load</param>
        private void UpdateSetDefinitionInLinkedList(SystemSettings setDefinition)
        {
            _linkList.UpdateMaxAmount(setDefinition.MaximumAmount);
            _linkList.UpdateDistanceLimit(setDefinition.DistanceLimit);
            _linkList.UpdateValidInDays(setDefinition.ValidInDays);
        }
        #endregion

        #region Displaying Box Data
        /// <summary>
        /// Gets the quantity of boxes to display by size
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns>Quantity of boxes</returns>
        public int DisplayingBoxData(int width, int height)
        {
            return _linkList.GetQuantity(width, height);
        }

        /// <summary>
        /// Gets data from LinkedList to list of ListViewData
        /// </summary>
        /// <returns>List of ListViewData</returns>
        public List<ListViewData> GetAllDataInLinkedList()
        {
            _myListOfListView = _linkList.GetListViewData();
            return _myListOfListView;
        }

        /// <summary>
        /// Gets data from LinkedList to ListViewData list for boxes that have not been bought T time
        /// </summary>
        /// <param name="numDays">Time of T parameter in days</param>
        /// <returns>List of ListViewData</returns>
        public List<ListViewData> SearchBoxesNoBuy(int numDays)
        {
            _myListOfListView = _linkList.GetListViewDataNoBuy(numDays);
            return _myListOfListView;
        }
        #endregion

        #region Choosing Boxes To Buy
        /// <summary>
        /// Updates the maximum number of splits
        /// </summary>
        /// <param name="maxSplits">Max Splits</param>
        public void UpdateMaxSplits(int maxSplits)
        {
            _linkList.UpdateMaxSplits(maxSplits);
        }

        /// <summary>
        /// Gets a message with the quantity of boxes by splits
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="quantity">Quantity</param>
        /// <returns>String Message</returns>
        public string ChoosingBoxesToBuy(int width, int height, int quantity)
        {
            return _linkList.MessageResult(width, height, quantity);
        }

        /// <summary>
        /// After clicking confirm buying, delete the relevant boxes from the linked list
        /// </summary>
        public void ToDeleteClickConfirm()
        {
            _linkList.DeleteBoxes();
        }
        #endregion
    }
}
