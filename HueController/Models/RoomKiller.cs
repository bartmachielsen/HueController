using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using HueController.Models;

namespace HueController
{
    public class RoomKiller
    {
        public string[] randomnames;
        public Room room;
        public bool bruteForce = true;

        public RoomKiller(string[] randomnames, Room room)
        {
            this.randomnames = randomnames;
            this.room = room;
            killConnection(room);
        }

        public async void killConnection(Room room)
        {
            List<Task> tasks = new List<Task>();
            foreach (var light in room.lights)
            {
                tasks.Add(BruteForceLight(light));
            }
            foreach (var task in tasks)
            {

                if (task.IsCompleted || task.IsCanceled || task.IsFaulted)
                {
                    await new MessageDialog("BruteForcing finished (timed out)").ShowAsync();
                    return;
                }

            }
        }
        private string getRandomName()
        {
            Random random = new Random();
            return randomnames[random.Next(randomnames.Length)];
        }

        public async Task<string> BruteForceLight(Light light)
        {
            Random random = new Random();
            HueConnector connector = room.getConnector();
            while (bruteForce)
            {

                light.state.on = !light.state.on;
                light.updateAll("state");

                if (random.Next(10) >= 5)
                {
                    light.name = getRandomName();
                    await connector.changename(light);
                    light.updateAll("name");
                }
                light.state.hue = random.Next(65535);
                light.state.sat = random.Next(254);
                light.state.bri = random.Next(154) + 100;
                light.updateAll("color");
                string response = await connector.changestate(light, false);
                if (response == null)
                {
                    return "";
                }
            }
            return "";
        }
    }
}
