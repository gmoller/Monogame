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
        public bool ShowGridLines { get; set; }

        public void Draw(SpriteBatch spriteBatch, HexagonGrid grid, Images images, FarSeerCamera2D camera)
        {
            Matrix transformMatrix = camera.SimView;
            spriteBatch.Begin(transformMatrix: transformMatrix);

            foreach (Cell cell in grid)
            {
                Vector2 centerPixel = DeterminePositionToDraw(cell);
                DrawCell(spriteBatch, cell, images, centerPixel, camera);
            }

            spriteBatch.End();
        }

        private Vector2 DeterminePositionToDraw(Cell cell)
        {
            HexagonAxial axial = cell.ToAxial();
            Vector2 centerPixel = axial.HexToPixel();

            return centerPixel;
        }

        private void DrawCell(SpriteBatch spriteBatch, Cell cell, Images images, Vector2 centerPixel, FarSeerCamera2D camera)
        {
            byte i = 0;
            byte? paletteId = cell.GetPaletteId(i);
            byte? terrainId = cell.GetTerrainId(i);
            Vector2 topLeftPixel = centerPixel - new Vector2(Constants.HALF_HEX_WIDTH, Constants.HALF_HEX_HEIGHT);

            if (camera.ConvertWorldToScreen(centerPixel).X > 1300)
            {
                return;
            }

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
                DrawHexagonOutline(spriteBatch, centerPixel);
            }
        }

        private void DrawHexagonOutline(SpriteBatch spriteBatch, Vector2 centerPixel)
        {
            Vector2[] corners = Hexagon.GetCorners(centerPixel);

            Color color = Color.DarkBlue;
            const float thickness = 2.0f;
            spriteBatch.DrawLine(corners[0], corners[1], color, thickness);
            spriteBatch.DrawLine(corners[1], corners[2], color, thickness);
            spriteBatch.DrawLine(corners[2], corners[3], color, thickness);
            spriteBatch.DrawLine(corners[3], corners[4], color, thickness);
            spriteBatch.DrawLine(corners[4], corners[5], color, thickness);
            spriteBatch.DrawLine(corners[5], corners[0], color, thickness);
        }
    }
}