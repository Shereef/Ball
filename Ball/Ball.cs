using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics; // Color, Texture2D
using Microsoft.Xna.Framework; // Vector

namespace Ball
{
    class Ball
    {
        int id;
        Texture2D texture;

        public Texture2D Texture
        {
            get { return texture; }
        }
        Vector2 position;

        public Vector2 Position
        {
            get { return position; }
        }
        float rotation;

        public float Rotation
        {
            get { return rotation; }
        }
        Vector2 center;

        public Vector2 Center
        {
            get { return center; }
        }
        int width, height;
        Color[] color;
        public Ball(int ballcount, Texture2D ballTexture, Vector2 startPosition)
        {
            id = ballcount;
            texture = ballTexture;
            position = startPosition;
            width = texture.Width;
            height = texture.Height;
            center = new Vector2(width / 2, height / 2);
            color = new Color[texture.Width * texture.Height];
            texture.GetData(color);
        }
    }
}
