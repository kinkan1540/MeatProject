using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Oikake.Actor
{
  abstract  class AI
    {
        protected Vector2 position;//移動量

       public AI()
        {
            position = Vector2.Zero;
        }

        public abstract Vector2 Think(Character character);
    }
}
