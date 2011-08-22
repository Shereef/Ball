using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Ball
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D ballTexture;                   // image file
        static int ballcount;
        Ball ball1;
        Ball ball2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ballcount = 0;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ballTexture = Content.Load<Texture2D>("Images\\ball");
            ball1 = new Ball(++ballcount, ballTexture, new Vector2(100, 100));
            ball2 = new Ball(++ballcount, ballTexture, new Vector2(400, 100));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            ball1.Move(gameTime, Window.ClientBounds.Width, Window.ClientBounds.Height);
            ball2.Move(gameTime, Window.ClientBounds.Width, Window.ClientBounds.Height);
            CheckCollisions();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend); // start drawing 2D images

            spriteBatch.Draw(ball1.Texture, ball1.Position, null, Color.White,
                                 0, ball1.Center, 1.0f, SpriteEffects.None, 0.0f);

            spriteBatch.Draw(ball2.Texture, ball2.Position, null, Color.White,
                                 0, ball2.Center, 1.0f, SpriteEffects.None, 0.0f);

            spriteBatch.End();                             // stop drawing 2D images

            base.Draw(gameTime);
        }

        private void CheckCollisions()
        {
            // transform the rectangles which surround each sprite
            Matrix ball1Transform = Transform(ball1.Center, 0, ball1.Position), ball2Transform = Transform(ball2.Center, 0, ball2.Position);
            Rectangle ball1Rectangle = TransformRectangle(ball1Transform, ball1.Width, ball1.Height), ball2Rectangle = TransformRectangle(ball2Transform, ball2.Width, ball2.Height);

             // collision checking
            if (ball1Rectangle.Intersects(ball2Rectangle)) // rough collision check
                if (PixelCollision( // exact collision check
                                    ball1Transform, ball1.Width, ball1.Height,
                                    ball2Transform, ball2.Width, ball2.Height))
                {
                    ball1.InvertSpeedX();
                    ball2.InvertSpeedX();
                }
        }

        public Matrix Transform(Vector2 center, float rotation, Vector2 position)
        {
            // move to origin, scale (if desired), rotate, translate
            return Matrix.CreateTranslation(new Vector3(-center, 0.0f)) *
                // add scaling here if you want
                                            Matrix.CreateRotationZ(rotation) *
                                            Matrix.CreateTranslation(new Vector3(position, 0.0f));
        }

        public static Rectangle TransformRectangle(Matrix transform, int width, int height)
        {
            // Get each corner of texture
            Vector2 leftTop = new Vector2(0.0f, 0.0f);
            Vector2 rightTop = new Vector2(width, 0.0f);
            Vector2 leftBottom = new Vector2(0.0f, height);
            Vector2 rightBottom = new Vector2(width, height);

            // Transform each corner
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum corners
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
            Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
            Vector2.Max(leftBottom, rightBottom));

            // Return transformed rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        public bool PixelCollision
        (
            Matrix transformA, int pixelWidthA, int pixelHeightA,
            Matrix transformB, int pixelWidthB, int pixelHeightB)
        {

            // set A transformation relative to B. B remains at x=0, y=0.
            Matrix AtoB = transformA * Matrix.Invert(transformB);

            // generate a perpendicular vectors to each rectangle side
            Vector2 columnStep, rowStep, rowStartPosition;

            columnStep = Vector2.TransformNormal(Vector2.UnitX, AtoB);
            rowStep = Vector2.TransformNormal(Vector2.UnitY, AtoB);

            // calculate the top left corner of A
            rowStartPosition = Vector2.Transform(Vector2.Zero, AtoB);

            // search each row of pixels in A. start at top and move down.
            for (int rowA = 0; rowA < pixelHeightA; rowA++)
            {
                // begin at the left
                Vector2 pixelPositionA = rowStartPosition;

                // for each column in the row (move left to right)
                for (int colA = 0; colA < pixelWidthA; colA++)
                {
                    // get the pixel position
                    int X = (int)Math.Round(pixelPositionA.X);
                    int Y = (int)Math.Round(pixelPositionA.Y);

                    // if the pixel is within the bounds of B
                    if (X >= 0 && X < pixelWidthB && Y >= 0 && Y < pixelHeightB)
                    {
                        // get colors of overlapping pixels
                        Color colorA = ball1.Color[X + Y * pixelWidthB];
                        Color colorB = ball2.Color[X + Y * pixelWidthB];

                        // if both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                            return true; // collision
                    }
                    // move to the next pixel in the row of A
                    pixelPositionA += columnStep;
                }
                // move to the next row of A
                rowStartPosition += rowStep;
            }
            return false; // no collision
        }
    }
}
