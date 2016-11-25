using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueController.Models
{
    interface IAnimation
    {
        List<int[]> Animate(Light light);
    }
}
