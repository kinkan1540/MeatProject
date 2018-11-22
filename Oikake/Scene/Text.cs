using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Oikake.Device;

namespace Oikake.Scene
{
    public class Text : Game1
    { 
        private SpriteBatch sprite;
        private SpriteFont font;
        private string stiring;
        private Vector2 position;

        public Text(string str, Vector2 position)
        {
            this.stiring = str;
            this.position = position;
            Content.RootDirectory = "content";
        }

        protected override void LoadContent()
        {
            sprite = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("TestFont");
            base.LoadContent();
        }

        private void Draw(Renderer renderer)
        {
            sprite.Begin();
            sprite.DrawString(font, stiring,position, Color.Black);
            sprite.End();
        }
    }
}
