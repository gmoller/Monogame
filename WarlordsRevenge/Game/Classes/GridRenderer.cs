using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.ViewportAdapters;

namespace WarlordsRevenge.Classes
{
    public class GridRenderer
    {
        private const float MOVEMENT_SPEED = 200.0f;

        private readonly Camera2D _camera;
        public Camera2D Camera { get { return _camera; } }

        private Rectangle _viewport;

        public bool ShowGridLines { get; set; }

        public GridRenderer(ViewportAdapter adapter)
        {
            _camera = new Camera2D(adapter);
            _viewport = new Rectangle(0, 0, 500, 500);
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = gameTime.GetElapsedSeconds();

            if (InputManager.IsKeyPressed(Keys.W) || InputManager.IsKeyPressed(Keys.Up))
            {
                _camera.Move(new Vector2(0, -MOVEMENT_SPEED) * deltaTime);
            }

            if (InputManager.IsKeyPressed(Keys.A) || InputManager.IsKeyPressed(Keys.Left))
            {
                _camera.Move(new Vector2(-MOVEMENT_SPEED, 0) * deltaTime);
            }

            if (InputManager.IsKeyPressed(Keys.S) || InputManager.IsKeyPressed(Keys.Down))
            {
                _camera.Move(new Vector2(0, MOVEMENT_SPEED) * deltaTime);
            }

            if (InputManager.IsKeyPressed(Keys.D) || InputManager.IsKeyPressed(Keys.Right))
            {
                _camera.Move(new Vector2(MOVEMENT_SPEED, 0) * deltaTime);
            }

            const float maxNumberOfHexagonsCanScrollLeftRight = Constants.THREE_QUARTERS_HEX_WIDTH * 4;
            const float maxNumberOfHexagonsCanScrollUpDown = Constants.HEX_HEIGHT * 2;
            if (_camera.Position.X > maxNumberOfHexagonsCanScrollLeftRight)
            {
                _camera.Position = new Vector2(maxNumberOfHexagonsCanScrollLeftRight, _camera.Position.Y);
            }
            else if (_camera.Position.X < -maxNumberOfHexagonsCanScrollLeftRight)
            {
                _camera.Position = new Vector2(-maxNumberOfHexagonsCanScrollLeftRight, _camera.Position.Y);
            }

            if (_camera.Position.Y > maxNumberOfHexagonsCanScrollUpDown)
            {
                _camera.Position = new Vector2(_camera.Position.X, maxNumberOfHexagonsCanScrollUpDown);
            }
            else if (_camera.Position.Y < -maxNumberOfHexagonsCanScrollUpDown)
            {
                _camera.Position = new Vector2(_camera.Position.X, -maxNumberOfHexagonsCanScrollUpDown);
            }
        }

        public void Draw(SpriteBatch spriteBatch, HexagonGrid grid, Images images)
        {
            var transformMatrix = _camera.GetViewMatrix(Vector2.One);
            spriteBatch.Begin(transformMatrix: transformMatrix);

            float xOffset = _camera.Origin.X - Constants.HALF_HEX_WIDTH;
            float yOffset = _camera.Origin.Y - Constants.HALF_HEX_HEIGHT;

            foreach (Cell cell in grid)
            {
                Vector2 position = DeterminePositionToDraw(cell, xOffset, yOffset);
                //if (_viewport.Intersects(new Rectangle((int)position.X, (int)position.Y, 72, 72)))
                {
                    DrawCell(spriteBatch, cell, images, position);
                }
            }

            spriteBatch.End();
        }

        private Vector2 DeterminePositionToDraw(Cell cell, float xOffset, float yOffset)
        {
            HexagonAxial axial = cell.ToAxial();
            Vector2 centerPixel = axial.HexToPixel();
            var position = new Vector2(centerPixel.X + xOffset, centerPixel.Y + yOffset);

            return position;
        }

        private void DrawCell(SpriteBatch spriteBatch, Cell cell, Images images, Vector2 position)
        {
            byte i = 0;
            byte? paletteId = cell.GetPaletteId(i);
            byte? terrainId = cell.GetTerrainId(i);
            while (paletteId != null)
            {
                spriteBatch.Draw(images.GetImage(paletteId.GetValueOrDefault(), terrainId.GetValueOrDefault()), position, Color.White);
                paletteId = cell.GetPaletteId(i);
                terrainId = cell.GetTerrainId(i);
                i++;
            }

            if (ShowGridLines)
            {
                DrawHexagonOutline(spriteBatch, position + new Vector2(Constants.HALF_HEX_WIDTH, Constants.HALF_HEX_HEIGHT));
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