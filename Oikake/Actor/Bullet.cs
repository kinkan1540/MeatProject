using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oikake.Device;
using Microsoft.Xna.Framework;
using Oikake.Def;
namespace Oikake
{
    class Bullet
    {
        private Vector2 position;
        private bool isDead;
        Vector2 velocity = Vector2.Zero;
        public Bullet(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
            isDead = false;
        }
        public void Update(GameTime gameTime)
        {
            if (position.X==Screen.Width)
            {
                isDead = true;
            }
            if (position.X == Screen.Height)
            {
                isDead = true;
            }
            if (position.X == Screen.Null-64)
            {
                isDead = true;
            }
            if (position.Y == Screen.Null-64)
            {
                isDead = true;
            }
            if (velocity.Length()!=0)
            {
                velocity.Normalize();
            }
            float speed = 8.0f;
            position = position + velocity * speed;
        }
        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("", position);
            if(isDead==true)
            {
                renderer.End();
            }
        }
        public bool IsDead() { return isDead; }

    }
}
