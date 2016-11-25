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
        public LightView()
        {
            this.lights = new ObservableCollection<Light>();
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
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
            getLights();
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
                light.color = null;
                connector.changestate(light, false);
            }
        }

       

        public async void getLights()
        {
            
            if (connector != null && connector.room != null && connector.room.username != null)
            {
                var value2 = await connector.RetrieveLights();
                lights.Clear();
                foreach (var light in JSONParser.getLights(value2))
                    lights.Add(light);
                
            }
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
            Frame.Navigate(typeof(ColorPickerPage), new object[] {light, connector});
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }

    
}
