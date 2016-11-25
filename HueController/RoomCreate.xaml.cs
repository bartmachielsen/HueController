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

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HueController
{
    public sealed partial class RoomCreate : ContentDialog
    {
        public RoomCreate(Room room = null)
        {
            this.InitializeComponent();
            if (room != null)
            {
                NameBox.Text = room.name;
                IPBox.Text = room.addres;
                PortBox.Text = ""+ room.port;
                UsernameBox.Text = room.username;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        public string[] getInputted()
        {
            return new string[] {NameBox.Text, IPBox.Text, PortBox.Text, UsernameBox.Text };
        }

        private void TextEvaluator(object sender, TextChangedEventArgs e)
        {
            var box = (TextBox) sender;
            if (box.Text.Contains(":") || box.Text.Contains(","))
            {
                box.Text = box.Text.Replace(":","").Replace(",","");
            }
        }
    }
}
