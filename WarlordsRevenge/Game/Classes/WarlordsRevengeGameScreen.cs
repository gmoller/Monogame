using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using WarlordsRevenge.FarseerSamples;
using WarlordsRevenge.Grid;
using FarSeerCamera2D = WarlordsRevenge.FarseerSamples.Camera2D;

namespace WarlordsRevenge.Classes
{
    public class WarlordsRevengeGameScreen : GameScreen
    {
        private const float MOVEMENT_SPEED = 200.0f;

        private ContentManager _content;

        private readonly Images _images;
        private Cursors _cursors;
        private readonly HexagonGrid _grid;
        private GridRenderer _gridRenderer;
        private FarSeerCamera2D _camera1;
        private FarSeerCamera2D _cameraMap;

        private readonly Troll _troll;

        private Vector2 _centerPixel;
        private Vector2 _topLeftPixel;
        private Vector2 _topRightPixel;
        private Vector2 _bottomLeftPixel;
        private Vector2 _bottomRightPixel;

        public WarlordsRevengeGameScreen()
        {
            _images = new Images();
            _troll = new Troll();
            _grid = MapReader.ReadFromFile("First.map");
        }

        public override void Initialize()
        {
            _gridRenderer = new GridRenderer();
            _camera1 = new FarSeerCamera2D(ScreenManager.GraphicsDevice);
            _cameraMap = new FarSeerCamera2D(ScreenManager.GraphicsDevice);

            const float maxNumberOfHexagonsCanScrollLeft = 6;
            const float maxNumberOfHexagonsCanScrollRight = 12;
            const float maxNumberOfHexagonsCanScrollUpDown = 4;
            const float maxNumberOfPixelsCanScrollLeft = Constants.THREE_QUARTERS_HEX_WIDTH * maxNumberOfHexagonsCanScrollLeft;
            const float maxNumberOfPixelsCanScrollRight = Constants.THREE_QUARTERS_HEX_WIDTH * maxNumberOfHexagonsCanScrollRight;
            const float maxNumberOfPixelsCanScrollUpDown = Constants.HEX_HEIGHT * maxNumberOfHexagonsCanScrollUpDown;

            var viewport = new Rectangle(0, 0, 1260, 900);

            //_cameraMap.MinPosition = new Vector2(-maxNumberOfPixelsCanScrollLeft, -maxNumberOfPixelsCanScrollUpDown);
            _cameraMap.MinPosition = new Vector2(-1260 * 0.25f, -900 * Constants.HALF);
            //_cameraMap.MaxPosition = new Vector2(maxNumberOfPixelsCanScrollRight, maxNumberOfPixelsCanScrollUpDown);
            _cameraMap.MaxPosition = new Vector2(1260 * Constants.HALF, 900 * Constants.HALF);

            _centerPixel = Vector2.Zero;
            _topLeftPixel = new Vector2(-ScreenManager.GraphicsDevice.Viewport.Width * Constants.HALF, -ScreenManager.GraphicsDevice.Viewport.Height * Constants.HALF);
            _topRightPixel = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width * Constants.HALF, -ScreenManager.GraphicsDevice.Viewport.Height * Constants.HALF);
            _bottomLeftPixel = new Vector2(-ScreenManager.GraphicsDevice.Viewport.Width * Constants.HALF, ScreenManager.GraphicsDevice.Viewport.Height * Constants.HALF);
            _bottomRightPixel = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width * Constants.HALF, ScreenManager.GraphicsDevice.Viewport.Height * Constants.HALF);

            _cursors = new Cursors(new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width - 340, ScreenManager.GraphicsDevice.Viewport.Height));
        }

        public override void LoadContent()
        {
            if (_content == null)
            {
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            _cursors.LoadContent(_content);
            _images.LoadContent(_content, new[] { "Palette1.txt", "Palette2.txt" });
            _troll.LoadContent(_content);
        }

        public override void UnloadContent()
        {
            _content.Unload();
        }

        public override void HandleInput(GameTime gameTime)
        {
            if (InputManager.IsKeyPressed(Keys.Escape))
            {
                ExitScreen();
            }

            if (InputManager.IsNewKeyPress(Keys.F1))
            {
                _gridRenderer.ShowGridLines = !_gridRenderer.ShowGridLines;
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            Vector2 mousePosition = InputManager.GetMousePosition();
            Cursor cursor = _cursors.GetCurrentCursor(mousePosition);
            InputManager.SetCursorSprite(cursor.Sprite);
            MoveCamera(cursor, gameTime);

            _troll.Update(_cameraMap);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void MoveCamera(Cursor cursor, GameTime gameTime)
        {
            Vector2 movementVector = Vector2.Zero;
            if (cursor.Name.Contains("North"))
            {
                movementVector += new Vector2(0, -1);
            }

            if (cursor.Name.Contains("West"))
            {
                movementVector += new Vector2(-1, 0);
            }

            if (cursor.Name.Contains("South"))
            {
                movementVector += new Vector2(0, 1);
            }

            if (cursor.Name.Contains("East"))
            {
                movementVector += new Vector2(1, 0);
            }

            float deltaTime = gameTime.GetElapsedSeconds();
            _cameraMap.MoveCamera(movementVector * MOVEMENT_SPEED * deltaTime);
            _cameraMap.Jump2Target();
        }

        public override void Draw(GameTime gameTime)
        {
            _gridRenderer.Draw(ScreenManager.SpriteBatch, _grid, _images, _cameraMap);
            _troll.Draw(ScreenManager.SpriteBatch, _cameraMap);

            Matrix transformMatrix = _camera1.SimView;
            ScreenManager.SpriteBatch.Begin(transformMatrix: transformMatrix);
            ScreenManager.SpriteBatch.DrawCircle(_centerPixel, 5.0f, 10, Color.Red, 2);
            ScreenManager.SpriteBatch.FillRectangle(_topLeftPixel.X + 10.0f, _topLeftPixel.Y + 10.0f, 265.0f, 105.0f, new Color(Color.Salmon, 0.5f));
            ScreenManager.SpriteBatch.FillRectangle(_topRightPixel.X - 340.0f, _topRightPixel.Y, 340.0f, 900.0f, Color.DarkSlateGray);

            Vector2 mousePosition1 = InputManager.GetMousePosition();
            Vector2 mousePosition2 = _camera1.ConvertScreenToWorld(mousePosition1);
            Vector2 mousePosition3 = mousePosition2 + _cameraMap.Position;
            Vector2 mousePosition4 = _camera1.ConvertWorldToScreen(mousePosition2);

            DrawString(string.Format("Camera Center: [{0} ; {1}]", _cameraMap.Position.X.ToString("0"), _cameraMap.Position.Y.ToString("0")), _topLeftPixel + new Vector2(15.0f, 35.0f), Color.DarkBlue);
            DrawString(string.Format("Mouse (Real): [{0} ; {1}]", mousePosition1.X.ToString("0"), mousePosition1.Y.ToString("0")), _topLeftPixel + new Vector2(15.0f, 55.0f), Color.DarkBlue);
            DrawString(string.Format("Mouse (ScreenToWorld): [{0} ; {1}]", mousePosition3.X.ToString("0"), mousePosition3.Y.ToString("0")), _topLeftPixel + new Vector2(15.0f, 75.0f), Color.DarkBlue);
            DrawString(string.Format("Mouse (WorldToScreen): [{0} ; {1}]", mousePosition4.X.ToString("0"), mousePosition4.Y.ToString("0")), _topLeftPixel + new Vector2(15.0f, 95.0f), Color.DarkBlue);

            ScreenManager.SpriteBatch.End();
        }

        private void DrawString(string text, Vector2 position, Color color)
        {
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Fonts.DetailsFont, text, position, color);
        }
    }
}