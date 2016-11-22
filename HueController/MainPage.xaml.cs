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
                this.lights = JSONParser.getLights(value2);
            }
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }
        

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Light light = (Light)((Button)sender).DataContext;
            light.state.on = !light.state.on;
            ((Button)sender).Content = light.statetext;


            connector.changestate(light);
        }

        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {
            Light light = (Light)((Button)sender).DataContext;
            var random = new Random();
            var button = ((Button)sender);
            button.Foreground = new SolidColorBrush(new Color() {A = (byte)random.Next(255), B = (byte)random.Next(255), G = (byte)random.Next(255), R = 255});
        }

        private void HomepageClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }
    }

    
}
