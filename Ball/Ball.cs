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
        Vector2 position, speed = new Vector2(0.16f, 0.16f);

        public Vector2 Position
        {
            get { return position; }
        }
        Vector2 center;

        public Vector2 Center
        {
            get { return center; }
        }
        int width, height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        Color[] color;

        public Color[] Color
        {
            get { return color; }
        }
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

        public void Move(GameTime gameTime, int windowWidth, int windowHeight)
        {
            // time between frames
            float timeLapse = (float)gameTime.ElapsedGameTime.Milliseconds;

            // asteroid centered at the middle of the image
            Rectangle safeArea = new Rectangle(0, 0, windowWidth - width,
                                       windowHeight - height);
            // ball right edge exceeds right window edge
            if (position.X > safeArea.Right + center.X)
            {
                position.X = safeArea.Right + center.X; // move it back
                speed.X *= -1.0f;                // reverse direction
            }
            // ball left edge precedes the left window edge
            else if (position.X - center.X < 0)
            {
                position.X = center.X;  // move it back
                speed.X *= -1.0f;                // reverse direction
            }
            // ball within window bounds so update position
            else
                position.X += speed.X * timeLapse;
            // ball top edge exceeds right window edge
            // Sample code to move in Y Axis instead of X
            /*if (position.Y > safeArea.Bottom + center.Y)
            {
                position.Y = safeArea.Bottom + center.Y; // move it back
                speed.Y *= -1.0f;              // reverse direction
            }
            // ball bottom edge precedes the left window edge
            else if (position.Y - center.Y < 0)
            {
                position.Y = center.Y;  // move it back
                speed.Y *= -1.0f;               // reverse direction
            }
            // ball within window bounds so update position
            else
                position.Y += speed.Y * timeLapse;*/
        }

        public void InvertSpeedX()
        {
            speed.X *= -1.0f; 
        }
    }
}
