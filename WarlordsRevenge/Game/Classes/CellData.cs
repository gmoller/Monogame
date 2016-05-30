namespace WarlordsRevenge.Classes
{
    public struct CellData
    {
        public byte PaletteId;
        public byte TerrainId;

        public CellData(byte paletteId, byte terrainId)
        {
            PaletteId = paletteId;
            TerrainId = terrainId;
        } 
    }
}