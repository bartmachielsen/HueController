using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueController.Models.Animations
{
    class SmoothAnimation : IAnimation
    {
        public List<int[]> Animate(Light light)
        {
            List<int[]> colors = new List<int[]>();

            for (int i = 0; i < 65535; i += 100)
            {
                colors.Add(new int[] {i, 254, 254});
            }

            return colors;
        }
    }
}
