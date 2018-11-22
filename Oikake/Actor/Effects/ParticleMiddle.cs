using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Oikake.Util;
using Oikake.Device; 

namespace Oikake.Actor.Effects
{
    class ParticleMiddle:Particle
    {
        private Timer timer;
        public ParticleMiddle(string name, Vector2 position, Vector2 velocity, IParticleMediator mediator) : base(name, position, velocity, mediator)
        {

        }
        public ParticleMiddle(IParticleMediator mediator) : base(mediator)
        {
            Random random = GameDevice.Instance().GetRandom();
            timer = new CountDownTimer(random.Next(3, 6));
            name = "particleBlue";
        }
        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void Update(GameTime gameTime)
        {
            //親クラスで更新
            base.Update(gameTime);
            velocity -= velocity * 0.006f;
            timer.Update(gameTime);
            isDeadFlag = timer.IsTime();
        }
    }
}
