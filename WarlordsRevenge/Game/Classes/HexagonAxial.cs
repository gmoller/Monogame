﻿using Microsoft.Xna.Framework;

namespace WarlordsRevenge.Classes
{
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
    }
}