namespace WarlordsRevenge.Classes
{
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

        //public HexagonOffset ToOffset() // odd-q
        //{
        //    short q = X;
        //    int r = Z + (X - (IsEven(X) ? 0 : 1)) / 2;
        //    var offset = new HexagonOffset(q, r);

        //    return offset;
        //}

        //private bool IsEven(short x)
        //{
        //    int i = (x & 1);

        //    return (i == 0);
        //}
    }
}