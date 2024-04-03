using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PackagingBoxes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WarehouseManagement warehouseManagement;
        bool systemSettingIsEnabled = false;
        public List<ListViewData> MyListOfListViewData { get; set; }


        #region Constructor MainWindow
        public MainWindow()
        {
            InitializeComponent();
            warehouseManagement = new WarehouseManagement();
        }
        #endregion

        #region Updating The Inventory Of Boxes
        /// <summary>
        /// Clears Fields In Updating The Inventory Of Boxes
        /// </summary>
        private void BtnUpdatClear_Click(object sender, RoutedEventArgs e)
        {
            ClearUpdatOrChoosTextBox(TxtUpdatWidth, TxtUpdatHeight, TxtUpdatQuantity, LableUpdatMessage, ImgUpdatV);
        }

        /// <summary>
        /// Validates that there are no empty fields, and adds boxes to the warehouse 
        /// </summary>
        private void BtnUpdatAdd_Click(object sender, RoutedEventArgs e)
        {
            string widthStr = TxtUpdatWidth.Text;
            string heightStr = TxtUpdatHeight.Text;
            string quantityStr = TxtUpdatQuantity.Text;

            bool validationStr = FieldValidation(widthStr, heightStr, quantityStr);

            if (validationStr)
            {
                int width = int.Parse(TxtUpdatWidth.Text);
                int height = int.Parse(TxtUpdatHeight.Text);
                int quantity = int.Parse(TxtUpdatQuantity.Text);

                ValidationToBouttonAdd(width, height, quantity);
            }
            else
            {
                LableUpdatMessage.Content = "All fields must be filled";
                ImgUpdatV.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Validates the fields, and updates boxes to the warehouse
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="quantity">Quantity</param>
        private void ValidationToBouttonAdd(int width, int height, int quantity)
        {
            bool validationInt = FieldValidation(width, height, quantity);

            if (validationInt)
            {
                if (!systemSettingIsEnabled)
                    SystemSettingIsEnabled();

                ClearUpdatOrChoosTextBox(TxtUpdatWidth, TxtUpdatHeight, TxtUpdatQuantity, LableUpdatMessage, ImgUpdatV);

                if (warehouseManagement.IsValidQuantity(quantity))
                {
                    warehouseManagement.UpdatingTheInventoryOfBoxes(width, height, quantity);
                    ShowMessageAndPicPartialAdd(width, height, quantity);
                }
                else
                {
                    warehouseManagement.UpdatingTheInventoryOfBoxes(width, height, quantity);
                    ShowMessageAndPicPartialAdd(width, height, quantity);
                }
            }
            else
            {
                LableUpdatMessage.Content = "Please enter valid values ​​(Min Q=1, Min X&Y=10cm)";
                ImgUpdatV.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// In a partial addition to the warehouse, displays a status message and a V image
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="quantity">Quantity</param>
        private void ShowMessageAndPicPartialAdd(int width, int height, int quantity)
        {
            if (warehouseManagement.DistanceBetweenMaxAmountAndQuantity() == 0)
                ShowMessageAndPicFullAdd(width, height, quantity);
            else
            {
                int num1 = warehouseManagement.DistanceBetweenMaxAmountAndQuantity();
                ShowMessageAndPicFullAdd(width, height, quantity - num1);

                if (quantity - num1 != 0)
                    LableUpdatMessage.Content = LableUpdatMessage.Content + Environment.NewLine + "And " + (num1) + " - boxes returned to the supplier.";
                else
                {
                    LableUpdatMessage.Content = "Sorry,\nIt is not possible to add more boxes of size [" + width + "*" + width + "*" + height + "]cm," + Environment.NewLine + "All " + quantity + " boxes are returned to the supplier.";
                    ImgUpdatV.Visibility = Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// In a full addition to the warehouse, displays a status message and a V image
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="quantity">Quantity</param>
        private void ShowMessageAndPicFullAdd(int width, int height, int quantity)
        {
            LableUpdatMessage.Content = quantity + " - Boxes of size " + width + "*" + height + " [cm]," + Environment.NewLine + "Have been successfully added.";
            ImgUpdatV.Visibility = Visibility;
        }

        /// <summary>
        /// Clears fields and status message and hides image
        /// </summary>
        /// <param name="textBoxWidth">TextBox Width</param>
        /// <param name="textBoxHeight">TextBox Height</param>
        /// <param name="textBoxQuantity">TextBox Quantity</param>
        /// <param name="labelStatusMessage">Label Status Message</param>
        /// <param name="imageV">Image</param>
        private void ClearUpdatOrChoosTextBox(TextBox textBoxWidth, TextBox textBoxHeight, TextBox textBoxQuantity, Label labelStatusMessage, Image imageV)
        {
            textBoxWidth.Text = "";
            textBoxHeight.Text = "";
            textBoxQuantity.Text = "";
            labelStatusMessage.Content = "Status Message";

            if (imageV == ImgUpdatV)
                imageV.Visibility = Visibility.Hidden;

            else
                ChangeImageToApprove(@"Images\Box.png");

        }

        /// <summary>
        /// Validation on the fields that are not empty
        /// </summary>
        /// <param name="width">String Width</param>
        /// <param name="height">String Height</param>
        /// <param name="quantity">String Quantity</param>
        /// <returns>True or False</returns>
        private bool FieldValidation(string width, string height, string quantity)
        {
            if (width != "" && height != "" && quantity != "")
                return true;

            return false;
        }

        /// <summary>
        /// Number range validation
        /// </summary>
        /// <param name="width">Int Width</param>
        /// <param name="height">Int Height</param>
        /// <param name="quantity">Int Quantity</param>
        /// <returns>True or False</returns>
        private bool FieldValidation(int width, int height, int quantity)
        {
            if (width >= 10 && height >= 10 && quantity > 0)
                return true;

            return false;
        }

        #region TextBox In Updating The Inventory Of Boxes
        private void TxtUpdatHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDigit(sender);
        }

        private void TxtUpdatWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDigit(sender);
        }

        private void TxtUpdatQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDigit(sender);
        }
        #endregion

        /// <summary>
        /// Allows typing only numbers in the TextBox
        /// </summary>
        /// <param name="sender">Text from TextBox</param>
        private void IsDigit(object sender)
        {
            string str = ((TextBox)sender).Text;
            for (int i = 0; i < str.Length; i++)
            {
                if (!char.IsDigit(str[i]))
                    str = str.Remove(i, 1);
            }
            ((TextBox)sender).Text = str;
            ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
        }
        #endregion

        #region System Settings
        /// <summary>
        /// Manual Radio Rutton of Max Amount - Displays TextBox and Button
        /// </summary>
        private void RbtnManMaxAmount_Checked(object sender, RoutedEventArgs e)
        {
            TxtMaxAmount.Visibility = Visibility.Visible;
            BtnSetMaxAmount.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///  Manual Radio Rutton of Distance Limit - Displays TextBox and Button and Label
        /// </summary>
        private void RbtnManDisLimit_Checked(object sender, RoutedEventArgs e)
        {
            TxtDisLimit.Visibility = Visibility.Visible;
            LablePerDisLimit.Visibility = Visibility.Visible;
            BtnSetDisLimit.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///  Manual Radio Rutton of Valid In Days - Displays TextBox and Button
        /// </summary>
        private void RbtnManValidDay_Checked(object sender, RoutedEventArgs e)
        {
            TxtValidDay.Visibility = Visibility.Visible;
            BtnSetValidDay.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///  Default Radio Rutton of Max Amount - Clears TextBox and hides TextBox and Button
        /// </summary>
        private void RbtnDefMaxAmount_Click(object sender, RoutedEventArgs e)
        {
            TxtMaxAmount.Visibility = Visibility.Hidden;
            ClearSystemSettingTextBox(TxtMaxAmount);
            BtnSetMaxAmount.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Default Radio Rutton of Distance Limit - Clears TextBox and hides TextBox and Button and Label
        /// </summary>
        private void RbtnDefDisLimit_Click(object sender, RoutedEventArgs e)
        {
            TxtDisLimit.Visibility = Visibility.Hidden;
            LablePerDisLimit.Visibility = Visibility.Hidden;
            ClearSystemSettingTextBox(TxtDisLimit);
            BtnSetDisLimit.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Default Radio Rutton of Valid In Days - Clears TextBox and hides TextBox and Button
        /// </summary>
        private void RbtnDefValidDay_Click(object sender, RoutedEventArgs e)
        {
            TxtValidDay.Visibility = Visibility.Hidden;
            ClearSystemSettingTextBox(TxtValidDay);
            BtnSetValidDay.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Clears the TextBox in the settings
        /// </summary>
        /// <param name="txtBox">TextBox</param>
        private void ClearSystemSettingTextBox(TextBox txtBox)
        {
            txtBox.Text = "";
        }

        #region TextBox In System Settings
        private void TxtMaxAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDigit(sender);
        }

        private void TxtDisLimit_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDigit(sender);
        }

        private void TxtValidDay_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDigit(sender);
        }
        #endregion

        /// <summary>
        /// Updates maximum amount settings, and locks the Radio Button and TextBox after the update
        /// </summary>
        private void BtnSetMaxAmount_Click(object sender, RoutedEventArgs e)
        {
            int num;
            if (TxtMaxAmount.Text != "" && int.Parse(TxtMaxAmount.Text) >= 1)
            {
                num = int.Parse(TxtMaxAmount.Text);
                warehouseManagement.SystemSettings("MaxAmount", num);

                RbtnDefMaxAmount.IsEnabled = false;
                RbtnManMaxAmount.IsEnabled = false;
                TxtMaxAmount.IsEnabled = false;
                BtnSetMaxAmount.IsEnabled = false;
            }
        }

        /// <summary>
        /// Updates distance limit in percentages in settings, and locks the Radio Button and TextBox after the update
        /// </summary>
        private void BtnSetDisLimit_Click(object sender, RoutedEventArgs e)
        {
            int num;
            if (TxtDisLimit.Text != "")
            {
                num = int.Parse(TxtDisLimit.Text);
                warehouseManagement.SystemSettings("DistanceLimit", num);

                RbtnDefDisLimit.IsEnabled = false;
                RbtnManDisLimit.IsEnabled = false;
                TxtDisLimit.IsEnabled = false;
                BtnSetDisLimit.IsEnabled = false;
            }
        }

        /// <summary>
        /// updates valid in days in settings, and locks the Radio Button and TextBox after the update, allowing a maximum value of 24 days
        /// </summary>
        private void BtnSetValidDay_Click(object sender, RoutedEventArgs e)
        {
            int num;
            if (TxtValidDay.Text != "" && int.Parse(TxtValidDay.Text) >= 1 && int.Parse(TxtValidDay.Text) <= 24)
            {
                num = int.Parse(TxtValidDay.Text);
                warehouseManagement.SystemSettings("ValidInDays", num);

                RbtnDefValidDay.IsEnabled = false;
                RbtnManValidDay.IsEnabled = false;
                TxtValidDay.IsEnabled = false;
                BtnSetValidDay.IsEnabled = false;
            }
        }

        /// <summary>
        /// Used to lock settings in case boxes were put into the warehouse
        /// </summary>
        private void SystemSettingIsEnabled()
        {
            systemSettingIsEnabled = true;

            RbtnDefMaxAmount.IsEnabled = false;
            RbtnManMaxAmount.IsEnabled = false;
            TxtMaxAmount.IsEnabled = false;
            BtnSetMaxAmount.IsEnabled = false;

            RbtnDefDisLimit.IsEnabled = false;
            RbtnManDisLimit.IsEnabled = false;
            TxtDisLimit.IsEnabled = false;
            BtnSetDisLimit.IsEnabled = false;

            RbtnDefValidDay.IsEnabled = false;
            RbtnManValidDay.IsEnabled = false;
            TxtValidDay.IsEnabled = false;
            BtnSetValidDay.IsEnabled = false;
        }

        /// <summary>
        /// Saves the data, and displays an appropriate message
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            ImgGifLoad.Visibility = Visibility.Visible;

            warehouseManagement.SaveData();

            string caption = "Save Data";
            string messageBoxText = "The data was saved successfully";

            MessageBoxButton buttons = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, buttons, icon);

            switch (result)
            {
                case MessageBoxResult.OK:
                    ImgGifLoad.Visibility = Visibility.Hidden;
                    break;
            }
        }

        /// <summary>
        /// Loads the data, and displays an appropriate message, and locks the system settings, and shows a loading animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            ImgGifLoad.Visibility = Visibility.Visible;

            string caption = "Load Data";
            string messageBoxText = "Are you sure you want to load data?\nAll other information will be deleted";

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, buttons, icon);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    SystemSettingIsEnabled();
                    warehouseManagement.LoadData();

                    MessageBoxButton buttonsOk = MessageBoxButton.OK;
                    MessageBoxImage iconInfo = MessageBoxImage.Information;

                    if (!warehouseManagement.FileToLoadNoFound())
                        MessageBox.Show("The data has been loaded successfully", caption, buttonsOk, iconInfo);

                    else
                    {
                        string messageBoxTextInCaseNoLoad = "Sorry but the save file was not found," +
                           "\nYou must save the data so that you can load it in the future";
                        MessageBox.Show(messageBoxTextInCaseNoLoad, caption, buttonsOk, iconInfo);
                    }

                    MyList.ItemsSource = null;
                    ImgGifLoad.Visibility = Visibility.Hidden;
                    break;

                case MessageBoxResult.No:
                    ImgGifLoad.Visibility = Visibility.Hidden;
                    break;
            }
        }

        /// <summary>
        /// Button Exit
        /// </summary>
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            string caption = "Exit The Program";
            string messageBoxText = "Are you sure you want to exit the program ?" +
                "\nIf you exit the program all data will be lost";

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, buttons, icon);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    System.Windows.Application.Current.Shutdown();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        #endregion

        #region Displaying Box Data
        #region TextBox In Displaying Box Data
        private void TxtDisplayWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDigit(sender);
        }

        private void TxtDisplayHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDigit(sender);
        }

        private void TxtDisplayDays_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDigit(sender);
        }
        #endregion

        /// <summary>
        /// Clears Fields In Displaying Box Data
        /// </summary>
        private void BtnDisplayClear_Click(object sender, RoutedEventArgs e)
        {
            ClearDisplayTextBox();
        }

        /// <summary>
        /// Clears fields and quantity message
        /// </summary>
        private void ClearDisplayTextBox()
        {
            TxtDisplayWidth.Text = "";
            TxtDisplayHeight.Text = "";
            LableDisplayMessage.Content = "No Info Available";
        }

        /// <summary>
        /// Validates that there are no empty fields, and calculates the number of boxes in the warehouse
        /// </summary>
        private void BtnDisplayCalculate_Click(object sender, RoutedEventArgs e)
        {
            string widthStr = TxtDisplayWidth.Text;
            string heightStr = TxtDisplayHeight.Text;

            bool validationStr = FieldValidation(widthStr, heightStr);

            if (validationStr)
            {
                int width = int.Parse(TxtDisplayWidth.Text);
                int height = int.Parse(TxtDisplayHeight.Text);

                ValidationToBoutton(width, height);
            }
            else
                LableDisplayMessage.Content = "No Info Available";
        }

        /// <summary>
        /// Validation on the fields that are not empty
        /// </summary>
        /// <param name="width">String Width</param>
        /// <param name="height">String Height</param>
        /// <returns>True or False</returns>
        private bool FieldValidation(string width, string height)
        {
            if (width != "" && height != "")
                return true;

            return false;
        }

        /// <summary>
        /// Number range validation
        /// </summary>
        /// <param name="width">Int Width</param>
        /// <param name="height">Int Height</param>
        /// <returns>True or False</returns>
        private bool FieldValidation(int width, int height)
        {
            if (width >= 10 && height >= 10)
                return true;

            return false;
        }

        /// <summary>
        /// Validates the fields, and displays the quantity of boxes
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        private void ValidationToBoutton(int width, int height)
        {
            int quantity;
            bool validationInt = FieldValidation(width, height);

            if (validationInt)
            {
                ClearDisplayTextBox();

                quantity = warehouseManagement.DisplayingBoxData(width, height);
                LableDisplayMessage.Content = "              " + quantity;

            }
            else
                LableDisplayMessage.Content = "Min X&Y=10cm";
        }

        /// <summary>
        /// Displaying the data in ViewList
        /// </summary>
        private void BtnDisplayShowAll_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataToMyList();
            MyList.ItemsSource = MyListOfListViewData;
        }

        /// <summary>
        /// Updates data to list of ListViewData to enable Binding in Xaml
        /// </summary>
        private void UpdateDataToMyList()
        {
            MyListOfListViewData = new List<ListViewData>();

            MyListOfListViewData = warehouseManagement.GetAllDataInLinkedList();

            DataContext = this;
        }

        /// <summary>
        /// Clears the data from the ViewList
        /// </summary>
        private void BtnDisplayClearViewList_Click(object sender, RoutedEventArgs e)
        {
            MyList.ItemsSource = null;
        }

        /// <summary>
        /// Displays data on boxes that have not been bought for more than T time
        /// </summary>
        private void BtnDisplaySeasrch_Click(object sender, RoutedEventArgs e)
        {
            string daysStr = TxtDisplayDays.Text;
            bool validStrFieldDays = FieldValidationToDays(daysStr);

            if (validStrFieldDays)
            {
                LableDisplayMessageDays.Content = "Status Message";
                int numDays = int.Parse(TxtDisplayDays.Text);
                UpdateDataOfNotBoughtToMyList(numDays);
                MyList.ItemsSource = MyListOfListViewData;
            }
            else
                LableDisplayMessageDays.Content = "Please enter valid values. ​​The days field must be filled";
        }

        /// <summary>
        /// Validating a range of numbers greater than 1 and that the field is not empty
        /// </summary>
        /// <param name="maxSplitsStr"></param>
        /// <returns></returns>
        private bool FieldValidationToDays(string days)
        {
            if (days != "" && int.Parse(days) >= 1)
                return true;

            return false;
        }

        /// <summary>
        /// Updates data of boxes not bought T time, to list of ListViewData to enable Xaml binding
        /// </summary>
        /// <param name="numDays">Paramete of time - T</param>
        private void UpdateDataOfNotBoughtToMyList(int numDays)
        {
            MyListOfListViewData = new List<ListViewData>();

            MyListOfListViewData = warehouseManagement.SearchBoxesNoBuy(numDays);

            DataContext = this;
        }
        #endregion

        #region Choosing Boxes To Buy

        #region TextBox In Choosing Boxes To Buy
        private void TxtChoosWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDigit(sender);
        }

        private void TxtChoosHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDigit(sender);
        }

        private void TxtChoosQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDigit(sender);
        }
        #endregion

        /// <summary>
        /// Makes sure there are only numbers, and at least one non - 0 number
        /// </summary>
        /// <param name="sender">Text from TextBox</param>
        private void TxtChoosMaxSplits_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDigit(sender);

            ((TextBox)sender).SelectAll();

            if (((TextBox)sender).Text == "" || ((TextBox)sender).Text == "0")
                ((TextBox)sender).Text = "5";
        }

        /// <summary>
        ///  Clears Fields In Choosing Boxes To Buy, and swich the Button state and the Button text 
        /// </summary>
        private void BtnChoosClear_Click(object sender, RoutedEventArgs e)
        {
            ClearUpdatOrChoosTextBox(TxtChoosWidth, TxtChoosHeight, TxtChoosQuantity, LableChoosMessage, ImgChoosV);

            if (BtnChoosCheck.IsEnabled == false)
            {
                BtnChoosCheck.IsEnabled = !BtnChoosCheck.IsEnabled;
                BtnChoosConfirm.IsEnabled = !BtnChoosConfirm.IsEnabled;

                if (BtnChoosClear.Content == "Cancel")
                    BtnChoosClear.Content = "Clear";
            }
        }

        /// <summary>
        /// Validates that there are no empty fields, and displays the buying options
        /// </summary>
        private void BtnChoosCheck_Click(object sender, RoutedEventArgs e)
        {
            string widthStr = TxtChoosWidth.Text;
            string heightStr = TxtChoosHeight.Text;
            string quantityStr = TxtChoosQuantity.Text;

            bool validationStr = FieldValidation(widthStr, heightStr, quantityStr);

            if (validationStr)
            {
                int width = int.Parse(TxtChoosWidth.Text);
                int height = int.Parse(TxtChoosHeight.Text);
                int quantity = int.Parse(TxtChoosQuantity.Text);

                ValidationToBouttonCheck(width, height, quantity);
            }
            else
            {
                LableChoosMessage.Content = "All fields must be filled";
                ChangeImageToApprove(@"Images\Box.png");
            }
        }

        /// <summary>
        /// Validates the fields, and displaying possible quantities by splits, and swich the Button state and the Button text 
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="quantity">Quantity</param>
        private void ValidationToBouttonCheck(int width, int height, int quantity)
        {
            bool validationInt = FieldValidation(width, height, quantity);
            string strMessage;
            string strNoFound = "Sorry,\nNo suitable size box found.";

            string maxSplitsStr = TxtChoosMaxSplits.Text;
            bool validationMaxSplits = FieldValidationToMaxSplits(maxSplitsStr);

            if (validationInt)
            {
                if (validationMaxSplits)
                {
                    warehouseManagement.UpdateMaxSplits(int.Parse(maxSplitsStr));

                    ClearUpdatOrChoosTextBox(TxtChoosWidth, TxtChoosHeight, TxtChoosQuantity, LableChoosMessage, ImgChoosV);

                    strMessage = warehouseManagement.ChoosingBoxesToBuy(width, height, quantity);

                    LableChoosMessage.Content = strMessage;
                    ChangeImageToApprove(@"Images\Box.png");

                    if (LableChoosMessage.Content != strNoFound)
                    {
                        BtnChoosClear.Content = "Cancel";
                        BtnChoosConfirm.IsEnabled = true;
                        BtnChoosCheck.IsEnabled = false;
                    }
                }
                else
                {
                    LableChoosMessage.Content = "Please enter valid values ​​between 1 and 5";
                    ChangeImageToApprove(@"Images\Box.png");
                }

            }
            else
            {
                LableChoosMessage.Content = "Please enter valid values ​​(Min Q=1, Min X&Y=10cm)";
                ChangeImageToApprove(@"Images\Box.png");
            }
        }

        /// <summary>
        /// Number range validation of a numbers between 1 - 5, in the maximum splits field
        /// </summary>
        /// <param name="maxSplitsStr">Text from TextBox</param>
        /// <returns>True or False</returns>
        private bool FieldValidationToMaxSplits(string maxSplitsStr)
        {
            if (maxSplitsStr != "" && int.Parse(maxSplitsStr) >= 1 && int.Parse(maxSplitsStr) <= 5)
                return true;

            return false;
        }

        /// <summary>
        /// Confirm action button, and swich the Button state and the Button text, and updating the quantities in the warehouse
        /// </summary>
        private void BtnChoosConfirm_Click(object sender, RoutedEventArgs e)
        {
            BtnChoosConfirm.IsEnabled = false;
            BtnChoosCheck.IsEnabled = true;
            BtnChoosClear.Content = "Clear";

            warehouseManagement.ToDeleteClickConfirm();
            LableChoosMessage.Content = LableChoosMessage.Content + "All boxes have been bought successfully.";
            ChangeImageToApprove(@"Images\Approved.png");
        }

        /// <summary>
        /// Changes the image by Path
        /// </summary>
        /// <param name="path">Path to change</param>
        private void ChangeImageToApprove(string path)
        {
            BitmapImage bitmapImage = new BitmapImage();
            UriKind uri = UriKind.Relative;
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(path, uri);
            bitmapImage.EndInit();
            ImgChoosV.Source = bitmapImage;
        }
        #endregion

    }
}
