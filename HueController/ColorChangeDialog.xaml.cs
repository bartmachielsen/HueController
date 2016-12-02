using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using HueController.Models;
using HueController.Models.Animations;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HueController
{
    public sealed partial class ColorChangeDialog : ContentDialog
    {
        private readonly HueConnector connector;
        private readonly List<Light> lights;

        public ColorChangeDialog(object e)
        {
            InitializeComponent();
           
            var collection = new GradientStopCollection();

            for (var i = 0; i < 100; i++)
                collection.Add(new GradientStop {Color = ColorUtil.getColor(655.35*i, 254, 254), Offset = i/100.0});

            HueSlider.Background = new LinearGradientBrush
            {
                GradientStops = collection
            };

            if (((object[]) e)[0] is Light)
                lights = new List<Light> {(Light) ((object[]) e)[0]};
            if (((object[]) e)[0] is List<Light>)
                lights = (List<Light>) ((object[]) e)[0];

            connector = (HueConnector) ((object[]) e)[1];
        }

        public Light light
        {
            get { return lights.ElementAt(0); }
        }

       
        


        private void SettingsClick(object sender, RoutedEventArgs e)
        {
        }

        private void ApplyClick(object sender, RoutedEventArgs e)
        {
            if (lights != null)
                foreach (var light2 in lights)
                {
                    light2.state.hue = (int) HueSlider.Value;
                    light2.state.sat = (int) SaturationSlider.Value;
                    light2.state.bri = (int) ValueSlider.Value;
                    light2.updateAll("color");
                    if (connector != null)
                        connector.changestate(light2, true);
                }
        }

        private void sliderValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var slider = (Slider) sender;
           
        }

       
        private async void AnimationClicked(object sender, RoutedEventArgs e)
        {
            
        }

        public async void executeAnimations()
        {
            if (ComboBox.SelectedItem == null)
                return;
            switch (ComboBox.SelectedIndex)
            {
                case 1:
                    AnimationHandler.ExecuteAnimation(new AllRandomAnimation(connector), lights);
                    break;
                case 2:
                    AnimationHandler.ExecuteAnimation(new BlackWhiteAnimation(connector), lights);
                    break;
                case 3:
                    AnimationHandler.ExecuteAnimation(new RandomAnimation(connector), lights );
                    break;
                case 4:
                    AnimationHandler.ExecuteAnimation(new ColorLoop((int)HueSlider.Value, lights.Count, connector), lights);
                    break;
                case 5:
                    AnimationHandler.ExecuteAnimation(new ColorLoopBack((int)HueSlider.Value, lights.Count, connector), lights);
                    break;
            }
        }

      
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (lights.Count == 1 && NameBox.Text != lights.ElementAt(0).name)
            {
                lights.ElementAt(0).name = NameBox.Text;
                connector.changename(lights.ElementAt(0));
            }
            if (ComboBox.SelectedItem != null && ComboBox.SelectedIndex > 0)
                executeAnimations();

            Hide();
        }


        private void ColorChangeDialog_OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            
        }
    }
}

