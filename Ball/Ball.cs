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
        Vector2 position = new Vector2(100.0f, 100.0f);
        float rotation;
        Vector2 center;
        int width, height;
        Color[] color;
        public Ball(int ballcount, Texture2D ballTexture)
        {
            id = ballcount;
            texture = ballTexture;
            width = texture.Width;
            height = texture.Height;
            center = new Vector2(width / 2, height / 2);
            color = new Color[texture.Width * texture.Height];
            texture.GetData(color);
        }
    }
}
