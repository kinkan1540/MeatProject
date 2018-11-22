using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Oikake.Device;

namespace Oikake.Scene
{
    class CheckScene : IScene
    {
        private bool isEndFlag;

        public CheckScene(IScene scene)
        {
            isEndFlag = true;
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            renderer.DrawTexture("CheckScen", Vector2.Zero);
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
            if (Input.GetKeyTrigger(Keys.Enter))
            {
               return Scene.Title;
            }
            return Scene.Stage2;
        }

        public void Shutdown()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            if (Input.GetKeyTrigger(Keys.Space))
            {
                isEndFlag = true;
            }

            if (Input.GetKeyTrigger(Keys.Enter))
            {
                isEndFlag = true;
            }
        }
    }
}
