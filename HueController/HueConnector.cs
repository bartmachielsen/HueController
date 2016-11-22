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
            var cts = new CancellationTokenSource();
            cts.CancelAfter(1000);
            try
            {
                HttpClient client = new HttpClient();
                Uri uriAllLight = new Uri($"http://{ip}:{port}/api/{username}/lights/");
                var response = await client.GetAsync(uriAllLight);
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

        public async Task<string> getUsername(string devicetype)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(1000);
            try
            {
                HttpClient client = new HttpClient();
                Uri uriAllLight = new Uri($"http://{ip}:{port}/api/");
                IHttpContent content = new HttpStringContent(JSONGenerator.getUsernameByDeviceType(devicetype), UnicodeEncoding.Utf8, "application/json");
                var response = await client.PostAsync(uriAllLight, content);
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
