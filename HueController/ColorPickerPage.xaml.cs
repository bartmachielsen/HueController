using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HueController.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HueController
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ColorPickerPage : Page
    {
        private HueConnector connector;
        private Light light;

        public ColorPickerPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }

        private void HomepageClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LightView));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            light = (Light)((object[])e.Parameter)[0];
            if(light == null)
                Frame.Navigate(typeof(LightView));
            connector = (HueConnector)((object[])e.Parameter)[1];
        }


        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void ApplyClick(object sender, RoutedEventArgs e)
        {
           
            if (light != null)
            {
                light.state.hue = (int)HueSlider.Value;
                light.state.sat = (int)SaturationSlider.Value;
                light.state.bri = (int)ValueSlider.Value;
                if (connector != null)
                {
                    connector.changestate(light, true);
                }
            }
            Frame.Navigate(typeof(LightView), connector.room);
            
        }
    }
}
