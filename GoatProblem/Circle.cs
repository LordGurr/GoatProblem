using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoatProblem
{
    internal class Circle
    {
        private float radius;
        private Vector2 center;
        public Texture2D texture;
        public float x { get => center.X; }
        public float y { get => center.Y; }
        public float r { get => radius; }

        public Circle(float radius, Vector2 center, GraphicsDevice graphicsDevice)
        {
            this.radius = radius;
            this.center = center;
            this.texture = StaticMethods.CreateCircleTex((int)radius, graphicsDevice);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            // texture.Width* x = 2 * radius;
            //x = (2*radius)/texture.Width;
            spriteBatch.Draw(texture, center - new Vector2(radius), color * 0.7f);
        }

        public float Area()
        {
            return radius * radius * MathF.PI;
        }
    }
}