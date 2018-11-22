using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Oikake.Def;
using Oikake.Device;

namespace Oikake.Actor.Effects
{
    class Particle
    {
        protected readonly float GRAVITY = 0.5f;//重力
        protected string name;//画像のアセット名
        protected bool isDeadFlag;//死亡フラグ
        protected Vector2 position;//位置
        protected Vector2 velocity;//移動量
        protected IParticleMediator mediator;//仲介者

      /// <summary>
      /// コンストラクタ
      /// </summary>
      /// <param name="name"></param>
      /// <param name="position"></param>
      /// <param name="velocity"></param>
      /// <param name="mediator"></param>
        public Particle(string name,Vector2 position,Vector2 velocity,IParticleMediator mediator)
        {
            this.name = name;
            this.position = position;
            this.velocity = velocity;
            this.mediator = mediator;
            isDeadFlag = false;
        }

        public Particle(IParticleMediator mediator):this("particle",Vector2.Zero,Vector2.Zero, mediator)
        {
            isDeadFlag = true;
        }
        /// <summary>
        /// テクスチャ設定
        /// </summary>
        /// <param name="name"></param>
        public void SetTexture(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// 位置の設定
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        /// <summary>
        /// 移動量の生成
        /// </summary>
        /// <param name="velocity"></param>
        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        /// <summary>
        /// テクスチャ名の取得
        /// </summary>
        /// <returns></returns>
        public string GetTexture()
        {
            return name;
        }

        /// <summary>
        /// 位置の取得
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition()
        {
            return position;
        }
        /// <summary>
        /// 更新メソッド
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            //移動
            position -= velocity;
            //重力を加える(画面下方向は+)
            velocity.Y += GRAVITY;
            //画面下についたか?
            isDeadFlag = (position.Y > Screen.Height);
        }

        /// <summary>
        ///描画仮想メソッド
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
        }

        /// <summary>
        /// 死亡か
        /// </summary>
        /// <returns></returns>
        public bool isDead()
        {
            return isDeadFlag;
        }
    }
}
