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
        public bool busy = false;
        public RoomView()
        {
            InitializeComponent();
            loadFromSave();
            rooms.Add(new SimulatorRoom(rooms.Count));
        }

        public void loadFromSave()
        {
            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("rooms"))
            {
                rooms = JSONParser.getRooms((string)localSettings.Values["rooms"]);
                if (rooms == null)
                {
                    rooms = new ObservableCollection<Room>();
                }
                if (rooms.Count == 0)
                {
                    rooms.Add(new Room(0, "Explora", "145.48.205.33", 80, "iYrmsQq1wu5FxF9CPqpJCnm1GpPVylKBWDUsNDhB"));
                    saveRooms(localSettings);
                }
            }
            else
            {
                saveRooms(localSettings);
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter is Room)
            {
                var cRoom = (Room) e.Parameter;
                foreach (var room in rooms)
                {
                    if (room.id == cRoom.id && room.name == cRoom.name && room.addres == cRoom.addres)
                    {
                        room.lights = cRoom.lights;
                    }

                }
            }else if (e.Parameter is String && e.Parameter != "" && e.Parameter != null)
            {
                await new MessageDialog(e.Parameter +"", "Error").ShowAsync();
            }
            
        }

        public void saveRooms(Windows.Storage.ApplicationDataContainer localSettings = null)
        {
            if(localSettings == null) {
                localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            }

            localSettings.Values["rooms"] = JSONGenerator.rooms(rooms);
        }

        private void EnlargeButton_OnClick(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }
        
        private async void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            
            Room room = ((Room)((Grid) sender).DataContext);
            
            if (busy || !rooms.Contains(room))
                return;
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
                var room = new Room(rooms.Count, creater.getInputted()[0], creater.getInputted()[1],
                    80, creater.getInputted()[3]);
                int port = 80;
                Int32.TryParse(creater.getInputted()[2], out port);
                room.port = 80;
                rooms.Add(room);
                saveRooms();
            }
        }

        private async void ChangeRoom(object sender, RoutedEventArgs e)
        {
            busy = true;
            Room room = (Room) ((Button) sender).DataContext;
            var creater = new RoomCreate(room);
            var result = await creater.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                room.name = creater.getInputted()[0];
                if(!(room is SimulatorRoom))
                {
                    room.addres = creater.getInputted()[1];
                    int port = 80;

                    Int32.TryParse(creater.getInputted()[2], out port);
                    room.port = port;
                    room.username = creater.getInputted()[3];
                    if(room.username == "" || room.username == " ")
                        room.username = null;
                }
                saveRooms();
            }
            busy = false;
        }

        private void DeleteRoom(object sender, RoutedEventArgs e)
        {
            busy = true;
            Room room = (Room)((Button)sender).DataContext;
            rooms.Remove(room);
            saveRooms();
            busy = false;
        }

        private async void AddRandomName(object sender, RoutedEventArgs e)
        {
            var changer =new NameChanger("");
            await changer.ShowAsync();
            string result = changer.getInput();

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("randomnames"))
                localSettings.Values["randomnames"] = localSettings.Values["randomnames"] + "," + result;
            else
            {
                localSettings.Values["randomnames"] = result;
            }
        }
    }
}
