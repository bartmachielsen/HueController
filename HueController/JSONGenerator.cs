using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static string changeState(bool stateOn, int hue, int sat, int bri)
        {
            if (!stateOn)
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
    }
}
