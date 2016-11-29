using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace HueController.Models.Animations
{
    public abstract class Animation
    {
        public int length { get; set; } = 100;
        public HueConnector connector;

        public Animation(HueConnector connector)
        {
            this.connector = connector;
        }

        public abstract void ExecuteOne(int index, Light light);

        public virtual void RoundFinished()
        {
            
        }
    }

    public class AllRandomAnimation : Animation
    {
        private Random random;

        public AllRandomAnimation(HueConnector connector) : base(connector)
        {
            this.random = new Random();
        }


        public override async void ExecuteOne(int index, Light light)
        {
            light.state.hue = random.Next(65535);
            light.state.sat = random.Next(254);
            light.state.bri = random.Next(154) + 100;
            light.updateAll("color");
            await connector.changestate(light, true);
        }
    }

    public class BlackWhiteAnimation : Animation
    {
        private bool even = false;
        public BlackWhiteAnimation(HueConnector connector) : base(connector)
        {
        }

        public override async void ExecuteOne(int index,Light light)
        {
            if (light.id%2 == 0 && even || light.id%2 != 0 && !even)
            {
                light.state.on = false;
            }
            else
            {

                light.state.on = true;
            }
            await connector.changestate(light, false);
        }

        public override void RoundFinished()
        {
            even = !even;
        }
    }
}
