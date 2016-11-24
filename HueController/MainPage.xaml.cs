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
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Light> lights = new ObservableCollection<Light>();
        private HueConnector connector;
        public MainPage()
        {
            this.lights = new ObservableCollection<Light>();
            this.InitializeComponent();
            loadvalues();   
        }


        public async void loadvalues()
        {
            connector = new HueConnector();
            tryGetUsername();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }
       
        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {
            Light light = (Light)((Button)sender).DataContext;
            var random = new Random();
            var button = ((Button)sender);
            Color color = new Color()
            {
                A = 255,
                B = (byte) random.Next(255),
                G = (byte) random.Next(255),
                R = (byte) random.Next(255)
            };
            button.Background = new SolidColorBrush(color);

            double hue, sat, bri;
            ColorUtil.getHSVFromColor(color, out hue, out sat,out bri);
            light.state.hue = (int)hue;
            light.state.bri = (int) bri;
            light.state.sat = (int) sat;

            connector.changestate(light);
        }

        private void HomepageClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
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
                light.color = null;
                connector.changestate(light, false);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            tryGetUsername();
        }

        public async void tryGetUsername()
        {
            if (connector.username == null)
            {
                var usernameresponse = await connector.getUsername("HueController");
                if (usernameresponse == null)
                {
                    connectionDied();
                    return;
                }
                connector.username = JSONParser.getUsername(usernameresponse);
            }
            if (connector.username != null)
            {
                var value2 = await connector.RetrieveLights();
                if (value2 == null)
                {
                    connectionDied();
                    return;
                }
                lights.Clear();
                foreach (var light in JSONParser.getLights(value2))
                {
                    lights.Add(light);
                }
            }
            else
            {
                await new MessageDialog("Please press HueBox Button").ShowAsync();
                tryGetUsername();
            }
        }

        public async void connectionDied()
        {
            await new MessageDialog("No Connection found with HueBox").ShowAsync();
            tryGetUsername();
        }

        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            tryGetUsername();
        }
    }

    
}
