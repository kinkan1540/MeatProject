using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace Oikake.Util
{
    class CountUpTimer : Timer
    {
        public CountUpTimer()
            : base()
        { Initialize(); }
        public CountUpTimer(float second)
            :base(second)
        {Initialize();}
        public override void Initialize()
        {
            CurrentTime = 0.0f;
        }
        public override bool IsTime()
        {
            return CurrentTime >= limitTime;
        }

        public override float Rate()
        {
            return CurrentTime / limitTime;
        }

        public override void Update(GameTime gameTime)
        {
            CurrentTime = Math.Min(CurrentTime + 1f, 0.0f);
        }
    }
}
