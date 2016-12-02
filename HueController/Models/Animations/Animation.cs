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
        public int delayTime { get; set; } = 400;
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
            delayTime = 500;
        }

        public override async void ExecuteOne(int index,Light light)
        {
            if (light.id%2 == 0 && even || light.id%2 != 0 && !even)
            {
                light.setColor(0, 0, 0);
            }
            else
            {
                light.setColor(0,0,254);
            }
            await connector.changestate(light, true);
        }

        public override void RoundFinished()
        {
            even = !even;
        }
    }


    public class RandomAnimation : Animation
    {
        public int hue, bri, sat;
        public Random random;
        public RandomAnimation(HueConnector connector) : base(connector)
        {
            random = new Random();
            RoundFinished();
        }

        public override void RoundFinished()
        {
            hue = random.Next(65535);
            sat = random.Next(254);
            bri = random.Next(154) + 100;
        }

        public override async void ExecuteOne(int index, Light light)
        {
            light.setColor(hue,sat,bri);
            await connector.changestate(light, true);
        }
    }

    public class ColorLoop : Animation
    {
        public int index = 0;
        public int focushue,  maxsize;
        public ColorLoop(int focushue,int maxsize, HueConnector connector) : base(connector)
        {
            this.focushue = focushue;
            this.maxsize = maxsize;

        }

        public override void ExecuteOne(int index, Light light)
        {
            if (this.index == light.id)
            {
                light.setColor(focushue, 254, 254);
            }
            else
            {
                light.setColor(focushue, 0, 0);
            }
        }

        public override void RoundFinished()
        {
            index++;
            if (index > maxsize)
            {
                index = 0;
            }
        }
    }

    public class ColorLoopBack : ColorLoop
    {
        private int target = 1;
        public ColorLoopBack(int focushue,  int maxsize, HueConnector connector) : base(focushue, maxsize, connector)
        {
        }

        public override void RoundFinished()
        {

            index += target;
            if (index > maxsize)
            {
                target = -1;
            }
            if (index < 0)
            {
                target = 1;
            }
        }
    }
}
