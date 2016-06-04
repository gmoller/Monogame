using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using WarlordsRevenge.Hexagons;
using FarSeerCamera2D = WarlordsRevenge.FarseerSamples.Camera2D;

namespace WarlordsRevenge.Classes
{
    public class Troll
    {
        private Texture2D _image;
        private HexagonAxial _position;
        private bool _isSelected;

        public Troll()
        {
            _position = new HexagonAxial(0.0f, 0.0f);
        }

        public void LoadContent(ContentManager content)
        {
            _image = content.Load<Texture2D>("troll-grunt");
        }

        public void Update(FarSeerCamera2D camera)
        {
            if (InputManager.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                Vector2 mousePosition = InputManager.GetMousePosition();
                Vector2 mousePositionWorld = camera.ConvertScreenToWorld(mousePosition);
                if (_position.IncludesPixel(mousePositionWorld))
                {
                    _isSelected = true;
                }
                else
                {
                    //_isSelected = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, FarSeerCamera2D camera)
        {
            Vector2 centerPixel = _position.HexToPixel();
            Vector2 topLeftPixel = centerPixel - new Vector2(Constants.HALF_HEX_WIDTH, Constants.HALF_HEX_HEIGHT);

            var transformMatrix = camera.SimView;
            spriteBatch.Begin(transformMatrix: transformMatrix);
            spriteBatch.Draw(_image, topLeftPixel, Color.White);
            if (_isSelected)
            {
                spriteBatch.DrawCircle(centerPixel, 30.0f, 20, Color.LimeGreen, 5);
            }
            spriteBatch.End();
        }
    }
}