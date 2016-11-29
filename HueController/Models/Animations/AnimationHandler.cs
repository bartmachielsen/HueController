using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueController.Models.Animations
{
    class AnimationHandler
    {
        public static async void ExecuteAnimation(Animation animation, List<Light> lights )
        {
            for (int i = 0; i < animation.length; i++)
            {
                foreach (var light in lights)
                {
                    animation.ExecuteOne(i, light);
                }
                animation.RoundFinished();
                await Task.Delay(animation.delayTime);
            }
        }
    }
}
