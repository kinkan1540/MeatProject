using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oikake.Device;
using Microsoft.Xna.Framework;

namespace Oikake.Scene
{
    class Score
    {
        private int poolScore;
        private int score;
        public Score()
        {
            Initialize();
        }
        public void Initialize()
        {
            score = 0;poolScore = 0;
        }
        public void Add()
        {
            poolScore++;
        }
        public void Add(int num)
        {
            poolScore = poolScore + num;
        }
        public void Update(GameTime gameTime)
        {
            if(poolScore>0)
            {
                score++;//合計を増やす
                poolScore -= 1;//プール得点を減らす
            }
            else if(poolScore>0)
            {
                score += 1;
                poolScore -= 1;
            }
        }
        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("score", new Vector2(50, 10));
            renderer.DrawNumber("number", new Vector2(250, 13),score);
        }
        public void Shutdown()
        {
            score += poolScore;poolScore=0 ;
            if(score<0)
            {
                score = 0;
            }
            poolScore = 0;
        }
        public int GetScore()
        {
            int curentScore = score + poolScore;
            if (curentScore < 0)
            {
                curentScore = 0;
            }
            return curentScore;
        }
    }
}
