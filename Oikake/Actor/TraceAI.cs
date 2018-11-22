using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Oikake.Actor
{
    class TraceAI : AI
    {
        private List<Vector2> positionList;
        private int current;

        public TraceAI()
        {
            positionList = new List<Vector2>()
            {
                new Vector2(10,10),
                new Vector2(700,500),
                new Vector2(700,10),
                new Vector2(10,500),
                new Vector2(350,10),
                new Vector2(350,300),
                new Vector2(600,300),
                new Vector2(10,10),
                new Vector2(400,300)
        };
            current = 0;
        }
        public override Vector2 Think(Character character)
        {
            character.SetPosition(ref position);

            var velocity = positionList[current] - position;
            velocity.Normalize();

            position += velocity * 5;

            if ((positionList[current] - position).Length() < 5.0f)
            {
                current = (current + 1) % positionList.Count;
            }
            return position;
        }
    }
}
