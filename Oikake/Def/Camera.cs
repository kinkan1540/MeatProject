using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Oikake.Def
{
    class Camera
    {
        private Matrix transform;
        public Matrix Transform
        {
            get { return transform; }
        }
        private Vector2 centre;
        private Viewport viewport;

        private static float zoom =0.0f;
        private float roatation = 0;
        public float X
        {
            get { return centre.X; }
            set { centre.X = value; }
        }
        public float Y
        {
            get { return centre.Y; }
            set { centre.Y = value; }
        }

        public static float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                if (zoom < 0.1f)
                    zoom = 0.1f;
            }
        }

        public float Rotation
        {
            get { return roatation; }
            set { roatation = value; }
        }
        public Camera(Viewport newViewport)
        {
            viewport = newViewport;
        }

        public void Update(Vector2 position)
        {
            centre = new Vector2(position.X, position.Y);
            transform = Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0)) *
                                                Matrix.CreateRotationZ(Rotation) *
                                                Matrix.CreateScale(new Vector3(zoom, zoom, 0)) *
                                                Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
        }
    }
}
