using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpinnyWeb
{
    public class GridManager
    {
        private const float DEPTH = 1f;

        private static Random rng = new Random();

        const float TICK_SPACING = 25;
        float tickScale;
        Texture2D line;
        Texture2D circle;

        public GridManager(float tickScale, Texture2D line, Texture2D circle)
        {
            this.tickScale = tickScale;
            this.line = line;
            this.circle = circle;
        }

        private void DrawLine(SpriteBatch sb, Vector2 pos, Vector2 size, float rot, Vector2 screenSize)
        {
            sb.Draw(
                line,
                pos,
                null,
                Color.Black,
                rot,
                new Vector2(0.5f, 0.5f),
                size,
                SpriteEffects.None,
                DEPTH
                );
        }
        private void DrawLine(SpriteBatch sb, Vector2 pos, Vector2 size, float rot, Vector2 screenSize, Color color)
        {
            sb.Draw(
                line,
                pos,
                null,
                color,
                rot,
                new Vector2(0.5f, 0.5f),
                size,
                SpriteEffects.None,
                DEPTH
                );
        }

        public Vector2 SnapToGrid(Vector2 position, Vector2 screenCenter)
        {
            return screenCenter + new Vector2(
                MathF.Round((position.X - screenCenter.X) / TICK_SPACING) * TICK_SPACING,
                MathF.Round((position.Y - screenCenter.Y) / TICK_SPACING) * TICK_SPACING
            );
        }

        public Vector2 RandomPos(Vector2 screenSize)
        {
            Vector2 center = screenSize * 0.5f;

            int maxXTicks = (int)(screenSize.X / (2 * TICK_SPACING));
            int maxYTicks = (int)(screenSize.Y / (2 * TICK_SPACING));

            int xTicks = rng.Next(-maxXTicks, maxXTicks + 1);
            int yTicks = rng.Next(-maxYTicks, maxYTicks + 1);

            Vector2 pos = center + new Vector2(xTicks * TICK_SPACING, yTicks * TICK_SPACING);
            return pos;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 screenSize)
        {
            Vector2 center = screenSize * 0.5f;

            spriteBatch.Draw(
                circle,
                center,
                null,
                Color.Black,
                0.0f,
                new Vector2(circle.Width, circle.Height) * 0.5f,
                0.02f,
                SpriteEffects.None,
                DEPTH
                );

            DrawLine(spriteBatch, center, new Vector2(tickScale, screenSize.Y), 0, screenSize);
            DrawLine(spriteBatch, center, new Vector2(screenSize.X, tickScale), 0, screenSize);

            float tickLength = 10f;

            for (float x = TICK_SPACING; x < screenSize.X; x += TICK_SPACING)
            {
                DrawLine(spriteBatch, new Vector2(center.X + x, center.Y), new Vector2(tickScale, tickLength), 0f, screenSize);
                DrawLine(spriteBatch, new Vector2(center.X - x, center.Y), new Vector2(tickScale, tickLength), 0f, screenSize);

                DrawLine(spriteBatch, new Vector2(center.X + x, center.Y), new Vector2(tickScale, screenSize.Y), 0f, screenSize, Color.Black * 0.1f);
                DrawLine(spriteBatch, new Vector2(center.X - x, center.Y), new Vector2(tickScale, screenSize.Y), 0f, screenSize, Color.Black * 0.1f);
            }

            for (float y = TICK_SPACING; y < screenSize.Y; y += TICK_SPACING)
            {

                DrawLine(spriteBatch, new Vector2(center.X, center.Y + y), new Vector2(tickLength, tickScale), 0f, screenSize);
                DrawLine(spriteBatch, new Vector2(center.X, center.Y - y), new Vector2(tickLength, tickScale), 0f, screenSize);

                DrawLine(spriteBatch, new Vector2(center.X, center.Y + y), new Vector2(screenSize.X, tickScale), 0f, screenSize, Color.Black * 0.1f);
                DrawLine(spriteBatch, new Vector2(center.X, center.Y - y), new Vector2(screenSize.X, tickScale), 0f, screenSize, Color.Black * 0.1f);
            }
        }
    }
}
