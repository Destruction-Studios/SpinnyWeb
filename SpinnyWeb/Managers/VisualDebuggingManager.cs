using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SpinnyWeb
{
    public class VisualDebuggingManager
    {
        private Vector2 spacing = new Vector2(0, 16);
        private List<string> textToRender = new();
        private SpriteFont font;
        public VisualDebuggingManager(SpriteFont font) 
        {
            this.font = font;
        }

        public void AddText(string text)
        {
            textToRender.Add(text);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textToRender.Count; i++)
            {
                spriteBatch.DrawString(
                    font,
                    textToRender[i],
                    new Vector2(15, 15) +  spacing  * i,
                    Color.DarkBlue);
            }

            textToRender.Clear();
        }
    }
}
