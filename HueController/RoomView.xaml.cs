using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Sms;
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
            loadFromSave();
        }

        public void loadFromSave()
        {
            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("rooms"))
            {
                System.Diagnostics.Debug.WriteLine("Loaded info!");
                var list = ((string) localSettings.Values["rooms"]).Split(',');
                foreach (var item in list)
                {
                    var splitted = item.Split(':');
                    string username = null;
                    if (splitted.Length > 3)
                    {
                        username = splitted[3];
                        System.Diagnostics.Debug.WriteLine("Loaded username " + username);
                    }
                    rooms.Add(new Room(rooms.Count, splitted[0], splitted[1], Int32.Parse(splitted[2]), username));
                }
            }
            else
            {
                saveRooms(localSettings);
            }
        }

        public void saveRooms(Windows.Storage.ApplicationDataContainer localSettings = null)
        {
            if(localSettings == null) {
                localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            }
            List<string> ser = new List<string>();
            foreach (var room in rooms)
            {
                ser.Add($"{room.name}:{room.addres}:{room.port}:{room.username}");
            }
            localSettings.Values["rooms"] = string.Join(",", ser);
        }

        private void EnlargeButton_OnClick(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }
        
        private async void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Room room = ((Room)((Grid) sender).DataContext);
            string username = await getUsername(room);
            if (username != null && username != "-1" && username != "")
            {
                room.username = username;
                saveRooms();
                Frame.Navigate(typeof(LightView), room);
            }
            else if (username == null)
            {
                //((Button) sender).Flyout = new Flyout() {Content = new TextBlock() {Text = "Druk op de HueBoxKnop!"}};
                await new MessageDialog($"You need to push the link button on the HueBox of {room.name}!", "Action Required").ShowAsync();
            }
            else
            {
                //((Grid)sender). = new Flyout() { Content = new TextBlock() { Text = "Geen verbinding beschikbaar!" } };
                await new MessageDialog($"{room.name} is not reachable!", "Connection error").ShowAsync();
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

        private async void Addroom_OnClick(object sender, RoutedEventArgs e)
        {
            var creater = new RoomCreate();
            var result = await creater.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                rooms.Add(new Room(rooms.Count, creater.getInputted()[0], creater.getInputted()[1],
                    Int32.Parse(creater.getInputted()[2])));

                saveRooms();
            }
        }

        private async void changeButton(object sender, RoutedEventArgs e)
        {
            Room room = (Room) ((Button) sender).DataContext;
            var creater = new RoomCreate(room);
            var result = await creater.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                room.name = creater.getInputted()[0];
                room.addres = creater.getInputted()[1];
                room.port = Int32.Parse(creater.getInputted()[2]);

                saveRooms();
            }
        }
    
    }
}
