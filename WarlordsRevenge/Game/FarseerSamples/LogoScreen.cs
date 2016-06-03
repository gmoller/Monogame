using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WarlordsRevenge.Classes;

namespace WarlordsRevenge.FarseerSamples
{
    public class LogoScreen : GameScreen
    {
        private const float LOGO_SCREEN_HEIGHT_RATIO = 4.0f / 6.0f;
        private const float LOGO_WIDTH_HEIGHT_RATIO = 1.4f;

        private ContentManager _content;
        private Rectangle _destination;
        private TimeSpan _duration;
        private Texture2D _farseerLogoTexture;

        public LogoScreen(TimeSpan duration)
        {
            _duration = duration;
            TransitionOffTime = TimeSpan.FromSeconds(1.0);
        }

        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (_content == null)
            {
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            _farseerLogoTexture = _content.Load<Texture2D>("Common/logo");

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            var rectHeight = (int)(viewport.Height * LOGO_SCREEN_HEIGHT_RATIO);
            var rectWidth = (int)(rectHeight * LOGO_WIDTH_HEIGHT_RATIO);
            int posX = viewport.Bounds.Center.X - rectWidth / 2;
            int posY = viewport.Bounds.Center.Y - rectHeight / 2;

            _destination = new Rectangle(posX, posY, rectWidth, rectHeight);
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            _content.Unload();
        }

        public override void HandleInput(GameTime gameTime)
        {
            if (InputManager.AnyKeyPressed() || InputManager.LeftMouseButtonClicked())
            {
                _duration = TimeSpan.Zero;
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _duration -= gameTime.ElapsedGameTime;
            if (_duration <= TimeSpan.Zero)
            {
                ExitScreen();
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.White);

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(_farseerLogoTexture, _destination, Color.White);
            ScreenManager.SpriteBatch.End();
        }
    }
}