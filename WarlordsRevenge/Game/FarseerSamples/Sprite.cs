using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarlordsRevenge.FarseerSamples
{
    public struct Sprite
    {
        public Vector2 Origin;
        public Texture2D Texture;

        public Sprite(Texture2D texture, Vector2 origin)
        {
            Texture = texture;
            Origin = origin;
        }

        public Sprite(Texture2D sprite)
        {
            Texture = sprite;
            Origin = new Vector2(sprite.Width / 2.0f, sprite.Height / 2.0f);
        }
    }
}