using System.Collections.Generic;

namespace WarlordsRevenge.Classes
{
    public class Cell
    {
        private readonly short _q; // column (aligned with x)
        private readonly short _r; // row (aligned with z)

        private readonly List<CellData> _data; // holds paletteId and terrainId for each layer

        public Cell(short q, short r, IEnumerable<CellData> data)
        {
            _q = q;
            _r = r;

            _data = new List<CellData>();
            foreach (CellData item in data)
            {
                _data.Add(item);
            }
        }

        public HexagonAxial ToAxial()
        {
            return new HexagonAxial(_q, _r);
        }

        public HexagonCube ToCube()
        {
            return new HexagonCube(_q, -_q - _r, _r);
        }

        public byte? GetPaletteId(byte layerId)
        {
            if (layerId >= _data.Count) return null;

            return _data[layerId].PaletteId;
        }

        public byte? GetTerrainId(byte layerId)
        {
            if (layerId >= _data.Count) return null;

            return _data[layerId].TerrainId;
        }
    }
}