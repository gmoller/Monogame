using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WarlordsRevenge.Classes
{
    public class MapReader
    {
        public static HexagonGrid ReadFromFile(string path)
        {
            string text = File.ReadAllText(path, Encoding.UTF8);
            text = text.Replace("\r", string.Empty);
            string[] lines = text.Split('\n');
            HexagonGrid grid = ParseLines(lines);

            return grid;
        }

        private static HexagonGrid ParseLines(IEnumerable<string> lines)
        {
            HexagonGrid grid = null;
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.StartsWith("Size: "))
                    {
                        grid = ParseSizeLine(line);
                    }
                    else
                    {
                        ParseCellLine(line, grid);
                    }
                }
            }

            return grid;
        }

        private static HexagonGrid ParseSizeLine(string line)
        {
            short size = Convert.ToInt16(line.Remove(0, 6));
            var grid = new HexagonGrid(size);

            return grid;
        }

        private static void ParseCellLine(string line, HexagonGrid grid)
        {
            string[] pieces = line.Split(':');

            HexagonAxial axial = GetHexagon(pieces[0]);
            List<CellData> cellData = GetCellData(pieces[1]);

            grid.SetCell(axial, cellData);
        }

        private static HexagonAxial GetHexagon(string coordsString)
        {
            string[] coords = coordsString.Split(';');
            float q = Convert.ToSingle(coords[0]);
            float r = Convert.ToSingle(coords[1]);

            var axial = new HexagonAxial(q, r);

            return axial;
        }

        private static List<CellData> GetCellData(string cellDataString)
        {
            var cellData = new List<CellData>();

            string[] pieces = cellDataString.Split('|');
            foreach (string piece in pieces)
            {
                string[] subPieces = piece.Split(';');
                byte paletteId = Convert.ToByte(subPieces[0]);
                short terrainId = Convert.ToInt16(subPieces[1]);
                if (terrainId >= 0)
                {
                    cellData.Add(new CellData {PaletteId = paletteId, TerrainId = (byte)terrainId});
                }
            }

            return cellData;
        }
    }
}