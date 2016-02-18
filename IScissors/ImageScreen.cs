using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IScissors.Filters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IScissors
{
    public class ImageScreen
    {
        private Texture2D originalTexture;
        private BasicImage originalImage;

        private Texture2D filteredTexture;
        private BasicImage filteredImage;

        public ImageScreen(Texture2D texture, List<IFilter> filters)
        {
            this.originalTexture = texture;
            this.originalImage = BasicImage.FromTexture(texture);

            filteredImage = BasicImage.FromTexture(texture);
            foreach (var filter in filters)
                filteredImage = filter.Apply(filteredImage);
            
            filteredTexture = filteredImage.ToTexture();
        }

        public void Update()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(filteredTexture, Vector2.Zero, Color.White);
        }
    }
}
