using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GoatProblem
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager myGraphics;
        private SpriteBatch mySpriteBatch;
        private Texture2D circleTexture;
        private Circle A;
        private Circle B;
        private Camera myCamera;
        private Texture2D mySquare;
        public static Vector2 AccessScreenSize { get; private set; }
        private Vector2 P1, P2 = Vector2.Zero;

        private SpriteFont myFont;

        private Texture2D Intersects;

        public Game1()
        {
            myGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            myGraphics.PreferredBackBufferWidth = 1280;
            myGraphics.PreferredBackBufferHeight = 720;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            myCamera = new Camera(new Viewport(new Rectangle(myGraphics.PreferredBackBufferWidth / 2, myGraphics.PreferredBackBufferHeight / 2, myGraphics.PreferredBackBufferWidth, myGraphics.PreferredBackBufferHeight)), new Rectangle(-myGraphics.PreferredBackBufferWidth, -myGraphics.PreferredBackBufferHeight, myGraphics.PreferredBackBufferWidth * 2, myGraphics.PreferredBackBufferHeight * 2));
            myCamera.UpdateCamera(Vector2.Zero);
            Input.setCameraStuff(myCamera);
            mySquare = new Texture2D(GraphicsDevice, 1, 1);
            Color[] colorData = new Color[1];
            colorData[0] = Color.White;
            mySquare.SetData(colorData);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            mySpriteBatch = new SpriteBatch(GraphicsDevice);
            A = new Circle(500, new Vector2(), GraphicsDevice);
            B = new Circle(400, new Vector2(500, 0), GraphicsDevice); //579.364288f

            circleTexture = StaticMethods.CreateCircleTex(10, GraphicsDevice);

            myFont = Content.Load<SpriteFont>("font");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Input.GetState(Window);
            myCamera.AccessZoom = (float)(Input.clampedScrollWheelValue * 0.001) + 1;
            myCamera.UpdateMouse();
            // TODO: Add your update logic here
            IntersectionPoints();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            mySpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, transformMatrix: myCamera.transform);
            StaticMethods.DrawGrid(50, 5, mySpriteBatch, myCamera, Window, mySquare);
            A.Draw(mySpriteBatch, Color.Green);
            B.Draw(mySpriteBatch, Color.Red);
            mySpriteBatch.Draw(circleTexture, Input.myWorldMousePos - new Vector2(circleTexture.Width / 2), Color.Orange);
            mySpriteBatch.Draw(circleTexture, P1 - new Vector2(circleTexture.Width / 2), Color.Orange);
            mySpriteBatch.Draw(circleTexture, P2 - new Vector2(circleTexture.Width / 2), Color.Orange);

            mySpriteBatch.End();
            mySpriteBatch.Begin();
            mySpriteBatch.DrawString(myFont, "Area A: " + A.Area(), Vector2.Zero, Color.White);
            mySpriteBatch.DrawString(myFont, "Area B: " + B.Area(), new Vector2(0, 30), Color.White);
            mySpriteBatch.DrawString(myFont, "Area intersect: " + Area(), new Vector2(0, 60), Color.White);
            mySpriteBatch.DrawString(myFont, "Area A-intesect: " + (A.Area() - Area()), new Vector2(0, 90), Color.White);
            mySpriteBatch.DrawString(myFont, "Area Difference: " + (A.Area() - Area() * 2), new Vector2(0, 120), Color.White);

            mySpriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void IntersectionPoints()
        {
            float d = AdvancedMath.Hypot(B.x - A.x, B.y - A.y);
            if (d <= A.r + B.r && d >= MathF.Abs(B.r - A.r))
            {
                float ex = (B.x - A.x) / d;
                float ey = (B.y - A.y) / d;

                float x = (A.r * A.r - B.r * B.r + d * d) / (2 * d);
                float y = MathF.Sqrt(A.r * A.r - x * x);
                P1 = new Vector2(A.x + x * ex - y * ey, A.y + x * ey + y * ex);
                P2 = new Vector2(A.x + x * ex + y * ey, A.y + x * ey - y * ex);
            }
            else
            {
                // No Intersection, far outside or one circle within the other
                P1 = P2 = Vector2.Zero;
            }
        }

        private float Area()
        {
            float d = AdvancedMath.Hypot(B.x - A.x, B.y - A.y);

            if (d < A.r + B.r)
            {
                float a = A.r * A.r;
                float b = B.r * B.r;

                float x = (a - b + d * d) / (2 * d);
                float z = x * x;
                float y = MathF.Sqrt(a - z);

                if (d <= MathF.Abs(B.r - A.r))
                {
                    return MathF.PI * MathF.Min(a, b);
                }
                return a * MathF.Asin(y / A.r) + b * MathF.Asin(y / B.r) - y * (x + MathF.Sqrt(z + b - a));
            }
            return 0;
        }
    }
}