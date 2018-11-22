using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oikake.Device;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Oikake.Scene
{
    class Ending:IScene
    {
        private bool isEndFlag;
        IScene backGroundScene;
        private Sound sound;

        public Ending(IScene scene)
        {
            isEndFlag = false;
            backGroundScene = scene;
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();
        }
        public void Draw(Renderer renderer)
        {
            backGroundScene.Draw(renderer);

            renderer.Begin();
            renderer.DrawTexture("GameOver",Vector2.Zero);
            renderer.End();
        }

        public void Initialize()
        {
            isEndFlag = false;
            
        }

        public bool IsEnd()
        {
            return isEndFlag;
        }

        public Scene Next()
        {
            return Scene.Title;
        }

        public void Shutdown()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            if(Input.GetKeyTrigger(Keys.Space))
            {
                sound.PlaySE("endingse");
                isEndFlag = true;
            }
            sound.PlayBGM("BadEnd");
        }
    }
}
