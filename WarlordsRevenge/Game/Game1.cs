using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.ViewportAdapters;
using WarlordsRevenge.Classes;

namespace WarlordsRevenge
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Camera2D _camera;
        private Point _virtualResolution = new Point(1920, 1080); // 1280; 768

        private Texture2D _background;
        private Images _images;
        private HexagonGrid _grid;
        private GridRenderer _gridRenderer;

        private SpriteFont _font;
        private FramesPerSecondCounterComponent _fps;

        private Troll _troll;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            _graphics.PreferredBackBufferWidth = 1680;
            _graphics.PreferredBackBufferHeight = 1050;
            _graphics.ApplyChanges();
            Window.Title = string.Format("Warlords Revenge [Resolution {0}x{1}]", _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            var viewportAdapter1 = new BoxingViewportAdapter(Window, GraphicsDevice, _virtualResolution.X, _virtualResolution.Y);
            var viewportAdapter2 = new BoxingViewportAdapter(Window, GraphicsDevice, _virtualResolution.X, _virtualResolution.Y);
            _camera = new Camera2D(viewportAdapter1);

            _grid = MapReader.ReadFromFile("First.map");
            _gridRenderer = new GridRenderer(viewportAdapter2);
            _images = new Images();

            _fps = new FramesPerSecondCounterComponent(this);

            _troll = new Troll();

            base.Initialize();
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Window.Title = string.Format("Warlords Revenge [Resolution {0}x{1}]", _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _background = Content.Load<Texture2D>(@"Background");
            _images.LoadContent(Content, new[] { "Palette1.txt", "Palette2.txt" });
            _font = Content.Load<SpriteFont>("Arial");

            _troll.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            if (InputManager.IsKeyPressed(Keys.Escape))
            {
                Exit();
            }

            // TODO: Add your update logic here
            if (InputManager.IsKeyTriggered(Keys.F1))
            {
                _gridRenderer.ShowGridLines = !_gridRenderer.ShowGridLines;
            }

            _gridRenderer.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _fps.Draw(gameTime);

            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            var transformMatrix = _camera.GetViewMatrix(Vector2.One);
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            _spriteBatch.End();

            //Viewport original = GraphicsDevice.Viewport;
            //GraphicsDevice.Viewport = new Viewport(0, 0, 1000, 1000);

            _gridRenderer.Draw(_spriteBatch, _grid, _images);

            _troll.Draw(_spriteBatch, _gridRenderer.Camera);

            //GraphicsDevice.Viewport = original;

            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _spriteBatch.DrawCircle(_camera.Origin, 5.0f, 10, Color.Red, 2);

            Vector2 mousePosition1 = InputManager.GetMousePosition();
            Vector2 mousePosition2 = _camera.ScreenToWorld(mousePosition1);
            Vector2 mousePosition3 = _camera.WorldToScreen(mousePosition2);

            _spriteBatch.FillRectangle(10.0f, 10.0f, 265.0f, 105.0f, new Color(Color.Salmon, 0.5f));
            _spriteBatch.DrawString(_font, string.Format("FPS: {0}", _fps.AverageFramesPerSecond.ToString("0")), new Vector2(15.0f, 15.0f), Color.DarkBlue);
            _spriteBatch.DrawString(_font, string.Format("Camera: [{0} ; {1}]", _gridRenderer.Camera.Position.X.ToString("0"), _gridRenderer.Camera.Position.Y.ToString("0")), new Vector2(15.0f, 35.0f), Color.DarkBlue);
            _spriteBatch.DrawString(_font, string.Format("Mouse (Real): [{0} ; {1}]", mousePosition1.X.ToString("0"), mousePosition1.Y.ToString("0")), new Vector2(15.0f, 55.0f), Color.DarkBlue);
            _spriteBatch.DrawString(_font, string.Format("Mouse (ScreenToWorld): [{0} ; {1}]", mousePosition2.X.ToString("0"), mousePosition2.Y.ToString("0")), new Vector2(15.0f, 75.0f), Color.DarkBlue);
            _spriteBatch.DrawString(_font, string.Format("Mouse (WorldToScreen): [{0} ; {1}]", mousePosition3.X.ToString("0"), mousePosition3.Y.ToString("0")), new Vector2(15.0f, 95.0f), Color.DarkBlue);
            //TODO: analyze this!
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}