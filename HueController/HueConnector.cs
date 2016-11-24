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
    class HueConnector
    {
        public string ip = "localhost";
        public int port = 80;
        public string username;

        public async Task<string> RetrieveLights()
        {
            Uri uriAllLight = new Uri($"http://{ip}:{port}/api/{username}/lights/");
            return await get(uriAllLight);   

        }

        public async Task<string> getUsername(string devicetype)
        {
            Uri uriAllLight = new Uri($"http://{ip}:{port}/api/");
            IHttpContent content = new HttpStringContent(JSONGenerator.getUsernameByDeviceType(devicetype), UnicodeEncoding.Utf8, "application/json");
            return await post(uriAllLight, content);
        }

        public async Task<string> changestate(Light light, bool setColor = true)
        {
            System.Diagnostics.Debug.WriteLine(light.state.on);
            string json;
            if (!setColor)
            {
                json = JSONGenerator.changeState(light.state.on);
            }
            else
            {
                json = JSONGenerator.changeState(light.state.on, light.state.hue, light.state.sat, light.state.bri);
            }
            Uri uriAllLight = new Uri($"http://{ip}:{port}/api/{username}/lights/{light.id}/state");
            var content = new HttpStringContent(json, UnicodeEncoding.Utf8, "application/json");
            return await put(uriAllLight,content);
        }

        public async Task<string> put(Uri link, IHttpContent content)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(1000);
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.PutAsync(link, content);
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
        public async Task<string> get(Uri link)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(1000);
            try
            {
                HttpClient client = new HttpClient();
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
        public async Task<string> post(Uri link, IHttpContent content)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(1000);
            try
            {
                HttpClient client = new HttpClient();
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
    }
}
