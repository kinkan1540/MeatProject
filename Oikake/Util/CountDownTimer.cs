using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Oikake.Util
{
    class CountDownTimer:Timer
    {
        public CountDownTimer()
            :base()
        { }
        public CountDownTimer(float second)
            :base(second)
        {
            Initialize();
        }
        public override void Initialize()
        {
            CurrentTime = limitTime;
        }

        public override bool IsTime()
        {
            return CurrentTime <= 0.0f;
        }

        public override float Rate()
        {
            return 1.0f - CurrentTime / limitTime;
        }

        public override void Update(GameTime gameTime)
        {
            CurrentTime = Math.Max(CurrentTime - 1f, 0.0f);
        }
    }
}
