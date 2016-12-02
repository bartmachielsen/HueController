using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HueController.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HueController
{
    class JSONParser
    {
        public static ObservableCollection<Light> getLights(string response)
        {
            dynamic json = JsonConvert.DeserializeObject(response);
            if (!(json is JObject))
            {
                System.Diagnostics.Debug.WriteLine(response);
                return null;
            }
            JObject obj = (JObject) json;
            ObservableCollection<Light> lights = new ObservableCollection<Light>();
            
            if (json == null)
            {
                return null;
            }
            foreach (JToken child in ((JObject)json).Children())
            {
                dynamic dyno = child.First;
                Light light = new Light();
                light.id = Int32.Parse(child.Path);

                light.name = dyno.name;
                light.modelid = dyno.modelid;
                State state = new State();
                light.state = state;
                if(dyno.state.ct != null)
                    state.ct = dyno.state.ct;
                if (dyno.state.sat != null)
                    state.sat = dyno.state.sat;
                if (dyno.state.effect != null)
                    state.effect = dyno.state.effect;
                if (dyno.state.bri != null)
                    state.bri = dyno.state.bri;
                if (dyno.state.hue != null)
                    state.hue = dyno.state.hue;
                state.on = dyno.state.on;
                state.reachable = dyno.state.reachable;
                lights.Add(light);
            }
            return lights;
        }

        public static string getUsername(string response)
        {
            dynamic json = ((JArray)JsonConvert.DeserializeObject(response))[0];
            JToken inner;
            if (((JObject)json).TryGetValue("success",out inner))
                return json.success.username;
            return null;
        }

        public static ObservableCollection<Room> getRooms(string response)
        {
            try
            {
                var json = (JArray) ((dynamic)JsonConvert.DeserializeObject(response)).rooms;
                int index = 0;
                ObservableCollection<Room> rooms = new ObservableCollection<Room>();
                foreach (var room in json)
                {
                    dynamic dyno = (dynamic) room;
                    Room rom = new Room(index, null,null,80);
                    rom.name = dyno.name;
                    rom.port = dyno.port;
                    rom.addres = dyno.address;
                    rom.username = dyno.username;
                    index++;
                    rooms.Add(rom);
                }
                return rooms;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ObservableCollection<RandomName> parseNamesFromSave(string localSetting)
        {
            ObservableCollection<RandomName> names = new ObservableCollection<RandomName>();
            try
            {
                dynamic json = JsonConvert.DeserializeObject(localSetting);
                JArray array = (JArray) (json.names);
                foreach (dynamic jsono in array)
                {
                    string name = jsono.name;
                    if(name != null)
                        names.Add(new RandomName(name));
                }
                return names;
            }
            catch (Exception)
            {
                return names;
            }
            
        }
    }
}
