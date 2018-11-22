using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Oikake.Device;
using Oikake.Util;

namespace Oikake.Actor.Effects
{
    class ParticleBule : Particle
    {
        private Timer timer;

        public ParticleBule(string name, Vector2 position, Vector2 velocity, IParticleMediator mediator):base(name,position,velocity,mediator)
        {
            var random = GameDevice.Instance().GetRandom();
            timer = new CountDownTimer(random.Next(1, 3));
        }

        public ParticleBule(IParticleMediator mediator):base(mediator)
        {
            var random = GameDevice.Instance().GetRandom();
            timer = new CountDownTimer(random.Next(1, 3));
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
            timer.Update(gameTime);
            isDeadFlag = timer.IsTime();
        }
    }
}
