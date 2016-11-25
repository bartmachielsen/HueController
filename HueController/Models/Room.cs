using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace HueController.Models
{
    [DataContract]
    public class Room
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string addres { get; set; }
        [DataMember]
        public int port { get; set; }
        [DataMember]
        public string username { get; set; }
        

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
    }
}
