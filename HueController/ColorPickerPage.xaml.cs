using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using HueController.Models.Animations;

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

        private void sliderValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var slider = (Slider) sender;
            TitleBlock.Foreground = new SolidColorBrush(ColorUtil.getColor((int)HueSlider.Value, (int)SaturationSlider.Value, (int)ValueSlider.Value));
        }

        private async void AnimationClicked(object sender, RoutedEventArgs e)
        {
            RandomAnimation random = new RandomAnimation();
            SmoothAnimation smooth = new SmoothAnimation();
            ColorswitchAnimation colorswitch = new ColorswitchAnimation();
            BlinkAnimation blink = new BlinkAnimation();
            if (this.ComboBox.SelectedItem != null)
            {
                Frame.Navigate(typeof(LightView), connector.room);
                string animation = this.ComboBox.SelectedItem.ToString();
                switch (animation)
                {
                    case "Random animation":
                        var randomcolors = random.Animate();
                        foreach (int[] c in randomcolors)
                        {
                            light.state.hue = c[0];
                            light.state.sat = c[1];
                            light.state.bri = c[2];
                            await Task.Delay(1000);
                        }
                        break;
                    case "Smooth animation":
                        var smoothcolors = smooth.Animate();
                        foreach (int[] c in smoothcolors)
                        {
                            light.state.hue = c[0];
                            light.state.sat = c[1];
                            light.state.bri = c[2];
                            await Task.Delay(100);
                        }
                        break;
                    case "Colorswitch animation":
                        var colorswitchcolors = colorswitch.Animate();
                        foreach (int[] c in colorswitchcolors)
                        {
                            light.state.hue = c[0];
                            light.state.sat = c[1];
                            light.state.bri = c[2];
                            await Task.Delay(1000);
                        }
                        break;
                    case "Blink animation":
                        var blinkcolors = blink.Animate();
                        foreach (int[] c in blinkcolors)
                        {
                            light.state.hue = c[0];
                            light.state.sat = c[1];
                            light.state.bri = c[2];
                            await Task.Delay(1000);
                        }
                        break;
                }
            }
        }
    }
}
