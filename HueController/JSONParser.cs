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
            throw new NotImplementedException();
        }

        public static string getUsername(string response)
        {
            dynamic json = ((JArray)JsonConvert.DeserializeObject(response))[0];
            JToken inner;
            if (((JObject)json).TryGetValue("succes",out inner))
                return json.succes.username;
            return null;
        }

    }
}
