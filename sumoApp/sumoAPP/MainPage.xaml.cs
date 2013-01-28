using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace SumoApp
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : SumoApp.Common.LayoutAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // Restore values stored in session state.
            if (pageState != null && pageState.ContainsKey("item0"))
            {

                int lCount = pageState.Count;
                for (int i = 0; i < lCount; i++)
                {
                    uListofRestaurants.Items.Add(pageState["item"+i]);
                }
            }

            // Restore values stored in app data.
            Windows.Storage.ApplicationDataContainer lRoamingSettings =
                Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (lRoamingSettings.Values.ContainsKey("AddNewTextBoxData"))
            {
                uAddNewTextBox.Text = lRoamingSettings.Values["AddNewTextBoxData"].ToString();
            }
            
            if (lRoamingSettings.Values.ContainsKey("Selected"))
            {
               int lIndex = Int32.Parse(lRoamingSettings.Values["Selected"].ToString());
                if ( uListofRestaurants.Items.Count > lIndex)
                {
                    uListofRestaurants.SelectedIndex = lIndex; 
                }
            }


        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            if (uListofRestaurants.Items.Count > 0)
            {
                int lCount = 0;
                foreach (string s in uListofRestaurants.Items)
                {
                    pageState["item" + lCount] = s;
                    lCount++;
                }
            }
        }


        private void uNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings =
               Windows.Storage.ApplicationData.Current.RoamingSettings;
            //  roamingSettings.Values["userName"] = nameInput.Text;
        }

        private void uAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            string lNewRestaurantName = uAddNewTextBox.Text;
            if (lNewRestaurantName != string.Empty)
            {
                uListofRestaurants.Items.Add(lNewRestaurantName);

            }
            uAddNewTextBox.Text = string.Empty;
            List<string> lRestaurants = uListofRestaurants.Items.Cast<string>().ToList();
            saveRestaurants(lRestaurants);



            UserControl addNewRestaurantFlyoutControl = new UserControl();
            addNewRestaurantFlyoutControl.Content.
            
            ShowPopup(uAddNewButton, blah); 
        }
        public static Popup ShowPopup(FrameworkElement source, UserControl control)
        {
            Popup flyout = new Popup();

            var windowBounds = Window.Current.Bounds;
            var rootVisual = Window.Current.Content;

            GeneralTransform gt = source.TransformToVisual(rootVisual);

            var absolutePosition = gt.TransformPoint(new Point(0, 0));

            control.Measure(new Size(Double.PositiveInfinity, double.PositiveInfinity));

            flyout.VerticalOffset = absolutePosition.Y - control.Height - 10;
            flyout.HorizontalOffset = (absolutePosition.X + source.ActualWidth / 2) - control.Width / 2;
            flyout.IsLightDismissEnabled = true;

            flyout.Child = control;
            var transitions = new TransitionCollection();
            transitions.Add(new PopupThemeTransition() { FromHorizontalOffset = 0, FromVerticalOffset = 100 });
            flyout.ChildTransitions = transitions;
            flyout.IsOpen = true;

            return flyout;
        }
        private void uRandomButton_OnClick(object sender, RoutedEventArgs e)
        {
            SelectRandomRestaurants();
        }
        private void saveRestaurants(List<string> aRestaurants)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        }
        private void SelectRandomRestaurants()
        {
            int lSelection = 0;
            Random lRand = new Random();
            lSelection = lRand.Next(uListofRestaurants.Items.Count);
            uListofRestaurants.SelectedIndex = lSelection;
            List<string> lRestaurants = uListofRestaurants.Items.Cast<string>().ToList();
            saveRestaurants(lRestaurants);
        }

        //async void WriteTimestamp()
        //{
        //    Windows.Globalization.DateTimeFormatting.DateTimeFormatter formatter =
        //        new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("longtime");

        //    StorageFile sampleFile = await roamingFolder.CreateFileAsync("dataFile.txt",
        //        CreateCollisionOption.ReplaceExisting);
        //    await FileIO.WriteTextAsync(sampleFile, formatter.Format(DateTime.Now));
        //}

        private void uRemoveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var lSelectedItem = uListofRestaurants.SelectedIndex;
            if (lSelectedItem != -1)
            {
                uListofRestaurants.Items.RemoveAt(lSelectedItem);
            }

            uAddNewTextBox.Text = string.Empty;

            List<string> lRestaurants = uListofRestaurants.Items.Cast<string>().ToList();
            //   saveRestaurants(lRestaurants); 

        }

        private void uAddnew_TextChanged(object sender, TextChangedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer lRoamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            if (uAddNewTextBox.Text != String.Empty)
            {
                lRoamingSettings.Values["AddNewTextBoxData"] = uAddNewTextBox.Text;
            }

            lRoamingSettings.Values["Selected"] = uListofRestaurants.SelectedIndex;

        }


        private void uListOfRestaurants_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            SelectRandomRestaurants();
        }
    }
}