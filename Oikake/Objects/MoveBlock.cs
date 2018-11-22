using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Oikake.Device;
using Oikake.Util;
using Oikake.Actor;
using Oikake.Scene;

namespace Oikake.Objects
{
    class MoveBlock : Character
    {
        Vector2 velocity = Vector2.Zero;
        private float speed;
        private Range range;
        private List<Vector2> movePos;
        private List<Vector2> rightPos;
        private List<Vector2> leftPos;
        private int minRct;
        private int maxRct;
        private int moveCount;



        public MoveBlock(IGameMediator mediator, Vector2 position, float speed) : base("MoveBlock", mediator)
        {
            this.position = position;
            this.speed = speed;
            range = new Range((int)position.X - 100, (int)position.X +100);
        }

        public override void Initialize()
        {
            velocity = new Vector2(1, 0);
            rightPos = new List<Vector2>() { new Vector2(64, 0), new Vector2(64, 31) };
            leftPos = new List<Vector2>() { new Vector2(-1, 0), new Vector2(-1, 31) };
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture("MoveBlock", position, Color.Red);
        }

        public override void Update(GameTime gameTime)
        {
            MoveUpdate();
        }



        private void MoveUpdate()
        {
            if (range.IsOutOFRange((int)position.X))
            {
                 velocity.X *= -1;
            }
            //移動
            Move();
            //カウンタ加算
            moveCount += 1;
            //8回移動したら
            if (moveCount >= 1)
            {
                moveCount = 0;
            }
        }

        private void Move()
        {
            Xmove();
        }

        private void Xmove()
        {
            //方向によるチェック位置を指定
            //右移動の場合
            if (velocity.X > 0)
            {
                movePos = rightPos;
            }
            //左移動の場合
            if (velocity.X < 0)
            {
                movePos = leftPos;
            }

            //1マスずつ移動する

            for (int i = 0; i < Math.Abs(velocity.X); i++)
            {
                foreach (Vector2 pos in movePos)
                {
                    if (mediator.IsMapBlock(position + pos))
                    {
                        velocity.X *= -1;
                        break;
                    }
                }
                position += velocity * speed;
            }
        }

        public override void Hit(Character other)
        {
            if (other is Player)
            {
                Player player = (Player)other;

                if (player.Position.Y > position.Y)
                {
                    return;
                }
              
                player.Position += velocity * speed;
            }
        }

        public override void Shutdown()
        {
            
        }

        public override Rectangle GetRect()
        {
            Rectangle rect = new Rectangle(
                (int)position.X, (int)position.Y,
                64, 32);

            return rect;
        }

        public Vector2 Velocity()
        {
            return velocity * speed;
        }
    }
}
