using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace HueController.Models
{
    public class Light
    {
        public bool state = false;
        public int id;
        public string name;

        public string showtext
        {

            get
            {
                if (state)
                    return "OFF";
                return "ON";

            }
            set { state = "ON" == value; }

        }

        public Brush color { get; set; }= new SolidColorBrush(Colors.Red);

        public Light(int id)
        {
            this.id = id;
            this.name = "Light" + id;
        }

        public override string ToString()
        {
            return $"{name}, {id}, {state}";
        }

       
    }
}
