using System;
using Microsoft.Xna.Framework;
using WarlordsRevenge.Classes;

namespace WarlordsRevenge.Hexagons
{
    public struct Hexagon
    {
        /// <summary>
        /// Returns the 6 vertices of a hexagon relative to the center of a hexagon.
        /// </summary>
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
            const float x = -Constants.HALF_HEX_WIDTH;
            const float y = 0.0f;

            return new Vector2(x, y);
        }

        private static Vector2 GetCorner2()
        {
            const float x = -Constants.QUARTER_HEX_WIDTH;
            const float y = -Constants.HALF_HEX_HEIGHT;

            return new Vector2(x, y);
        }

        private static Vector2 GetCorner3()
        {
            const float x = Constants.QUARTER_HEX_WIDTH;
            const float y = -Constants.HALF_HEX_HEIGHT;

            return new Vector2(x, y);
        }

        private static Vector2 GetCorner4()
        {
            const float x = Constants.HALF_HEX_WIDTH;
            const float y = 0.0f;

            return new Vector2(x, y);
        }

        private static Vector2 GetCorner5()
        {
            const float x = Constants.QUARTER_HEX_WIDTH;
            const float y = Constants.HALF_HEX_HEIGHT;

            return new Vector2(x, y);
        }

        private static Vector2 GetCorner6()
        {
            const float x = -Constants.QUARTER_HEX_WIDTH;
            const float y = Constants.HALF_HEX_HEIGHT;

            return new Vector2(x, y);
        }
    }
}