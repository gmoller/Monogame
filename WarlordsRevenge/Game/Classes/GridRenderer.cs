using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using WarlordsRevenge.Grid;
using WarlordsRevenge.Hexagons;
using FarSeerCamera2D = WarlordsRevenge.FarseerSamples.Camera2D;

namespace WarlordsRevenge.Classes
{
    public class GridRenderer
    {
        private readonly Rectangle _viewport;

        public GridRenderer(Rectangle viewport)
        {
            _viewport = viewport;
            const int fuzz = 40;
            _viewport = new Rectangle(viewport.X - fuzz, viewport.Y - fuzz, viewport.Width + (fuzz * 2), viewport.Height + (fuzz * 2));
        }

        public bool ShowGridLines { get; set; }

        public void Draw(SpriteBatch spriteBatch, HexagonGrid grid, Images images, FarSeerCamera2D camera)
        {
            foreach (Cell cell in grid)
            {
                Vector2 centerPixel = DeterminePositionToDraw(cell);
                Vector2 screenPosition = camera.ConvertWorldToScreen(centerPixel);
                if (_viewport.Contains(screenPosition))
                {
                    DrawCell(spriteBatch, cell, images, centerPixel);
                }
            }
        }

        private Vector2 DeterminePositionToDraw(Cell cell)
        {
            HexagonAxial axial = cell.ToAxial();
            Vector2 centerPixel = axial.HexToPixel();

            return centerPixel;
        }

        private void DrawCell(SpriteBatch spriteBatch, Cell cell, Images images, Vector2 centerPixel)
        {
            byte i = 0;
            byte? paletteId = cell.GetPaletteId(i);
            byte? terrainId = cell.GetTerrainId(i);
            Vector2 topLeftPixel = centerPixel - new Vector2(Constants.HALF_HEX_WIDTH, Constants.HALF_HEX_HEIGHT);

            while (paletteId != null)
            {
                Texture2D texture = images.GetImage(paletteId.GetValueOrDefault(), terrainId.GetValueOrDefault());
                spriteBatch.Draw(texture, topLeftPixel, Color.White);
                paletteId = cell.GetPaletteId(i);
                terrainId = cell.GetTerrainId(i);
                i++;
            }

            if (ShowGridLines)
            {
                DrawHexagonOutline(spriteBatch, centerPixel, 1.0f, Color.DarkBlue);
            }
        }

        public void DrawHexagonOutline(SpriteBatch spriteBatch, Vector2 centerPixel, float thickness, Color color)
        {
            Vector2[] corners = Hexagon.GetCorners(centerPixel);

            spriteBatch.DrawLine(corners[0], corners[1], color, thickness);
            spriteBatch.DrawLine(corners[1], corners[2], color, thickness);
            spriteBatch.DrawLine(corners[2], corners[3], color, thickness);
            spriteBatch.DrawLine(corners[3], corners[4], color, thickness);
            spriteBatch.DrawLine(corners[4], corners[5], color, thickness);
            spriteBatch.DrawLine(corners[5], corners[0], color, thickness);
        }
    }
}