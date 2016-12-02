using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media;

namespace HueController.Models
{
    public class Light : INotifyPropertyChanged
    {
        public string name;
        public event PropertyChangedEventHandler PropertyChanged;
        public int id;
        public bool selected { get; set; } = false;
        public string modelid { get; set; }
        public State state { get; set; }
        public string type { get; set; }
        public int trans { get; set; }= 0;
        public string uniqueid { get; set; }
        

        public string[] slider
        {
            get
            {
                return new string[] {state.hue +"", state.bri +"", state.sat +""};
            }
            set
            {
               
            }
        }

        public Brush color
        {
            get
            {
                if(state.on)
                    return new SolidColorBrush(ColorUtil.HsvToRgb(state.hue, state.sat, state.bri));
                return new SolidColorBrush(Colors.Transparent);
            }
            set
            {
                updateAll("color");
            }
        }

        public void updateAll(string value)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(value));
        }
        public string statetext
        {
            get {return state.on + ""; }
            set { state.on = value == "True"; }
        }

        public void setColor(int hue, int sat, int bri)
        {
            this.state.hue = hue;
            this.state.bri = bri;
            this.state.sat = sat;
            this.updateAll("color");
        }

        public void setColor(Color color)
        {
            double hue, sat, bri;
            ColorUtil.RGBtoHSV(color.R, color.G, color.B, out hue, out sat, out bri);
            this.state.hue = (int)hue;
            if (this.state.hue < 0)
                this.state.hue = 0;
            this.state.sat = (int) sat;
            this.state.bri = (int) bri;
            this.updateAll("color");
        }

        public bool merge(Light light)
        {
            bool colorchanged = false;
            if (this.id == light.id)
            {
                if (this.state.hue != light.state.hue)
                {
                    colorchanged = true;
                    this.state.hue = light.state.hue;
                }

                if (this.state.bri != light.state.bri)
                {
                    colorchanged = true;
                    this.state.bri = light.state.bri;
                }


                if (this.state.sat != light.state.sat)
                {
                    colorchanged = true;
                    this.state.sat = light.state.sat;
                }
                if(colorchanged)
                    this.updateAll("color");
                if (this.name != light.name)
                {
                    this.name = light.name;
                    this.updateAll("name");
                }
                return true;
            }
            return false;
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
