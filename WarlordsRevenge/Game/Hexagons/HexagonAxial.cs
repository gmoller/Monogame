using Microsoft.Xna.Framework;
using WarlordsRevenge.Classes;

namespace WarlordsRevenge.Hexagons
{
    /// <summary>
    /// Representation of a hexagon using axial coordinates.
    /// </summary>
    public struct HexagonAxial
    {
        private readonly float _q; // column (aligned with x)
        private readonly float _r; // row (aligned with z)

        public float Q { get { return _q; } }
        public float R { get { return _r; } }

        public HexagonAxial(float q, float r)
        {
            _q = q;
            _r = r;
        }

        public HexagonCube ToCube()
        {
            float x = Q;
            float z = R;
            float y = -x - z;
            var hexCube = new HexagonCube(x, y, z);

            return hexCube;
        }

        /// <summary>
        /// Returns the center pixel of a hexagon
        /// </summary>
        public Vector2 HexToPixel()
        {
            float x = Constants.HALF_HEX_WIDTH * 1.5f * Q;
            //double y = Constants.HALF_HEX_HEIGHT * Math.Sqrt(3) * (axial.R + axial.Q / 2);
            float y = Constants.HALF_HEX_HEIGHT * 2.0f * (R + Q / 2.0f);

            return new Vector2(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IncludesPixel(Vector2 pixel)
        {
            //float q = pixel.X * Constants.TWO_THIRDS / Constants.HALF_HEX_WIDTH;
            //double r = (-pixel.X / 3.0f + Constants.HALF * pixel.Y) / Constants.HALF_HEX_HEIGHT;
            //var axial = new HexagonAxial(q, (float)r);
            HexagonAxial axial = pixel.PixelToHex();
            axial = axial.Round();

            return (axial.Q == Q && axial.R == R);
        }

        public HexagonAxial Round()
        {
            HexagonCube cube = ToCube();
            cube = cube.Round();

            return cube.ToAxial();
        }
    }
}