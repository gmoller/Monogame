using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarlordsRevenge.FarseerSamples
{
    /// <summary>
    /// Track and display the FramesPerSecond.
    /// </summary>
    public class FrameRateCounter : DrawableGameComponent
    {
        //private readonly SpriteBatch _spriteBatch;
        //private readonly SpriteFont _font;

        private TimeSpan _elapsedTime = TimeSpan.Zero;
        private readonly NumberFormatInfo _format;
        private readonly Vector2 _position;
        private int _frameCounter;
        private int _frameRate;
        private ScreenManager _screenManager;

        //public FrameRateCounter(Game game, SpriteBatch spriteBatch, SpriteFont font)
        public FrameRateCounter(ScreenManager screenManager)
            : base(screenManager.Game)
        {
            _screenManager = screenManager;
            //_spriteBatch = spriteBatch;
            //_font = font;
            _format = new NumberFormatInfo { NumberDecimalSeparator = "." };
            _position = new Vector2(15, 15);
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime <= TimeSpan.FromSeconds(1)) return;

            _elapsedTime -= TimeSpan.FromSeconds(1);
            _frameRate = _frameCounter;
            _frameCounter = 0;
        }

        public override void Draw(GameTime gameTime)
        {
            _frameCounter++;

            string fps = string.Format(_format, "{0} fps", _frameRate);

            _screenManager.SpriteBatch.Begin();
            _screenManager.SpriteBatch.DrawString(_screenManager.Fonts.FrameRateCounterFont, fps, _position + Vector2.One, Color.Black);
            _screenManager.SpriteBatch.DrawString(_screenManager.Fonts.FrameRateCounterFont, fps, _position, Color.White);
            _screenManager.SpriteBatch.End();
        }
    }
}