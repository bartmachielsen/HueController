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
        private List<Light> lights;
      

        public Light light
        {
            get { return lights.ElementAt(0); }
        }

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
                lights = new List<Light>() { (Light)((object[])e.Parameter)[0]};
            if (((object[]) e.Parameter)[0] is List<Light>)
            {
                lights = (List<Light>) ((object[]) e.Parameter)[0];
                if (lights.Count == 0)
                {
                    Frame.Navigate(typeof(LightView), connector.room);
                    return;
                }
            }

            if (lights == null || lights.Count == 0)
                Frame.Navigate(typeof(LightView), connector.room);
            connector = (HueConnector)((object[])e.Parameter)[1];
        }


        private void SettingsClick(object sender, RoutedEventArgs e)
        {
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
            
            Frame.Navigate(typeof(LightView), connector.room);
        }

        private void sliderValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var slider = (Slider) sender;
            TitleBlock.Foreground = new SolidColorBrush(ColorUtil.getColor((int)HueSlider.Value, (int)SaturationSlider.Value, (int)ValueSlider.Value));
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LightView), connector.room);
        }

        private async void Addroom_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private async void AnimationClicked(object sender, RoutedEventArgs e)
        {
            executeAnimations();
            Frame.Navigate(typeof(LightView), connector.room);
        }

        public async void executeAnimations()
        {
            if (this.ComboBox.SelectedItem == null)
                return;
            switch (ComboBox.SelectedIndex)
            {
                case 0:
                    AnimationHandler.ExecuteAnimation(new AllRandomAnimation(connector),lights );
                    break;
                case 1:
                    AnimationHandler.ExecuteAnimation(new BlackWhiteAnimation(connector), lights);
                    break;
            }
            
            
        }

        private async void ChangeName(object sender, RoutedEventArgs e)
        {
            var changer = new NameChanger(lights.ElementAt(0).name);
            await changer.ShowAsync();
            string input = changer.getInput();
            lights.ElementAt(0).name = input;
            connector.changename(lights.ElementAt(0));
        }
    }
}
