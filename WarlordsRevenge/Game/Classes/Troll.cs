﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using WarlordsRevenge.Hexagons;
using FarSeerCamera2D = WarlordsRevenge.FarseerSamples.Camera2D;

namespace WarlordsRevenge.Classes
{
    public class Troll
    {
        private InputManager _inputManager;
        private Texture2D _image;
        private HexagonAxial _position;
        private bool _isSelected;

        public Troll(InputManager inputManager)
        {
            _inputManager = inputManager;
            _position = new HexagonAxial(0.0f, 0.0f);
        }

        public void LoadContent(ContentManager content)
        {
            _image = content.Load<Texture2D>("troll-grunt");
        }

        public void Update(FarSeerCamera2D camera)
        {
            if (_inputManager.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                Vector2 mousePosition = _inputManager.GetMousePosition();
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

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 centerPixel = _position.HexToPixel();
            Vector2 topLeftPixel = centerPixel - new Vector2(Constants.HALF_HEX_WIDTH, Constants.HALF_HEX_HEIGHT);

            spriteBatch.Draw(_image, topLeftPixel, Color.White);
            if (_isSelected)
            {
                spriteBatch.DrawCircle(centerPixel, 30.0f, 20, Color.LimeGreen, 5);
            }
        }
    }
}