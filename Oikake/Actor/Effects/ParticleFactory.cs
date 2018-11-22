using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Oikake.Actor.Effects
{
    class ParticleFactory
    {
        //仲介者
        private IParticleMediator mediator;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mediator"></param>
        public ParticleFactory(IParticleMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// 作成
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Particle create(string name)
        {
            Particle particle = null;
            if(name=="Particle")
            {
                particle = new Particle(mediator);
            }
            else if(name=="particleBlue")
            {
                particle = new ParticleBule(mediator);
            }
            return particle;
        }

        public Particle create(string name, Vector2 position,Vector2 velocity)
        {
            Particle particle = null;
            if (name == "Particle")
            {
                particle = new Particle("particle", position, velocity, mediator);
            }
            else if(name=="ParticleBlue")
            {
                particle = new ParticleBule("particleBlue", position, velocity, mediator);
            }
            return particle;
        }
          
    }
}
