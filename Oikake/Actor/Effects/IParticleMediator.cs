using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Oikake.Actor.Effects
{
    /// <summary>
    /// パーティクル仲介者
    /// </summary>
   interface IParticleMediator
    {
        /// <summary>
        /// パーティクル仲介者
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Particle generate(string name);
    }
}
