using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace HueController.Models
{
    public class Room
    {
        public int id { get; set; }
        public string name { get; set; }
        public string addres { get; set; }
        public int port { get; set; }
        public string username { get; set; }

        public Brush background
        {
            get
            {
                Random random = new Random();
                return new SolidColorBrush(new Color() {A=100, B=(byte)random.Next(255), G = (byte)random.Next(255), R = (byte)random.Next(255) });
            }
        }

        public Room(int id, string name, string addres, int port)
        {
            this.name = name;
            this.addres = addres;
            this.port = port;
        }
    }
}
