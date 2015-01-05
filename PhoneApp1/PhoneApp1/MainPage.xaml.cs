using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp1.Resources;
using System.Collections.ObjectModel;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Popups;


namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        string thisPodObjectType = "audio";
        public ObservableCollection<PodCast> itemsViewSource = new ObservableCollection<PodCast>();
        private IMobileServiceTable<PodCast> PodCastTable = App.MobileService.GetTable<PodCast>();
        public ObservableCollection<PodCast> playListView
        { get { return itemsViewSource; } }
        // Constructor
        public MainPage()
        {
            InitializeComponent();
			GetPlayListItemsFromMobileService();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }
        
        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        private void ItemListView_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //Code here to open the next ui
            PodCast selectedItem = ((sender as LongListSelector).SelectedItem as PodCast);
           // AudioPlaybackAgent1.AudioPlayer audio = new AudioPlaybackAgent1.AudioPlayer();
            //AudioPlaybackAgent1.AudioPlayer.listOfUrls;
            GetUrlsFromPodCastItems(ref AudioPlaybackAgent1.AudioPlayer.listOfUrls, itemsViewSource, selectedItem);
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("yo"))
            {
                settings["yo"] = AudioPlaybackAgent1.AudioPlayer.listOfUrls;
            }else{
                settings.Add("yo", AudioPlaybackAgent1.AudioPlayer.listOfUrls);
            }            
            settings.Save();
            NavigationService.Navigate(new Uri("/MusicSeed.xaml", UriKind.Relative));
        }

        /*protected override void OnNavigatedTo( System.Windows.Navigation.NavigationEventArgs e) 
        { 
            AudioPlayer.
        }*/
        private void GetUrlsFromPodCastItems(ref List<String> listOfUrls, Collection<PodCast> podCastItems, PodCast selectedPodCast)
        {
            listOfUrls = new List<String>();
            PodCast finalPodItem;
            String prependString = "texttext";
            foreach(var podItem in podCastItems)
            {
                
                if (podItem.Content != null)
                {
                    //If selected add at beginning
                    if (podItem.Url.Equals(selectedPodCast.Url))
                    {
                        if (!podItem.Type.Equals(Constants.TypeText)) //Add only if it not is text
                        {
                            listOfUrls.Insert(0, podItem.Content);
                        }
                        else
                        {
                            listOfUrls.Insert(0, prependString + podItem.Content);
                        }
                        //Add to the beginning

                    }
                    else if (!podItem.Type.Equals(Constants.TypeText)) //Add only if it not is text
                    {
                        listOfUrls.Add(podItem.Content);
                    }
                    else
                    {
                        listOfUrls.Add(prependString + podItem.Content);
                    }
                }
            }
            //return listOfUrls;
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string thisText = ExtractTitleFromWebpage(TodoInput.Text);
            var podItem = new PodCast { Url = TodoInput.Text, Type = thisPodObjectType, Text=thisText , Id = "1", Content = "", Complete = false };
            InsertPodItem(podItem);
            ChangeUrlTextboxVisibility(System.Windows.Visibility.Collapsed);
        }
        private void InsertPodItem(PodCast podItem)
        {
            SetIconForPodItem(ref podItem);            
			SendPodObjectToMobileService(podItem);
            GetPlayListItemsFromMobileService();
            if(itemsViewSource.Count==1 && itemsViewSource[0].Id.Equals("-1"))
            {
                itemsViewSource.RemoveAt(0);
            }
            itemsViewSource.Add(podItem);
        }
		private void SetIconForPodItem(ref PodCast podItem)
		{
			switch (podItem.Type)
            {
                case Constants.TypeAudio: podItem.Icon = Constants.IconAudio; break;
                case Constants.TypeVideo: podItem.Icon = Constants.IconVideo; break;
                case Constants.TypeText: podItem.Icon = Constants.IconText; break;
            }
		}
        private void TodoInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonSave.IsEnabled = true;
        }
        private void Image_AudioTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            thisPodObjectType = Constants.TypeAudio;
			
            ChangeUrlTextboxVisibility(System.Windows.Visibility.Visible);
        }
        private void Image_VideoTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            thisPodObjectType = Constants.TypeVideo;
            ChangeUrlTextboxVisibility(System.Windows.Visibility.Visible);
        }
        private void Image_TextTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            thisPodObjectType = Constants.TypeText;
            ChangeUrlTextboxVisibility(System.Windows.Visibility.Visible);
        }

        private void ChangeUrlTextboxVisibility(System.Windows.Visibility visibility)
        {
            this.ButtonSave.Visibility = visibility;
            this.TodoInput.Visibility = visibility;
            /*if (visibility == System.Windows.Visibility.Visible)
            {
                this.ButtonSave.Height = 10;
                this.TodoInput.Height = 10;
                ButtonSave.IsEnabled = false;
            }
            else
            {
                this.ButtonSave.Height = 0;
                this.TodoInput.Height = 0;
              ButtonSave.IsEnabled = false;
              
            }*/
        }
		private async void  SendPodObjectToMobileService(PodCast podItem)
		{
            try
            {
               await PodCastTable.InsertAsync(podItem);
            }
            catch(Exception e)
            {

            }
		}

		private async void  GetPlayListItemsFromMobileService()
		{
            MobileServiceInvalidOperationException exception = null;
            itemsViewSource=new ObservableCollection<PodCast>();
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
                // The query excludes completed TodoItems
                var podItemsFromService = await PodCastTable
                    .Where(todoItem => todoItem.Complete == false)
                    .ToCollectionAsync();
                foreach (var podItem in podItemsFromService)
                {
                    PodCast tempItem = podItem;
                    SetIconForPodItem(ref tempItem);
                    itemsViewSource.Add(tempItem);
                }
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
            }

			if(itemsViewSource.Count == 0)
			{
				itemsViewSource.Add(new PodCast { Id="-1", Text = "No Playlists Present", Url = "Click on an icon above to add a URL" });
			}

            itemListView.ItemsSource = playListView;
            
		}
        private string ExtractTitleFromWebpage(string url)
        {
            //Function to get title froma  webpage url - Abhik, delete below two lines after its done
            string text = url;
            return text;
        }
		
    }
}