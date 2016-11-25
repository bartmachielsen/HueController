using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HueController.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HueController
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    ///
    public sealed partial class RoomView : Page
    {
        public ObservableCollection<Room> rooms = new ObservableCollection<Room>();
        
        public RoomView()
        {
            InitializeComponent();
            rooms.Add(new Room(0, "Simulator","127.0.0.1", 80));
            rooms.Add(new Room(1, "Simulator demo", "127.0.0.1", 80));
        }

        private void EnlargeButton_OnClick(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }

        private async void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Room room = rooms.ElementAt(((Room)((Button) sender).DataContext).id);
            string username = await getUsername(room);
            if (username != null && username != "-1")
            {
                room.username = username;
                Frame.Navigate(typeof(LightView), room);
            }
            else if (username == null)
            {
                ((Button) sender).Flyout = new Flyout() {Content = new TextBlock() {Text = "Druk op de HueBoxKnop!"}};
            }
            else
            {
                ((Button)sender).Flyout = new Flyout() { Content = new TextBlock() { Text = "Geen verbinding beschikbaar!" } };
            }
        }

        public async Task<string> getUsername(Room room)
        {
            HueConnector connector = new HueConnector(room);
            if (room.username == null)
            {
                System.Diagnostics.Debug.WriteLine("Getting username");
                var usernameresponse = await connector.getUsername("HueController");
                if (usernameresponse == null)
                {
                    return "-1";
                }
                return JSONParser.getUsername(usernameresponse);
            }
            else
            {
                return room.username;
            }
        }
        private void Homepage_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RoomView));
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
