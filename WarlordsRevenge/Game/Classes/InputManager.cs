using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WarlordsRevenge.Classes
{
    public static class InputManager
    {
        /// <summary>
        /// The state of the keyboard as of the last update.
        /// </summary>
        public static KeyboardState CurrentKeyboardState { get; private set; }

        /// <summary>
        /// The state of the keyboard as of the previous update.
        /// </summary>
        private static KeyboardState _previousKeyboardState;

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
        public static bool IsKeyTriggered(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key)) && (!_previousKeyboardState.IsKeyDown(key));
        }

        public static Vector2 GetMousePosition()
        {
            var mouseState = Mouse.GetState();

            return new Vector2(mouseState.X, mouseState.Y);
        }

        public static void Update()
        {
            // update the keyboard state
            _previousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }
    }
}