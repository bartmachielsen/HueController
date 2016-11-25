using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueController.Models.Animations
{
    class ColorswitchAnimation : IAnimation
    {
        public List<int[]> Animate(Light light)
        {
            List<int[]> colors = new List<int[]>();

            for (int i = 1; i < 17; i++)
            {
                colors.Add(new int[] {(int) 65535/16*i, 254, 254});
            }

            return colors;
        }
    }
}
