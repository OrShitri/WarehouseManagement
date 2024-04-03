using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PackagingBoxes
{
    /// <summary>
    /// Class LinkedList
    /// </summary>
    public class LinkedList
    {
        private int _maxAmount;
        private double _distanceLimit;
        private int _validInDays;
        private int _maxSplits;
        private const int CRITICAL_QUANTITY = 3;

        private bool _listIsNoEmpty = false;
        private int _numResult = 0;
        private int _saveQuantityOfLastLink = 0;

        private StringBuilder _sbMessage;

        private List<ListViewData> _myListViewData;

        private Link _head;

        private List<Link> _linksToResult;


        public void UpdateMaxAmount(int number)
        {
            _maxAmount = number;
        }

        public void UpdateDistanceLimit(double number)
        {
            _distanceLimit = number;
        }

        public void UpdateValidInDays(int number)
        {
            _validInDays = number;
        }

        public void UpdateMaxSplits(int number)
        {
            _maxSplits = number;
        }

        /// <summary>
        /// Number returned to calculate the difference
        /// </summary>
        /// <returns></returns>
        public int ReturnNumResult()
        {
            return _numResult;
        }

        public bool IsNoEmpty()
        {
            return _listIsNoEmpty;
        }

        /// <summary>
        /// Adds boxes to the list
        /// </summary>
        /// <param name="queueBox">Kind of box to add</param>
        public void Insert(BoxesManagement queueBox)
        {
            _numResult = 0;
            Link link = new Link(queueBox);

            Link tempLink = IsKeyExsit(link.Key);
            if (tempLink != null)
            {
                MovesBoxesBetweenQueues(queueBox, tempLink);
            }

            else
            {
                UpadateHigherQuantityThanMaximum(link, queueBox);
                Insert(link);
            }
        }

        /// <summary>
        /// Search for position for new link
        /// </summary>
        /// <param name="newLink">New link to insert</param>
        private void Insert(Link newLink)
        {
            if (_head == null)
            {
                _listIsNoEmpty = true;
                _head = newLink;
                return;
            }

            if (_head.Key.CompareTo(newLink.Key) != -1)
            {
                newLink.Next = _head;
                _head = newLink;

                return;
            }

            Link link = _head;
            while (link.Next != null && link.Next.Key.CompareTo(newLink.Key) == -1)
            {
                link = link.Next;
            }

            newLink.Next = link.Next;
            link.Next = newLink;
        }

        /// <summary>
        /// Checks if the link exists according to the key
        /// </summary>
        /// <param name="key">key of link</param>
        /// <returns>If the link exists returns the link otherwise returns null</returns>
        private Link IsKeyExsit(string key)
        {
            Link link = _head;

            while (link != null && key != link.Key)
            {
                link = link.Next;
            }

            return link;
        }

        /// <summary>
        /// Moves boxes between 2 queues
        /// </summary>
        /// <param name="queueBox">Queue from which boxes are transferred</param>
        /// <param name="tempLink">The link to which the boxes are transferred</param>
        private void MovesBoxesBetweenQueues(BoxesManagement queueBox, Link tempLink)
        {
            for (int i = 0; i < queueBox.Quantity && tempLink.Data.Quantity < _maxAmount; i++)
            {
                Box b = queueBox.QueueBox.Dequeue();
                tempLink.Data.QueueBox.Enqueue(b);
                tempLink.Data.Quantity++;
                tempLink.Data.ValidInDays = DateTime.Now;
            }
            _numResult = queueBox.QueueBox.Count;
            queueBox = null;
        }

        /// <summary>
        /// Updates the queue quantity to the maximum value
        /// </summary>
        /// <param name="link">The updated link</param>
        /// <param name="queueBox">The updated queue</param>
        private void UpadateHigherQuantityThanMaximum(Link link, BoxesManagement queueBox)
        {
            if (link.Data.Quantity > _maxAmount)
            {
                _numResult = queueBox.QueueBox.Count - _maxAmount;

                link.Data.Quantity = _maxAmount;
                for (; _maxAmount < queueBox.QueueBox.Count;)
                    queueBox.QueueBox.Dequeue();
            }
        }

        /// <summary>
        /// Returns the number of boxes according to size
        /// </summary>
        /// <param name="x">Width </param>
        /// <param name="y">Height</param>
        /// <returns>Quantity</returns>
        public int GetQuantity(int x, int y)
        {
            int quantity;
            string key = (x / 100d).ToString("F") + "*" + (y / 100d).ToString("F");
            Link link = IsKeyExsit(key);

            if (link != null)
            {
                quantity = link.Data.Quantity;
                return quantity;
            }

            return 0;
        }

        /// <summary>
        /// Returns all possible splits recursively according to a distance limit
        /// </summary>
        /// <param name="link">Link head</param>
        /// <param name="x">Width </param>
        /// <param name="y">Height</param>
        /// <returns>Link at the limit of the defined distance</returns>
        public Link RecursiveSearch(Link link, int x, int y)
        {
            string keyStr = (x / 100d).ToString("F") + "*" + (y / 100d).ToString("F");

            if (link == null)
                return null;

            if (link.Key == keyStr || LinkBoolX(link, x, y) || LinkBoolY(link, x, y))
                _linksToResult.Add(link);

            if (_linksToResult.Count == _maxSplits)
                return null;

            return RecursiveSearch(link.Next, x, y);
        }

        /// <summary>
        /// Checks width link
        /// </summary>
        /// <param name="link">Link to check</param>
        /// <param name="x">Width</param>
        /// <param name="y">Height</param>
        /// <returns>Returns True if maximum distance limit is met</returns>
        private bool LinkBoolX(Link link, int x, int y)
        {
            int linkX = link.Data.Box.X;
            int linkY = link.Data.Box.Y;
            bool sortLinkX = (x == linkX) && (y <= linkY && y + y * _distanceLimit / 100 >= linkY);
            return sortLinkX;
        }

        /// <summary>
        /// Checks height link
        /// </summary>
        /// <param name="link">Link to check</param>
        /// <param name="x">Width </param>
        /// <param name="y">Height</param>
        /// <returns>Returns True if maximum distance limit is met</returns>
        private bool LinkBoolY(Link link, int x, int y)
        {
            int linkX = link.Data.Box.X;
            int linkY = link.Data.Box.Y;
            bool sortLinkY = ((x <= linkX) && (x + x * _distanceLimit / 100 >= linkX)) && (y == linkY || (y <= linkY && y + y * _distanceLimit / 100 >= linkY));
            return sortLinkY;
        }

        /// <summary>
        /// Remove From Last Link 
        /// </summary>
        /// <param name="linkTemp">Get Link to Remove</param>
        /// <param name="numReturnHowQuantityInMyLink">How box to remove from link</param>
        private void RemoveFromLink(Link linkTemp, int numReturnHowQuantityInMyLink)
        {
            Link link = _head;

            while (link != null && link != linkTemp)
            {
                link = link.Next;
            }

            for (int i = 0; i < numReturnHowQuantityInMyLink; i++)
            {
                link.Data.QueueBox.Dequeue();
                link.Data.Quantity--;
                link.Data.ValidInDays = DateTime.Now;
                link.Data.BoxBought = true;
            }
            switch (link.Data.Quantity)
            {
                case 0:
                    {
                        DeleteLink(link.Key);
                        string caption = "Last box bought";
                        MessageBoxButton buttonsOk = MessageBoxButton.OK;
                        MessageBoxImage iconInfo = MessageBoxImage.Information;
                        MessageBox.Show("This was the last box of the size - " + link.Data.Box + " [cm]", caption, buttonsOk, iconInfo);
                    }
                    break;
                case <= CRITICAL_QUANTITY:
                    {
                        string caption = "Critical Quantity";
                        MessageBoxButton buttonsOk = MessageBoxButton.OK;
                        MessageBoxImage iconInfo = MessageBoxImage.Information;
                        MessageBox.Show("Note last " + link.Data.Quantity + " in stock - " + link.Data.Box + " [cm]", caption, buttonsOk, iconInfo);
                    }
                    break;
            }
        }

        /// <summary>
        /// Goes through the list of possible splits, calculates and displays a message accordingly
        /// </summary>
        /// <param name="x">Width</param>
        /// <param name="y">Height</param>
        /// <param name="quantity">Quantity</param>
        /// <returns>Message with the split options</returns>
        public string MessageResult(int x, int y, int quantity)
        {
            _linksToResult = new List<Link>();
            RecursiveSearch(_head, x, y);
            _sbMessage = new StringBuilder();

            foreach (Link item in _linksToResult)
            {
                if (quantity <= item.Data.Quantity)
                {
                    _saveQuantityOfLastLink = quantity;
                    _sbMessage.Append(quantity + " Boxes From Size: " + item.Data.Box + " [cm]" + "\n");
                    break;
                }
                else
                {
                    _saveQuantityOfLastLink = item.Data.Quantity;

                    quantity -= item.Data.Quantity;
                    _sbMessage.Append(item.Data.Quantity + " Boxes From Size: " + item.Data.Box + " [cm]" + Environment.NewLine);
                }
            }

            if (_sbMessage.Equals(""))
                return "Sorry,\nNo suitable size box found.";

            return _sbMessage.ToString();
        }

        /// <summary>
        /// Counts the number of splits found to be relevant
        /// </summary>
        /// <param name="str">String of the status message</param>
        /// <returns>Number of splits</returns>
        private int HowSplitsInList(StringBuilder str)
        {
            int numSplit = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '\n')
                    numSplit++;
            }

            return numSplit;
        }

        /// <summary>
        /// Searches for expired boxes and deletes them according to a time parameter, and displays an appropriate message
        /// </summary>
        /// <param name="time">Elapsed time according to a time parameter defined in the settings</param>
        public void LookForExpiredBox(TimeSpan time)
        {
            bool isExpried = false;
            DateTime dateTimeNow = DateTime.Now;
            if (_head != null)
            {
                Link tempLink;
                if (dateTimeNow - _head.Data.ValidInDays >= time)
                {
                    tempLink = _head;
                    _head = _head.Next;

                    string caption = "Box Expired";
                    MessageBoxButton buttonsOk = MessageBoxButton.OK;
                    MessageBoxImage iconInfo = MessageBoxImage.Information;
                    MessageBox.Show("This box has been deleted, \nBecause it has expired - " + tempLink.Data.Box, caption, buttonsOk, iconInfo);

                    if (_head == null)
                        _listIsNoEmpty = false;
                    return;
                }

                Link link = _head.Next;
                Link preLink = _head;
                while (link != null)
                {

                    if (dateTimeNow - link.Data.ValidInDays >= time)
                        isExpried = true;

                    if (isExpried)
                    {
                        tempLink = link;
                        link = link.Next;
                        preLink.Next = link;

                        string caption = "Box Expired";
                        MessageBoxButton buttonsOk = MessageBoxButton.OK;
                        MessageBoxImage iconInfo = MessageBoxImage.Information;
                        MessageBox.Show("This box has been deleted, \nBecause it has expired - " + tempLink.Data.Box, caption, buttonsOk, iconInfo);

                        isExpried = false;
                    }
                    else
                    {
                        preLink = link;
                        link = link.Next;
                    }
                }
            }

            if (_head == null)
                _listIsNoEmpty = false;
        }

        /// <summary>
        /// Deletes the list of links except for the last link
        /// </summary>
        private void Delete()
        {
            int i = 0;
            List<Link> tempListLink = CutListLink();
            int count = tempListLink.Count;
            bool IskeyLinkDelete = false;

            //Generates a temp link in the head to prevent a search in the head link
            Link tempLink = new Link(new BoxesManagement(0, new Box(0, 0)));
            tempLink.Next = _head;
            _head = tempLink;

            Link link = _head.Next;
            Link preLink = _head;

            while (link != null && i != count - 1)
            {
                if (tempListLink[i].Key == link.Key)
                {
                    IskeyLinkDelete = true;
                    preLink.Next = link.Next;

                    string caption = "Last box bought";
                    MessageBoxButton buttonsOk = MessageBoxButton.OK;
                    MessageBoxImage iconInfo = MessageBoxImage.Information;
                    MessageBox.Show("This was the last box of the size - " + tempListLink[i].Data.Box + " [cm]", caption, buttonsOk, iconInfo);
                    i++;
                }

                if (IskeyLinkDelete)
                {
                    IskeyLinkDelete = false;
                    link = link.Next;
                }

                else
                {
                    preLink = link;
                    link = link.Next;
                }
            }

            //Deleting the temp link
            _head = _head.Next;
        }

        /// <summary>
        /// Selects an appropriate deletion function according to the amount of splits
        /// </summary>
        public void DeleteBoxes()
        {
            int numSplit = HowSplitsInList(_sbMessage);
            List<Link> tempListLink = CutListLink();

            if (numSplit >= 1)
            {
                Delete();
                RemoveFromLink(tempListLink.Last(), _saveQuantityOfLastLink);
            }
            else
                RemoveFromLink(tempListLink.First(), _saveQuantityOfLastLink);
        }

        /// <summary>
        /// Deletes a specific link by key
        /// </summary>
        /// <param name="key">Key of link</param>
        private void DeleteLink(string key)
        {
            if (_head.Key == key)
            {
                _head = _head.Next;
                return;
            }

            Link link = _head.Next;
            Link previous = _head;
            while (link.Next != null && link.Key != key)
            {
                previous = link;
                link = link.Next;
            }
            link = link.Next;
            previous.Next = link;
        }

        /// <summary>
        /// Collects data on the entire list for the listview display
        /// </summary>
        /// <returns>List of ListViewData</returns>
        public List<ListViewData> GetListViewData()
        {
            _myListViewData = new List<ListViewData>();
            Link link = _head;
            int i = 1;
            while (link != null)
            {
                _myListViewData.Add(new ListViewData(i, link.Data.Box, link.Data.ValidInDays, link.Data.Quantity));
                i++;
                link = link.Next;
            }

            return _myListViewData;
        }

        /// <summary>
        /// Cuts the list of all splitting options, to the selected relevant number
        /// </summary>
        /// <returns>List of relevant links</returns>
        private List<Link> CutListLink()
        {
            List<Link> tempListLink = new List<Link>();
            int numSplit = HowSplitsInList(_sbMessage);
            for (int i = 0; i < numSplit; i++)
                tempListLink.Add(_linksToResult[i]);

            return tempListLink;
        }

        /// <summary>
        /// Return current head to allow a save
        /// </summary>
        /// <returns>Current head</returns>
        public Link GetHeadLink()
        {
            return _head;
        }

        /// <summary>
        /// Load data to head of LinkedList
        /// </summary>
        /// <param name="head">Current head</param>
        public void LoadHeadLink(Link head)
        {
            _head = head;
        }

        /// <summary>
        /// Collects data on boxes not bought T time, for the list view
        /// </summary>
        /// <param name="numDays">Time of T parameter in days</param>
        /// <returns>List of ListViewData</returns>
        public List<ListViewData> GetListViewDataNoBuy(int numDays)
        {
            _myListViewData = new List<ListViewData>();
            Link link = _head;
            int i = 1;
            while (link != null)
            {
                if (!link.Data.BoxBought || (link.Data.BoxBought && DateTime.Now.AddDays(-numDays) >= link.Data.ValidInDays))
                {
                    _myListViewData.Add(new ListViewData(i, link.Data.Box, link.Data.ValidInDays, link.Data.Quantity));
                    i++;
                }

                link = link.Next;
            }

            return _myListViewData;
        }
    }
}
