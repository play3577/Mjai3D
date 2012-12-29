using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Mjai3D
{
    class FpsCounter
    {

        double totalTime;
        int frameCount;

        public double Fps
        {
            get;
            private set;
        }

        public FpsCounter()
        {
            totalTime = 0;
            frameCount = 0;
            Fps = 0;
        }

        public void Draw(GameTime gameTime)
        {
            totalTime += gameTime.ElapsedGameTime.TotalSeconds;
            frameCount++;

            if (totalTime > 1.0)
            {
                Fps = frameCount / totalTime;
                totalTime = 0;
                frameCount = 0;               
            }
        }
    }
}
