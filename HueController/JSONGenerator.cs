using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HueController.Models;
using Newtonsoft.Json;

namespace HueController
{
    class JSONGenerator
    {
        public static string getUsernameByDeviceType(string devicetype)
        {
            return JsonConvert.SerializeObject(new
            {
                devicetype = devicetype
            });
        }

        public static string changeState(bool stateOn, int hue = -1, int sat = -1, int bri = -1)
        {
            if (!stateOn || hue == -1 || sat == -1 || bri == -1)
            {
                return JsonConvert.SerializeObject(new
                {
                    on = stateOn
                });
            }
            return JsonConvert.SerializeObject(new
            {
                on = stateOn,
                sat = sat,
                bri = bri,
                hue = hue
            });
        }

        public static string changeName(Light light)
        {
            return JsonConvert.SerializeObject(new
            {
                name = light.name
            });
        }

        public static string rooms(ObservableCollection<Room> rooms)
        {
            var dynamicarray = new dynamic[rooms.Count];
            for (int i = 0; i < rooms.Count; i++)
            {
                Room room = rooms.ElementAt(i);
                dynamicarray[i] = new
                {
                    name = room.name,
                    address = room.addres,
                    port = room.port,
                    username = room.username
                };
            }
            return JsonConvert.SerializeObject(new
            {
                rooms = dynamicarray
            });
        }
    }
}
