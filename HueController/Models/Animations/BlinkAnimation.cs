using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueController.Models.Animations
{
    class BlinkAnimation : IAnimation
    {
        public List<int[]> Animate(Light light)
        {
            List<int[]> colors = new List<int[]>();

            for (int i = 0; i < 5; i++)
            {
                for (int ii = 0; i < 2; i++)
                {
                    colors.Add(new int[] { 0,0,254 });
                    colors.Add(new int[] { 0,0,0 });
                }
            }
            
            return colors;
        }
    }
}
