using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Oikake.Util;
using Oikake.Device;
using Oikake.Def;
using Oikake.Scene;
namespace Oikake.Actor
{
    class RandomAI : AI
    {
        //private intchangeTimer//切り替え時間
        private Timer timer;
        private Random rnd;
        private GameTime gameTime;

        public RandomAI()
        {
            var gameDevice = GameDevice.Instance();
            rnd = gameDevice.GetRandom();
            timer = new CountDownTimer(rnd.Next(2, 6));
            gameTime = gameDevice.GetGameTime();
        }

        public override Vector2 Think(Character character)
        {
            character.SetPosition(ref position);

            timer.Update(gameTime);

            if(timer.IsTime())
            {
                position = new Vector2(
                    rnd.Next(Screen.Width - 64),
                    rnd.Next(Screen.Height - 64));
            }
            return position;
        }
    }
}
