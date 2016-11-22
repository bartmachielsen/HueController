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

namespace HueController
{
    class HueConnector
    {
        public string ip = "127.0.0.1";
        public int port = 8000;
        public string username = "";

        public async Task<string> RetrieveLights()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(1000);
            try
            {
                HttpClient client = new HttpClient();
                Uri uriAllLight = new Uri($"http://{ip}:{port}/api/{username}/lights/");
                var response = await client.GetAsync(uriAllLight).AsTask(cts.Token);
                if (response == null || !response.IsSuccessStatusCode)
                    return string.Empty;
                return await response.Content.ReadAsStringAsync();
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
                IHttpContent content = new HttpStringContent(JSONGenerator.getUsernameByDeviceType(devicetype));
                var response = await client.PostAsync(uriAllLight, content).AsTask(cts.Token);
                System.Diagnostics.Debug.WriteLine(response);
                if (response == null || !response.IsSuccessStatusCode)
                    return string.Empty;
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
}
