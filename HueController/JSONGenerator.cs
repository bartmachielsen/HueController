using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HueController.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public static string changeState(bool stateOn, int hue = -1, int sat = -1, int bri = -1, int trans = 0)
        {
            if (!stateOn || hue == -1 || sat == -1 || bri == -1)
            {
                return JsonConvert.SerializeObject(new
                {
                    on = stateOn,
                    transitiontime = trans
                });
            }
            return JsonConvert.SerializeObject(new
            {
                on = stateOn,
                sat = sat,
                bri = bri,
                hue = hue,
                transitiontime = trans
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
            var only = new List<Room>(rooms);
            only.RemoveAll(room => room is SimulatorRoom);
            rooms = new ObservableCollection<Room>(only);
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

        public static string generateUglyLights(List<Light> lights)
        {
            JObject obj = new JObject();
            foreach (Light light in lights)
            {
                dynamic jobj = new JObject();
                obj[light.id + ""] = jobj;
                jobj.name = light.name;
                jobj.modelid = light.modelid;

                dynamic state = new JObject();
                state.ct = light.state.ct;
                state.sat = light.state.sat;
                state.effect = light.state.effect;
                state.bri = light.state.bri;
                state.hue = light.state.hue;
                state.on = light.state.on;
                state.reachable = light.state.reachable;
                jobj.state = state;
            }
            return JsonConvert.SerializeObject(obj);
        }

        public static string usernamesuccesResponse(string username)
        {
            return JsonConvert.SerializeObject(new
            {
                succes = new {username=username }
            });
        }

        public static string usernames(string[] usernames)
        {
            return JsonConvert.SerializeObject(new
            {
                usernames = usernames
            });
        }
    }
}
