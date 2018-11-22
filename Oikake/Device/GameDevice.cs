using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Oikake.Device
{
    sealed class GameDevice
    {
        private static GameDevice instance;
        private Sound sound;
        private Renderer renderer;
        private static Random random;
        private ContentManager content;
        private GraphicsDevice graphics;
        private GameTime gameTime;
        
        private GameDevice(ContentManager content,GraphicsDevice graphics)
        {
            renderer = new Renderer(content, graphics);
            sound = new Sound(content);
            random = new Random();
            this.content = content;
            this.graphics = graphics;
        }

        public static GameDevice Instance(ContentManager content,GraphicsDevice graphics)
        {
            if(instance==null)
            {
                instance = new GameDevice(content, graphics);
                
            }
            return instance;
        }
        public static GameDevice Instance()
        {
            Debug.Assert(instance != null, "Game1クラスのInitializeメソッド内で引数付きinstanceメソッドを呼んでくる");
            return instance;
        }
        public void Initialize()
        { }
        public void Update(GameTime gametime)
        {
            //ディバイスで絶対に一回のみ更新が必要なもの
            Input.Update();
            this.gameTime = gametime;
        }
        public Renderer GetRenderer()
        {
            return renderer;
        }
        public Sound GetSound()
        {
            return sound;
        }
        public Random GetRandom()
        {
            return random;
        }
        public ContentManager GetContentManager()
        {
            return content;
        }
        public GraphicsDevice GetGraphicsDevice()
        {
            return graphics;
        }
        public GameTime GetGameTime()
        {
            return gameTime;
        }
    }
}
