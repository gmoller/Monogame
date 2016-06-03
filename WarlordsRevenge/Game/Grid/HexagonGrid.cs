using System;
using System.Collections;
using System.Collections.Generic;
using WarlordsRevenge.Hexagons;

namespace WarlordsRevenge.Grid
{
    /// <summary>
    /// Represents a grid of hexagons as a 2 dimensional array of Cells.
    /// </summary>
    public class HexagonGrid : IEnumerable<Cell>
    {
        private readonly short _size; // size is the number of concentric circles around the center hexagon
        private readonly short _width;
        private readonly short _height;
        private readonly Cell[,] _cells;

        public HexagonGrid(short size)
        {
            _size = size;
            _width = Convert.ToInt16((size * 2) + 1);
            _height = Convert.ToInt16(size + 1);
            short arraySize = _width;
            _cells = new Cell[arraySize, arraySize];
        }

        public void SetCell(HexagonAxial axial, List<CellData> data)
        {
            _cells[(short)axial.Q + _size, (short)axial.R + _size] = new Cell((short)axial.Q, (short)axial.R, data);
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            for (int q = 0; q < _width; q++)
            {
                for (int r = 0; r < _width; r++)
                {
                    Cell cell = _cells[q, r];
                    if (cell != null)
                    {
                        yield return cell;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}