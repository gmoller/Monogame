using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace WarlordsRevenge.Classes
{
    public class Troll
    {
        private Texture2D _image;
        private HexagonAxial _position;

        public Troll()
        {
            _position = new HexagonAxial(0.0f, 0.0f);
        }

        public void LoadContent(ContentManager content)
        {
            _image = content.Load<Texture2D>("troll-grunt");
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            float xOffset = camera.Origin.X - Constants.HALF_HEX_WIDTH;
            float yOffset = camera.Origin.Y - Constants.HALF_HEX_HEIGHT;

            var position = _position.HexToPixel();
            position.X += xOffset;
            position.Y += yOffset;

            var transformMatrix = camera.GetViewMatrix(Vector2.One);
            spriteBatch.Begin(transformMatrix: transformMatrix);
            spriteBatch.Draw(_image, position, Color.White);
            spriteBatch.End();
        }
    }
}