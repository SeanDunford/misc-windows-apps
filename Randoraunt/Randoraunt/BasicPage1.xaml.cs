using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Randoraunt
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        /// 
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // Restore values stored in session state.
            if (pageState != null && pageState.ContainsKey("ListOfRestaurants"))
            {
                

            // Restore values stored in app data.
            Windows.Storage.ApplicationDataContainer roamingSettings =
                Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey("userName"))
            {
                //nameInput.Text = roamingSettings.Values["userName"].ToString();
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        private void uAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            string lNewRestaurantName = uAddNewTextBox.Text;
            if (lNewRestaurantName != string.Empty)
            {
                uListofRestaurants.Items.Add(lNewRestaurantName);

            }
            uAddNewTextBox.Text = string.Empty;
            //// Create the message dialog and set its content and title
            //var messageDialog = new MessageDialog("Please enter the name of the new restaurant", "Add a new restaurant");

            //// Add commands and set their command ids

            //messageDialog.Commands.Add(new UICommand("Add", null, 0));
            //messageDialog.Commands.Add(new UICommand("Cancel", null, 1));


            //// Set the command that will be invoked by default
            //messageDialog.DefaultCommandIndex = 1;

            //// Show the message dialog and get the event that was invoked via the async operator
            //var commandChosen = await messageDialog.ShowAsync();

        }

        private void uRandomButton_OnClick(object sender, RoutedEventArgs e)
        {
            int lSelection = 0;
            Random lRand = new Random();
            lSelection = lRand.Next(uListofRestaurants.Items.Count);
            uListofRestaurants.SelectedIndex = lSelection;
            List<string> lRestaurants = uListofRestaurants.Items.Cast<string>().ToList();
            saveRestaurants(lRestaurants);
        }
        private void saveRestaurants(List<string> aRestaurants)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["ListOfRestaurants"] = aRestaurants;

        }
    }
}
