using System;
using System.Collections.Generic;
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
        private List<Light> lights;

        public ColorPickerPage()
        {
            this.InitializeComponent();
            GradientStopCollection collection = new GradientStopCollection();
            for (int i = 0; i < 100; i++)
            {
                collection.Add(new GradientStop() {Color= ColorUtil.getColor(655.35* i,254,254), Offset = i/100.0});
            }
            
            HueSlider.Background = new LinearGradientBrush()
            {
                GradientStops = collection
            };
            
            
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
            if(((object[])e.Parameter)[0] is Light)
                light = (Light)((object[])e.Parameter)[0];
            if (((object[]) e.Parameter)[0] is List<Light>)
            {
                lights = (List<Light>) ((object[]) e.Parameter)[0];
                if (lights.Count == 0)
                {
                    Frame.Navigate(typeof(LightView), connector.room);
                    return;
                }
                light = lights.First();
            }

            if (light == null && lights == null)
                Frame.Navigate(typeof(LightView), connector.room);
            connector = (HueConnector)((object[])e.Parameter)[1];
        }


        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void ApplyClick(object sender, RoutedEventArgs e)
        {

            if (lights != null)
            {
                foreach (var light2 in lights)
                {
                    light2.state.hue = (int)HueSlider.Value;
                    light2.state.sat = (int)SaturationSlider.Value;
                    light2.state.bri = (int)ValueSlider.Value;
                    if (connector != null)
                    {
                        connector.changestate(light2, true);
                    }
                }
                
            }
            else if(light != null)
            {
                light.state.hue = (int) HueSlider.Value;
                light.state.sat = (int) SaturationSlider.Value;
                light.state.bri = (int) ValueSlider.Value;
                if (connector != null)
                {
                    connector.changestate(light, true);
                }
            }
            Frame.Navigate(typeof(LightView), connector.room);
            
        }

        private void sliderValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var slider = (Slider) sender;
            TitleBlock.Foreground = new SolidColorBrush(ColorUtil.getColor((int)HueSlider.Value, (int)SaturationSlider.Value, (int)ValueSlider.Value));
        
        }

        private void ChangeName(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
