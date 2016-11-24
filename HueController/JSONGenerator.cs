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
    }
}
