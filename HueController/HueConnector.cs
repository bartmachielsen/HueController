using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using HueController.Models;
using HttpClient = Windows.Web.Http.HttpClient;
using HttpResponseMessage = Windows.Web.Http.HttpResponseMessage;
using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

namespace HueController
{
 
   public class HueConnector
    {
       
        public Room room;
        public HttpClient client;
        public HueConnector(Room room)
        {
            this.room = room;
            client = new HttpClient();
        }

        public virtual async Task<string> RetrieveLights()
        {
            Uri uriAllLight = new Uri($"http://{room.addres}:{room.port}/api/{room.username}/lights/");
            return await get(uriAllLight);   

        }

        public virtual async Task<string> getUsername(string devicetype)
        {
            Uri uriAllLight = new Uri($"http://{room.addres}:{room.port}/api/");
            IHttpContent content = new HttpStringContent(JSONGenerator.getUsernameByDeviceType(devicetype), UnicodeEncoding.Utf8, "application/json");
            return await post(uriAllLight, content);
        }

        public virtual async Task<string> changestate(Light light, bool setColor = true)
        {
            string json;
            if (!setColor)
            {
                json = JSONGenerator.changeState(light.state.on);
            }
            else
            {
                json = JSONGenerator.changeState(light.state.on, light.state.hue, light.state.sat, light.state.bri, light.trans);
            }
            Uri uriAllLight = new Uri($"http://{room.addres}:{room.port}/api/{room.username}/lights/{light.id}/state");
            var content = new HttpStringContent(json, UnicodeEncoding.Utf8, "application/json");
            return await put(uriAllLight,content);
        }

        

        public virtual async Task<string> put(Uri link, IHttpContent content)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(1000);
            try
            {
                var response = await client.PutAsync(link, content);
                if (response == null || !response.IsSuccessStatusCode)
                    return null;
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
                return null;
            }
        }
        public virtual async Task<string> get(Uri link)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(1000);
            try
            {
                var response = await client.GetAsync(link);
                if (response == null || !response.IsSuccessStatusCode)
                    return string.Empty;

                string cont = await response.Content.ReadAsStringAsync();
                return cont;
            }
            catch (Exception e)
            {
                return string.Empty;
            }

        }
        public virtual async Task<string> post(Uri link, IHttpContent content)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(1000);
            try
            {
                var response = await client.PostAsync(link, content);
                if (response == null || !response.IsSuccessStatusCode)
                    return null;
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<string> changename(Light light)
        {
            Uri uriAllLight = new Uri($"http://{room.addres}:{room.port}/api/{room.username}/lights/{light.id}");
            var content = new HttpStringContent(JSONGenerator.changeName(light), UnicodeEncoding.Utf8, "application/json");
            return await put(uriAllLight, content);
        }
    }


    public class Simulator : HueConnector
    {
        List<Light> lights = new List<Light>();
        public Simulator(Room room) : base(room)
        {
            if (room.lights != null && room.lights.Count > 1)
            {
                lights = new List<Light>(room.lights);
                return;
            }
            lights = new List<Light>();
            for (int i = 0; i < 4; i++)
            {
                lights.Add(new Light()
                {
                    id = i,
                    name = $"Light [{i}]",
                    modelid = "FakeLight",
                    state = new State()
                    {
                      ct  = 0,
                      sat = 0,
                      effect = String.Empty,
                      bri = 255,
                      hue = 50,
                      on = false,
                      reachable = true
                    }
                });
            }

        }

        public override async Task<string> changename(Light light)
        {
            return "";
        }
        public override async Task<string> RetrieveLights()
        {
            return JSONGenerator.generateUglyLights(lights);
        }

        public override async Task<string> getUsername(string devicetype)
        {

            return JSONGenerator.usernamesuccesResponse("[NO-USERNAME-NEEDED]");
        }

        public override async Task<string> changestate(Light light, bool setColor = true)
        {
            lights.RemoveAll(light1 => light.id == light1.id);
            lights.Add(light);
            return "state changed";
        }
    }
}
