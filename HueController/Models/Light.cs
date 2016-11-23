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
        public string name;
        public int id;
        public string modelid { get; set; }
        public State state { get; set; }
        public string type { get; set; }
        public string uniqueid { get; set; }

        public Brush color
        {
            get { return new SolidColorBrush(ColorUtil.HsvToRgb(state.hue, state.sat, state.bri)); }
            set
            {
               
            }
        }

        public string statetext
        {
            get {return state.on + ""; }
            set { state.on = value == "True"; }
        }

    }


    public class State
    {
        public IList<int> xy { get; set; }
        public int ct { get; set; }
        public string alert { get; set; }
        public int sat { get; set; }
        public string effect { get; set; }
        public int bri { get; set; }
        public int hue { get; set; }
        public string colormode { get; set; }
        public bool reachable { get; set; }
        public bool on { get; set; }
    }

   

}
