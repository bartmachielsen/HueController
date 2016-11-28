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
    public sealed partial class LightView : Page
    {
        private ObservableCollection<Light> lights = new ObservableCollection<Light>();
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
                Room room = (Room) e.Parameter;
                connector = new HueConnector(room);
                
                
            }
            if (! await getLights())
            {
                Frame.Navigate(typeof(RoomView), connector.room);
            }
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }
       
        

        private void HomepageClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RoomView));
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
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
                lights.Clear();
                var locallights = JSONParser.getLights(value2);
                if (locallights == null)
                    return false;
           
                foreach (var light in locallights)
                    lights.Add(light);
                connector.room.lights = lights;
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

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            
            Light light = (Light)(((Grid)sender).DataContext);
            if (select)
            {
                light.selected = !light.selected;
                if (light.selected)
                {
                    ((Grid) sender).Background = new SolidColorBrush(Colors.Transparent);
                }
                else
                {
                    ((Grid) sender).Background = light.color;
                }
            }
            else
            {

                Frame.Navigate(typeof(ColorPickerPage), new object[] {light, connector});
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

        private void ApllyAll_OnClick(object sender, RoutedEventArgs e)
        {
           List<Light> lightsfiltered = new List<Light>(lights);
           lightsfiltered.RemoveAll((Light light) => !light.selected);
            Frame.Navigate(typeof(ColorPickerPage), new object[] { lightsfiltered, connector });
        }

        private void RandomColors(object sender, RoutedEventArgs e)
        {
            foreach (var light in lights)
            {
                var random = new Random();
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
            System.Diagnostics.Debug.WriteLine(randomnamen.Length);
            if (randomnamen.Length == 0)
                return;

            foreach (var light in lights)
            {
                light.name = randomnamen[random.Next(randomnamen.Length)];
                connector.changename(light);
                light.updateAll("name");
                

            }
        }
    }

    
}
