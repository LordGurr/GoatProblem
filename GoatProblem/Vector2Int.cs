using Microsoft.Xna.Framework;
using System;

namespace GoatProblem
{
    internal class Vector2Int
    {
        private int myX;
        private int myY;

        public int X
        {
            get
            {
                return myX;
            }
            set
            {
                myX = value;
            }
        }

        public int Y
        {
            get
            {
                return myY;
            }
            set
            {
                myY = value;
            }
        }

        public static readonly Vector2Int One = new Vector2Int(1, 1);
        public static readonly Vector2Int Zero = new Vector2Int(0, 0);

        public Vector2Int()
        {
            X = 0;
            Y = 0;
        }

        public Vector2Int(int _x)
        {
            X = _x;
            Y = 0;
        }

        public Vector2Int(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2Int operator *(Vector2Int a, int b)
        {
            return new Vector2Int(a.X * b, a.X * b);
        }

        public static Vector2Int operator *(Vector2Int a, float b)
        {
            return new Vector2Int((int)Math.Round(a.X * b, MidpointRounding.AwayFromZero), (int)Math.Round(a.Y * b, MidpointRounding.AwayFromZero));
        }

        public static Vector2Int operator /(Vector2Int a, int b)
        {
            return new Vector2Int(a.X / b, a.Y / b);
        }

        public static Vector2Int operator /(Vector2Int a, float b)
        {
            return new Vector2Int((int)Math.Round(a.X / b, MidpointRounding.AwayFromZero), (int)Math.Round(a.Y / b, MidpointRounding.AwayFromZero));
        }

        public static Vector2Int operator %(Vector2Int a, int b)
        {
            return new Vector2Int(a.X % b, a.Y % b);
        }

        public static Vector2Int operator %(Vector2Int a, float b)
        {
            return new Vector2Int((int)Math.Round(a.X % b, MidpointRounding.AwayFromZero), (int)Math.Round(a.Y % b, MidpointRounding.AwayFromZero));
        }

        public override string ToString()
        {
            return "{ X: " + myX + " Y: " + myY + " }";
        }

        public Vector2 ToVector2()
        {
            return new Vector2(myX, myX);
        }
    }
}