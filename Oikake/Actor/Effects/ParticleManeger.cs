using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Oikake.Device;

namespace Oikake.Actor.Effects
{
    class ParticleManeger
    {
        //パーティクルのリスト
        private List<Particle> particles = new List<Particle>();
        private List<Particle> addParticles = new List<Particle>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ParticleManeger()
        {
       
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            particles.Clear();//リストクリア
            addParticles.Clear();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //一括更新
            particles.ForEach(particle => particle.isDead());

            //追加リストからデータを登録
            particles.AddRange(addParticles);
            addParticles.Clear();

            //死亡しているものはリストから消去
            addParticles.RemoveAll(particles => particles.isDead());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            //一括描画
            particles.ForEach(particle => particle.Draw(renderer));
        }

        /// <summary>
        /// パーティクルの追加
        /// </summary>
        /// <param name="particle"></param>
        public void Add(Particle particle)
        {
            addParticles.Add(particle);
        }
    }
}
