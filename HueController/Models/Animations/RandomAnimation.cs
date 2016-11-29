using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueController.Models.Animations
{
    class RandomAnimation
    {
        public List<int[]> Animate()
        {
            List<int[]> colors = new List<int[]>();
            Random random = new Random();

            for (int i = 0; i < 100; i++)
            {
                colors.Add(new int[] {random.Next(65553), random.Next(254), random.Next(254)});
            }

            return colors;
        }
    }
}
