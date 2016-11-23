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
            var usernameresponse = await connector.getUsername("HUEController");
            connector.username = JSONParser.getUsername(usernameresponse);
            if (connector.username == null)
            {
                // SHOW MESSAGE THAT THE BUTTON MUST BEEN PRESSED AND RESEND
                connector.username = JSONParser.getUsername(usernameresponse);
            }
            else
            {
                var value2 = await connector.RetrieveLights();
                lights.Clear();
                foreach (var light in JSONParser.getLights(value2))
                {
                    lights.Add(light);
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }
       
        private void SelectColor_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ColorPickerPage));
            //Light light = (Light)((Button)sender).DataContext;
            //var random = new Random();
            //var button = ((Button)sender);
            //light.color = new SolidColorBrush(new Color() {A = (byte)random.Next(255), B = (byte)random.Next(255), G = (byte)random.Next(255), R = (byte)random.Next(255) });
            //button.Background = new SolidColorBrush(new Color() { A = (byte)random.Next(255), B = (byte)random.Next(255), G = (byte)random.Next(255), R = (byte)random.Next(255) });
            //connector.changestate(light);
        }

        private void HomepageClick(object sender, RoutedEventArgs e)
        {
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
                connector.changestate(light);
            }
        }
    }

    
}
