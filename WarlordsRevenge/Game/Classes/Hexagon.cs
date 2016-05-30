using System;
using Microsoft.Xna.Framework;

namespace WarlordsRevenge.Classes
{
    public struct Hexagon
    {
        public static Vector2[] GetCorners(Vector2 center)
        {
            var corners = new Vector2[6];
            for (int i = 1; i <= 6; i++)
            {
                Vector2 corner = GetCorner(i);
                var p = new Vector2 { X = center.X + corner.X, Y = center.Y + corner.Y };
                corners[i - 1] = p;
            }

            return corners;
        }

        private static Vector2 GetCorner(int corner)
        {
            Vector2 p;
            switch (corner)
            {
                case 1:
                    p = GetCorner1();
                    break;
                case 2:
                    p = GetCorner2();
                    break;
                case 3:
                    p = GetCorner3();
                    break;
                case 4:
                    p = GetCorner4();
                    break;
                case 5:
                    p = GetCorner5();
                    break;
                case 6:
                    p = GetCorner6();
                    break;
                default:
                    throw new NotSupportedException(string.Format("Corner {0} is not supported.", corner));
            }

            return p;
        }

        private static Vector2 GetCorner1()
        {
            float x = -Constants.HALF_HEX_WIDTH;
            float y = 0.0f;

            return new Vector2(x, y);
        }

        private static Vector2 GetCorner2()
        {
            float x = -Constants.QUARTER_HEX_WIDTH;
            float y = -Constants.HALF_HEX_HEIGHT;

            return new Vector2(x, y);
        }

        private static Vector2 GetCorner3()
        {
            float x = Constants.QUARTER_HEX_WIDTH;
            float y = -Constants.HALF_HEX_HEIGHT;

            return new Vector2(x, y);
        }

        private static Vector2 GetCorner4()
        {
            float x = Constants.HALF_HEX_WIDTH;
            float y = 0.0f;

            return new Vector2(x, y);
        }

        private static Vector2 GetCorner5()
        {
            float x = Constants.QUARTER_HEX_WIDTH;
            float y = Constants.HALF_HEX_HEIGHT;

            return new Vector2(x, y);
        }

        private static Vector2 GetCorner6()
        {
            float x = -Constants.QUARTER_HEX_WIDTH;
            float y = Constants.HALF_HEX_HEIGHT;

            return new Vector2(x, y);
        }
    }
}