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
            JObject obj = (JObject) json;
            ObservableCollection<Light> lights = new ObservableCollection<Light>();
            foreach (JToken child in ((JObject)json).Children())
            {
                dynamic dyno = child.First;
                Light light = new Light();
            
                light.name = dyno.name;
                light.modelid = dyno.modelid;

                State state = new State();
                light.state = state;
                state.ct = dyno.state.ct;
                state.sat = dyno.state.sat;
                state.effect = dyno.state.effect;
                state.bri = dyno.state.bri;
                state.hue = dyno.state.hue;
                state.on = dyno.state.on;
                state.reachable = dyno.state.reachable;
                state.effect = dyno.state.effect;
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

    }
}
