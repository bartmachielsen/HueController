using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HueController
{
    public sealed partial class NameChanger : ContentDialog
    {
        public ObservableCollection<RandomName> names;


        public NameChanger(ObservableCollection<RandomName> names)
        {
            this.names = names;
            this.InitializeComponent();
        }


        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (TextBox.Text != "")
            {
                names.Add(new RandomName(TextBox.Text));
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void DeleteName(object sender, RoutedEventArgs e)
        {
            names.Remove((RandomName)((TextBlock)sender).DataContext);
        }
    }
}

