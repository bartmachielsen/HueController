using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HueController.Models;
using Newtonsoft.Json;

namespace HueController
{
    class JSONParser
    {
        public static ObservableCollection<Light> getLights(string response)
        {
            dynamic json = JsonConvert.DeserializeObject(response);
            throw new NotImplementedException();
        }
    }
}
