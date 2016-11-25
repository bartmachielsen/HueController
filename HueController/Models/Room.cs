using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueController.Models
{
    public class Room
    {
        public string name { get; set; }
        public string addres { get; set; }
        public int port { get; set; }

        public Room(string name, string addres, int port)
        {
            this.name = name;
            this.addres = addres;
            this.port = port;
        }
    }
}
