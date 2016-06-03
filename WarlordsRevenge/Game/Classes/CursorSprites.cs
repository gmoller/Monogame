using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WarlordsRevenge.FarseerSamples;

namespace WarlordsRevenge.Classes
{
    public enum CursorTypes
    {
        Sword,
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }

    public class Cursor
    {
        public string Name { get; private set; }
        public Rectangle Zone { get; private set; }
        public Sprite Sprite { get; set; }

        public Cursor(string name, Rectangle zone)
        {
            Name = name;
            Zone = zone;
        }
    }

    public class Cursors
    {
        private readonly List<Cursor> _cursors = new List<Cursor>();

        public Cursors(Rectangle viewport)
        {
            const int offset = 50;

            _cursors.Add(new Cursor("Main", new Rectangle(viewport.X + offset, viewport.Y + offset, viewport.Width - (offset * 2), viewport.Height - (offset * 2))));
            _cursors.Add(new Cursor("RightPanel", new Rectangle(viewport.Width, viewport.Y, 340, viewport.Height)));
            _cursors.Add(new Cursor("North", new Rectangle(viewport.X + offset, viewport.Y, viewport.Width - (offset * 2), offset)));
            _cursors.Add(new Cursor("NorthEast", new Rectangle(viewport.Width - offset, viewport.Y, offset, offset)));
            _cursors.Add(new Cursor("East", new Rectangle(viewport.Width - offset, viewport.Y + offset, offset, viewport.Height - (offset * 2))));
            _cursors.Add(new Cursor("SouthEast", new Rectangle(viewport.Width - offset, viewport.Height - offset, offset, offset)));
            _cursors.Add(new Cursor("South", new Rectangle(viewport.X + offset, viewport.Height - offset, viewport.Width - (offset * 2), offset)));
            _cursors.Add(new Cursor("SouthWest", new Rectangle(viewport.X, viewport.Height - offset, offset, offset)));
            _cursors.Add(new Cursor("West", new Rectangle(viewport.X, viewport.Y + offset, offset, viewport.Height - (offset * 2))));
            _cursors.Add(new Cursor("NorthWest", new Rectangle(viewport.X, viewport.Y, offset, offset)));
        }

        public void LoadContent(ContentManager content)
        {
            _cursors[0].Sprite = new Sprite(content.Load<Texture2D>("Common/attack"), Vector2.Zero);
            _cursors[1].Sprite = new Sprite(content.Load<Texture2D>("Common/attack"), Vector2.Zero);
            _cursors[2].Sprite = new Sprite(content.Load<Texture2D>("Common/arrow_n"));
            _cursors[3].Sprite = new Sprite(content.Load<Texture2D>("Common/arrow_ne"));
            _cursors[4].Sprite = new Sprite(content.Load<Texture2D>("Common/arrow_e"));
            _cursors[5].Sprite = new Sprite(content.Load<Texture2D>("Common/arrow_se"));
            _cursors[6].Sprite = new Sprite(content.Load<Texture2D>("Common/arrow_s"));
            _cursors[7].Sprite = new Sprite(content.Load<Texture2D>("Common/arrow_sw"));
            _cursors[8].Sprite = new Sprite(content.Load<Texture2D>("Common/arrow_w"));
            _cursors[9].Sprite = new Sprite(content.Load<Texture2D>("Common/arrow_nw"));
        }

        public Cursor GetCurrentCursor(Vector2 mousePosition)
        {
            foreach (Cursor cursor in _cursors)
            {
                if (cursor.Zone.Contains(mousePosition))
                {
                    return cursor;
                }
            }

            return _cursors[0];
        }
    }
}