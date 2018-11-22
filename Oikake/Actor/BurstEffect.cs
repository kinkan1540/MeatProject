using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Oikake.Util;
using Oikake.Scene;
using Oikake.Device;

namespace Oikake.Actor
{
    class BurstEffect : Character
    {
        private Timer timer;
        private int counter;
        private readonly int pictureNum = 7;
        public BurstEffect(Vector2 position,IGameMediator mediator)
            :base("pipo-btleffect", mediator)
        {
            this.position = position;
        }
        public override void Hit(Character other)
        {
            
        }

        public override void Initialize()
        {
            counter = 0;
            isDeadFlag = false;
            timer = new CountDownTimer(0.05f);
        }

        public override void Shutdown()
        {
           
        }

        public override void Update(GameTime gameTime)
        {
            timer.Update(gameTime);
            if(timer.IsTime())
            {
                counter += 1;
                timer.Initialize();
                if (counter >= pictureNum)
                {
                    isDeadFlag = true;
                } 
            }
        }
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position, new Rectangle(counter * 120, 0, 120, 120));
        }
    }
}
