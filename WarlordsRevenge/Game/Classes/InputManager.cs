using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarlordsRevenge.FarseerSamples;

namespace WarlordsRevenge.Classes
{
    /// <summary>
    /// An enum of all available mouse buttons.
    /// </summary>
    public enum MouseButtons
    {
        LeftButton,
        MiddleButton,
        RightButton,
        ExtraButton1,
        ExtraButton2
    }

    public static class InputManager
    {
        private static bool _cursorIsVisible;

        private static Sprite _cursorSprite;

        private static Viewport _viewport;

        public static bool ShowCursor
        {
            get { return _cursorIsVisible && IsCursorValid; }
            set { _cursorIsVisible = value; }
        }

        public static Vector2 CursorPosition { get; private set; }

        public static bool IsCursorMoved { get; private set; }

        public static bool IsCursorValid { get; private set; }

        /// <summary>
        /// The state of the keyboard as of the last update.
        /// </summary>
        public static KeyboardState CurrentKeyboardState { get; private set; }

        /// <summary>
        /// The state of the keyboard as of the previous update.
        /// </summary>
        private static KeyboardState _previousKeyboardState;

        /// <summary>
        /// The state of the mouse as of the last update.
        /// </summary>
        public static MouseState CurrentMouseState { get; private set; }

        /// <summary>
        /// The state of the mouse as of the previous update.
        /// </summary>
        private static MouseState _previousMouseState;

        public static void SetCursorSprite(Sprite cursorSprite)
        {
            _cursorSprite = cursorSprite;
        }

        /// <summary>
        /// Check if a key is pressed.
        /// </summary>
        public static bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Check if a key was just pressed in the most recent update.
        /// </summary>
        public static bool IsNewKeyPress(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key)) && (!_previousKeyboardState.IsKeyDown(key));
        }

        public static bool IsNewKeyRelease(Keys key)
        {
            return (CurrentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key));
        }

        public static bool AnyKeyPressed()
        {
            return CurrentKeyboardState.GetPressedKeys().Length > 0;
        }

        public static bool LeftMouseButtonClicked()
        {
            return CurrentMouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Check if a mouse button was just pressed during the most recent update.
        /// </summary>
        public static bool IsNewMouseButtonPress(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return (CurrentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released);
                case MouseButtons.RightButton:
                    return (CurrentMouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Released);
                case MouseButtons.MiddleButton:
                    return (CurrentMouseState.MiddleButton == ButtonState.Pressed && _previousMouseState.MiddleButton == ButtonState.Released);
                case MouseButtons.ExtraButton1:
                    return (CurrentMouseState.XButton1 == ButtonState.Pressed && _previousMouseState.XButton1 == ButtonState.Released);
                case MouseButtons.ExtraButton2:
                    return (CurrentMouseState.XButton2 == ButtonState.Pressed && _previousMouseState.XButton2 == ButtonState.Released);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks if the requested mouse button is released.
        /// </summary>
        /// <param name="button">The button.</param>
        public static bool IsNewMouseButtonRelease(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return (_previousMouseState.LeftButton == ButtonState.Pressed && CurrentMouseState.LeftButton == ButtonState.Released);
                case MouseButtons.RightButton:
                    return (_previousMouseState.RightButton == ButtonState.Pressed && CurrentMouseState.RightButton == ButtonState.Released);
                case MouseButtons.MiddleButton:
                    return (_previousMouseState.MiddleButton == ButtonState.Pressed && CurrentMouseState.MiddleButton == ButtonState.Released);
                case MouseButtons.ExtraButton1:
                    return (_previousMouseState.XButton1 == ButtonState.Pressed && CurrentMouseState.XButton1 == ButtonState.Released);
                case MouseButtons.ExtraButton2:
                    return (_previousMouseState.XButton2 == ButtonState.Pressed && CurrentMouseState.XButton2 == ButtonState.Released);
                default:
                    return false;
            }
        }

        public static Vector2 GetMousePosition()
        {
            return new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
        }

        /// <summary>
        /// Checks for a "menu select" input action.
        /// </summary>
        public static bool IsMenuSelect()
        {
            return IsNewKeyPress(Keys.Space) || IsNewKeyPress(Keys.Enter) || IsNewMouseButtonPress(MouseButtons.LeftButton);
        }

        public static bool IsMenuPressed()
        {
            return IsKeyPressed(Keys.Space) || IsKeyPressed(Keys.Enter) || LeftMouseButtonClicked();
        }

        public static bool IsMenuReleased()
        {
            return IsNewKeyRelease(Keys.Space) || IsNewKeyRelease(Keys.Enter) || IsNewMouseButtonRelease(MouseButtons.LeftButton);
        }

        /// <summary>
        /// Checks for a "menu cancel" input action.
        /// </summary>
        public static bool IsMenuCancel()
        {
            return IsNewKeyPress(Keys.Escape);
        }

        public static void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _cursorSprite = new Sprite(content.Load<Texture2D>("Common/attack"), Vector2.Zero);
            _viewport = graphicsDevice.Viewport;
        }

        public static void Update()
        {
            // update the keyboard state
            _previousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            _previousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            // Update cursor
            Vector2 oldCursor = CursorPosition;
            CursorPosition = new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
            CursorPosition = new Vector2(MathHelper.Clamp(CursorPosition.X, 0f, _viewport.Width), MathHelper.Clamp(CursorPosition.Y, 0f, _viewport.Height));

            if (IsCursorValid && oldCursor != CursorPosition)
                IsCursorMoved = true;
            else
                IsCursorMoved = false;

            IsCursorValid = _viewport.Bounds.Contains(CurrentMouseState.X, CurrentMouseState.Y);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (_cursorIsVisible && IsCursorValid)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(_cursorSprite.Texture, CursorPosition, null, Color.White, 0f, _cursorSprite.Origin, 1f, SpriteEffects.None, 0f);
                spriteBatch.End();
            }
        }
    }
}