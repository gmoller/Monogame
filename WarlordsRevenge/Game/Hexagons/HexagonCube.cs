using System;
using Microsoft.Xna.Framework;

namespace WarlordsRevenge.Hexagons
{
    /// <summary>
    /// Representation of a hexagon using cube coordinates.
    /// </summary>
    public struct HexagonCube
    {
        private readonly float _x;
        private readonly float _y;
        private readonly float _z;

        public float X { get { return _x; } }
        public float Y { get { return _y; } }
        public float Z { get { return _z; } }

        public HexagonCube(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public HexagonAxial ToAxial()
        {
            float q = X;
            float r = Z;
            var axial = new HexagonAxial(q, r);

            return axial;
        }

        /// <summary>
        /// Returns the center pixel of a hexagon
        /// </summary>
        public Vector2 HexToPixel()
        {
            HexagonAxial axial = ToAxial();
            Vector2 pixel = axial.HexToPixel();

            return pixel;
        }

        public HexagonCube Round()
        {
            double rx = Math.Round(X);
            double ry = Math.Round(Y);
            double rz = Math.Round(Z);

            double xDiff = Math.Abs(rx - X);
            double yDiff = Math.Abs(ry - Y);
            double zDiff = Math.Abs(rz - Z);

            if (xDiff > yDiff && xDiff > zDiff)
            {
                rx = -ry - rz;
            }
            else if (yDiff > zDiff)
            {
                ry = -rx - rz;
            }
            else
            {
                rz = -rx - ry;
            }

            return new HexagonCube((float)rx, (float)ry, (float)rz);
        }
    }
}