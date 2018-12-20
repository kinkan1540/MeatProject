using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oikake.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Oikake.Device;
namespace Oikake.Actor
{
    /// <summary>
    /// キャラクター継承クラス
    /// </summary>
    abstract　class Character
    {
        public Vector2 position;
        public int HP { get; set; }
        protected string name;
        protected bool isDeadFlag;
        protected IGameMediator mediator;
        public bool IsGetOn { get; set; }
        public Vector2 Position { get; set;}
        public Vector2 RobotPosition { get; set;}
        public abstract void Hit(Character s);
        public bool IsA{ get;set;}
        public bool isDamage { get; set; }
        public int damageCnt { get; set; }
        public int counter{ get; set; }
        public int Damege { get; set;}

        protected enum State
           
        {
            Preparation,//準備
            Dying,
            Alive,//死に際
            Dead//死亡
        };

        /// <summary>
        /// 位置の受け渡し
        /// (引数で渡された変数に自分の位置を渡す)
        /// </summary>
        /// <param name="other">位置を送りたい相手</param>
        public void SetPosition(ref Vector2 other)
        {
            other = position;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">画像の名前</param>
        public Character(string name,IGameMediator mediator)
        {
            this.name = name;
            position = Vector2.Zero;
            isDeadFlag = false;
            this.mediator = mediator;
        }
        
        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);
        public abstract void Shutdown();

        /// <summary>
        /// 死んでいるか？
        /// </summary>
        /// <returns></returns>
        public bool IsDead()
        {
            return isDeadFlag;
        }
        public bool IsCollision(Character other)
        {
          
            if (GetRect().Intersects(other.GetRect()))
            {
                return true;
            }
            return false;
        }


        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
        }


        public virtual Vector2 GetPosition()
        {
            return position;
        }

        public virtual Rectangle GetRect()
        {
            Rectangle rect = new Rectangle(
                (int)position.X, (int)position.Y,
                32, 32);
            return rect;
        }

        public bool Invincibly()
        {
            if (mediator.GetRobot().IsGetOn == false)
            {
                if (Math.Abs(mediator.GetRobot().GetPosition().X - position.X) < 32 && Math.Abs(GetPosition().Y - mediator.GetRobot().GetPosition().Y) < 32)
                {
                    if (isDamage == false)
                    {
                        isDamage = true;
                        HP -= Damege;
                        return true;
                    }
                }
            }
            if (IsGetOn == false)
            {
                if (isDamage)
                {
                    damageCnt++;
                    counter++;
                    Damege = 0;
                    if (100 < damageCnt)
                    {
                        isDamage = false;
                        damageCnt = 0;
                        counter = 0;
                    }
                }
            }
            return false;
        }
    }
}


     