using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//using Pathfinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoatProblem
{
    internal static class StaticMethods
    {
        public static Vector2Int ToLocation(Vector2 vector2)
        {
            return new Vector2Int((int)vector2.X, (int)vector2.Y);
        }

        public static void DrawString(Vector2Int cell, string temp, int size, SpriteBatch spritebatch, SpriteFont font)
        {
            Vector2 pos = ((cell.ToVector2() + new Vector2(0.5f, 0.7f)) * size - font.MeasureString(temp) / 2);
            spritebatch.DrawString(font, temp, pos, Color.Black);
        }

        public static void DrawString(Vector2Int cell, string temp, int size, SpriteBatch spritebatch, SpriteFont font, Color color)
        {
            Vector2 pos = ((cell.ToVector2() + new Vector2(0.8f, 1.9f)) * size - font.MeasureString(temp) / 2);
            spritebatch.DrawString(font, temp, pos, color, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0);
        }

        //public static  Rectangle operator /(Rectangle a, int b)
        //{
        //    return new Rectangle(a.X, a.Y, a.Width / b, a.Height / b);
        //}

        public static Rectangle Divide(Rectangle a, int b)
        {
            return new Rectangle(a.X, a.Y, a.Width / b, a.Height / b);
        }

        //private void DrawString(Location cell, string temp, int size)
        //{
        //    Vector2 pos = ((cell.ToVector2() + new Vector2(0.5f, 0.7f)) * size - myFont.MeasureString(temp) / 2);
        //    mySpriteBatch.DrawString(myFont, temp, pos, Color.Black);
        //}

        //private Location ToLocation(Vector2 vector2)
        //{
        //    return new Location((int)vector2.X, (int)vector2.Y);
        //}

        public static void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, int width, Texture2D mySquare, Color color)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);

            sb.Draw(mySquare, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), width), null, color, angle, new Vector2(0, 0), SpriteEffects.None, 0);
            /*
             sb.Draw(t,
                  new Rectangle(// rectangle defines shape of line and position of start of line
                      (int)start.X,
                      (int)start.Y,
                      (int)edge.Length(), //sb will strech the texture to fill this rectangle
                      1), //width of line, change this to make thicker line
                  null,
                  Color.Red, //colour of line
                  angle,     //angle of line (calulated above)
                  new Vector2(0, 0), // point in line about which to rotate
                  SpriteEffects.None,
                  0);
                  */
        }

        public static void DrawArrow(SpriteBatch sb, Vector2 start, Vector2 end, int width, Texture2D mySquare)
        {
            DrawLine(sb, start, end, width, mySquare, Color.Black);
            float lerp = 10 / AdvancedMath.Magnitude(end - start);
            Vector2 lerped = AdvancedMath.Lerp(end, start, lerp);
            Vector2 one = AdvancedMath.RotateAboutOrigin(lerped, end, 0.15f * MathF.PI);
            DrawLine(sb, end, one, width, mySquare, Color.Black);
            Vector2 two = AdvancedMath.RotateAboutOrigin(lerped, end, -0.15f * MathF.PI);
            DrawLine(sb, end, two, width, mySquare, Color.Black);
            sb.Draw(mySquare, two, Color.Red);
            sb.Draw(mySquare, one, Color.Red);
            sb.Draw(mySquare, lerped, Color.Green);
        }

        public static void DrawGraph(SpriteBatch sb, Rectangle rect, float[] data, SpriteFont font, Texture2D mySquare)
        {
            float average = 0;
            for (int i = 0; i < data.Length; i++)
            {
                average += data[i];
            }
            average /= data.Length;
            float max = Max(data);
            float min = Min(data);
            float dataWidth = (float)rect.Width / (float)data.Length;
            float[] normalisedData = Normalize(data, min, max);
            sb.Draw(mySquare, rect, Color.DarkGreen);
            for (int i = 0; i < normalisedData.Length - 1; i++)
            {
                DrawLine(sb, new Vector2(dataWidth * i + rect.X, rect.Y + rect.Height * Invert(normalisedData[i])), new Vector2(dataWidth * (i + 1) + rect.X, rect.Y + rect.Height * Invert(normalisedData[i + 1])), 1, mySquare, Color.Black);
            }
            if (float.IsFinite(max))
            {
                Vector2 temp = font.MeasureString(max.ToString("F1"));
                sb.DrawString(font, max.ToString("F1"), new Vector2(rect.X - temp.X, rect.Y), Color.Black);
            }
            if (float.IsFinite(min))
            {
                Vector2 temp = font.MeasureString(min.ToString("F1"));
                sb.DrawString(font, min.ToString("F1"), new Vector2(rect.X - temp.X, rect.Y + rect.Height - temp.Y), Color.Black);
            }
            if (float.IsFinite(average))
            {
                Vector2 temp = font.MeasureString(average.ToString("F1"));
                sb.DrawString(font, average.ToString("F1"), new Vector2(rect.X - temp.X, rect.Y + (rect.Height - temp.Y) / 2), Color.Black);
            }
        }

        private static float Invert(float temp)
        {
            //return (float)Math.Atan2(temp, temp);
            return MathF.Abs(temp - 1);
        }

        private static float[] Normalize(float[] data, float min, float max)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (data[i] - min) / (max - min);
            }
            return data;
        }

        private static float Max(float[] input)
        {
            float max = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (max < input[i])
                {
                    max = input[i];
                }
            }
            return max;
        }

        private static float Min(float[] input)
        {
            float min = float.MaxValue;
            for (int i = 0; i < input.Length; i++)
            {
                if (min > input[i])
                {
                    min = input[i];
                }
            }
            return min;
        }

        public static Texture2D Box(GraphicsDevice graphics, int width, int height)
        {
            Texture2D box = new Texture2D(graphics, width, height);
            Color[] colorData = new Color[width * height];
            for (int x = 0; x < width; x++)
            {
                int index = x;
                colorData[index] = Color.White;
                index = x + (height - 1) * width;
                colorData[index] = Color.White;
            }
            for (int y = 0; y < height; y++)
            {
                int index = y * width;
                colorData[index] = Color.White;
                index = width - 1 + y * width;
                colorData[index] = Color.White;
            }
            colorData[0] = Color.White;
            box.SetData(colorData);
            return box;
        }

        public static Texture2D FixTexture(GraphicsDevice graphics, Texture2D textureIN)
        {
            Texture2D textureOUT = new Texture2D(graphics, textureIN.Width, textureIN.Height);
            Color[] textureOUTData = new Color[(textureIN.Width) * (textureIN.Height)];

            Color[] textureINData = new Color[(textureIN.Width) * (textureIN.Height)];
            textureIN.GetData(textureINData);

            //float diam = aRadius / 2f;
            //float diamsq = diam * diam;

            for (int x = 0; x < textureIN.Width; x++)
            {
                for (int y = 0; y < textureIN.Height; y++)
                {
                    int index = x + y * textureIN.Width;
                    Color colourA = textureINData[index];
                    if (colourA == new Color(89, 255, 85))
                    {
                        colourA = Color.Black * 0.5f;
                    }
                    else if (colourA == new Color(0, 0, 0))
                    {
                        colourA = Color.Transparent;
                    }
                    textureOUTData[index] = colourA;

                    //Vector2 pos = new Vector2(x - diam, y - diam);
                    //if (pos.LengthSquared() <= diamsq)
                    //{
                    //    colorData[index] = Color.White;
                    //}
                    //else
                    //{
                    //    colorData[index] = Color.Transparent;
                    //}
                }
            }

            textureOUT.SetData(textureOUTData);
            return textureOUT;
        }

        public static string Vector2ToString(Vector2 vector2)
        {
            return "{X:" + vector2.X.ToString("F1") + " Y:" + vector2.Y.ToString("F1") + "}";
        }

        /// <summary>
        /// Skapar en cirkel textur.
        /// </summary>
        public static Texture2D CreateCircleTex(int aRadius, GraphicsDevice graphicsDevice)
        {
            Texture2D texture = new Texture2D(graphicsDevice, aRadius, aRadius);
            Color[] colorData = new Color[aRadius * aRadius];

            float diam = aRadius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < aRadius; x++)
            {
                for (int y = 0; y < aRadius; y++)
                {
                    int index = x * aRadius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}