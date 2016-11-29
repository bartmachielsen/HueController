using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
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
        public ObservableCollection<Light> lights;
        

        public Room(int id, string name, string addres, int port, string username = null)
        {
            this.name = name;
            this.addres = addres;
            this.port = port;
            if (username == "" || username == "")
            {
                this.username = null;
            }
            else
            {
                this.username = username;
            }
        }

        public virtual HueConnector getConnector()
        {
            return new HueConnector(this);
        }
    }

    public class SimulatorRoom : Room
    {
        private Simulator simulator;
        public SimulatorRoom(int id) : base(id, "Simulator", "Application", 69, "[no-username-needed]")
        {
            simulator = new Simulator(this);
        }

        public override HueConnector getConnector()
        {
            return simulator;
        }
    }
}
