namespace WarlordsRevenge.Classes
{
    public struct HexagonOffset
    {
        private readonly short _q; // column
        private readonly short _r; // row

        public short Q { get { return _q; } }
        public short R { get { return _r; } }

        public HexagonOffset(short q, short r)
        {
            _q = q;
            _r = r;
        }
    }
}