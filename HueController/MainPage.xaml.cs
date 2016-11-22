using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public MainPage()
        {
            this.lights = new ObservableCollection<Light>()
            {
                new Light(0),
                new Light(1),
                new Light(2),
                new Light(3),
                new Light(4),
                new Light(5),
                new Light(6)
            };
            this.InitializeComponent();
            loadvalues();
        }

        public async void loadvalues()
        {
            HueConnector connector = new HueConnector();
            var usernameresponse = await connector.getUsername("HUEController");
            connector.username = JSONParser.getUsername(usernameresponse);
            var value2 = await connector.RetrieveLights();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }
        

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var button = ((Button) sender);
            button.Content = (String)button.Content == "ON" ? "OFF" : "ON";
        }

        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {
            var random = new Random();
            var button = ((Button)sender);
            button.Foreground = new SolidColorBrush(new Color() {A = (byte)random.Next(255), B = (byte)random.Next(255), G = (byte)random.Next(255), R = 255});
        }
    }
}
