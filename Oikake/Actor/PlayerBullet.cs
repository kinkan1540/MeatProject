using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Oikake.Scene;
using Oikake.Util;
using Oikake.Def;

namespace Oikake.Actor
{
    class PlayerBullet : Character
    {
        private Vector2 velocity;
        public PlayerBullet(Vector2 position, IGameMediator mediator, Vector2 velocity) : base("white", mediator)
        {
            this.position = position;
            this.velocity = velocity;
        }
        public override void Hit(Character other)
        {
            isDeadFlag = true;
        }

        public override void Initialize()
        {
           
        }

        public override void Shutdown()
        { 
        }

        public override void Update(GameTime gameTime)
        {
            //移動処理
            position += velocity * 20;

            //画面の外に出たら死亡
            Range range = new Range(0, Screen.Width);
            if(range.IsOutOFRange((int)position.Y))
            {
                isDeadFlag = true;
            }
            range = new Range(0, Screen.Height);
            if(range.IsOutOFRange((int)position.Y))
            {
                isDeadFlag = true;
            }
        }
    }
}
