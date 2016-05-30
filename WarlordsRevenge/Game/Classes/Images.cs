using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WarlordsRevenge.Classes
{
    public class Images
    {
        private readonly List<List<Texture2D>> _images = new List<List<Texture2D>>();

        public void LoadContent(ContentManager content, string[] paths)
        {
            foreach (string path in paths)
            {
                string text = File.ReadAllText(path, Encoding.UTF8);
                text = text.Replace("\r", string.Empty);
                string[] lines = text.Split('\n');

                var images = new List<Texture2D>();
                foreach (string line in lines)
                {
                    string[] pieces = line.Split(':');
                    string imageName = pieces[1].Replace("\"", string.Empty);
                    imageName = string.Format(@"{0}\{1}", Path.GetFileNameWithoutExtension(path), Path.GetFileNameWithoutExtension(imageName));
                    var image = content.Load<Texture2D>(imageName);
                    images.Add(image);
                }
                _images.Add(images);
            }
        }

        public Texture2D GetImage(byte paletteId, byte imageId)
        {
            return _images[paletteId][imageId];
        }
    }
}