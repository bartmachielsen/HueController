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
using Windows.Web.Http;
using HueController.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HueController
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LightView : Page
    {
        private ObservableCollection<Light> lights;
        private Room room;
        private HueConnector connector;
        private bool select = false;
        public LightView()
        {
            this.lights = new ObservableCollection<Light>();
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            if (e.Parameter == null && connector != null && connector.room != null)
            {
                return;
            }
            if (e.Parameter is Room)
            {
                room = (Room) e.Parameter;
                connector = room.getConnector();

            }
            lights = room.lights;
            if (! await getLights())
            {
                Frame.Navigate(typeof(RoomView), $"{((Room)e.Parameter).name} is not reachable!");
            }
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }
       
        

        private void HomepageClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RoomView), room);
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
        }
        
        private void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            Light light = (Light)((ToggleSwitch)sender).DataContext;
            if (light != null && light.state != null)
            {
                light.state.on = !light.state.on;
                connector.changestate(light, false);
                light.updateAll("color");
            }
        }

       

        public async Task<bool> getLights()
        {
            if (connector != null && connector.room != null && connector.room.username != null && connector.room.username != "")
            {
                var value2 = await connector.RetrieveLights();
                lights = room.lights;
                var locallights = JSONParser.getLights(value2);
                if (locallights == null)
                    return false;

                foreach (var locallight in locallights)
                {
                    bool merged = false;
                    foreach (var light in lights)
                    {
                        if (light.merge(locallight))
                        {
                            merged = true;
                            break;
                        }
                    }
                    if (!merged)
                        lights.Add(locallight);
                }
                room.lights = lights;
                return true;
            }
            return false;
        }

        

        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            getLights();
        }

        private void Frame1_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private async void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            
            Light light = (Light)(((Grid)sender).DataContext);
            if (select)
            {
                light.selected = !light.selected;
                if (light.selected)
                {
                    ((Grid)sender).BorderBrush = new SolidColorBrush(Colors.DarkBlue);
                    ((Grid)sender).BorderThickness = new Thickness(5,5,5,5);
                }
                else
                {
                    ((Grid)sender).BorderThickness = new Thickness(2, 2, 2, 2);
                    ((Grid) sender).BorderBrush = new SolidColorBrush(Colors.Black);
                }
            }
            else
            {

                var picker = new ColorChangeDialog(new object[] { light, connector });
                await picker.ShowAsync();
            }
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

      
        private void SelectMore(object sender, RoutedEventArgs e)
        {
            select = !select;
            ApllyAll.Visibility = @select ? Visibility.Visible : Visibility.Collapsed;
            foreach (var light in lights)
            {
                light.selected = false;
            }
        }

        private async void ApllyAll_OnClick(object sender, RoutedEventArgs e)
        {
           List<Light> lightsfiltered = new List<Light>(lights);
           lightsfiltered.RemoveAll((Light light) => !light.selected);
           var picker = new ColorChangeDialog(new object[] { lightsfiltered, connector });
           await picker.ShowAsync();
        }

        private void RandomColors(object sender, RoutedEventArgs e)
        {
            var random = new Random();
            foreach (var light in lights)
            {
                light.state.hue = random.Next(65535);
                light.state.sat = random.Next(254);
                light.state.bri = random.Next(154)+100;
                connector.changestate(light);
                light.updateAll("color");
            }
            
        }

        private void RandomNames(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (!localSettings.Values.ContainsKey("randomnames"))
                localSettings.Values["randomnames"] = "";
            string[] randomnamen = ((String) localSettings.Values["randomnames"]).Split(',');
            if (randomnamen.Length == 0)
                return;

            foreach (var light in lights)
            {
                light.name = randomnamen[random.Next(randomnamen.Length)];
                connector.changename(light);
                light.updateAll("name");
                

            }
        }
        private string[] getRandomNames()
        {
            Random random = new Random();
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (!localSettings.Values.ContainsKey("randomnames"))
                localSettings.Values["randomnames"] = "";
            return ((String)localSettings.Values["randomnames"]).Split(',');
        }
        private string getRandomName(string[] randomnamen = null)
        {
            Random random = new Random();
            if(randomnamen == null)
                randomnamen = getRandomNames();
            return randomnamen[random.Next(randomnamen.Length)];
        }

        private void KillConnectionBridge(object sender, RoutedEventArgs e)
        {
            killConnection();
        }

        public async void killConnection()
        {
            List<Task> tasks = new List<Task>();
            foreach (var light in lights)
            {
                tasks.Add(BruteForceLight(light));
            }
            foreach (var task in tasks)
            {
                
                if (task.IsCompleted || task.IsCanceled || task.IsFaulted)
                {
                    await new MessageDialog("BruteForcing finished (timed out)").ShowAsync();
                    return;
                }
                    
            }
        }

        public async Task<string> BruteForceLight(Light light)
        {
            Random random = new Random();
            string[] randomnames = getRandomNames();
            HueConnector connector = this.connector.room.getConnector();
            while (true)
            {
                
                light.state.on = !light.state.on;
                light.updateAll("state");

                if (random.Next(10) >= 5)
                {
                    light.name = getRandomName(randomnames);
                    await connector.changename(light);
                    light.updateAll("name");
                }
                light.state.hue = random.Next(65535);
                light.state.sat = random.Next(254);
                light.state.bri = random.Next(154) + 100;
                light.updateAll("color");
                string response = await connector.changestate(light, false);
                if (response == null)
                {
                    return "";
                }
            }
        }

        private async void Changeevery(object sender, RoutedEventArgs e)
        {
            var picker = new ColorChangeDialog(new object[] { new List<Light>(lights), connector });
            await picker.ShowAsync();
        }
    }

    
}
