using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpinnyWeb
{
    public class GridPoint
    {
        private Vector2 TEXT_OFFSET = new Vector2(19, 25);

        public Vector2 position { get; private set; }
        public bool hovering = false;
        protected float scale = .05f;
        protected Color color = Color.Red;

        public GridPoint(Vector2 startPosition)
        {
            position = startPosition;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public float GetScale()
        {
            return scale;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GridManager grid, Vector2 screenSize, Texture2D circle, SpriteFont font, GridPointDrawOptions options)
        {
            Vector2 circleRenderPos = grid.SnapToGrid(position, screenSize * 0.5f);


            spriteBatch.Draw(
                circle,
                circleRenderPos,
                null,
                color,
                0.0f,
                new Vector2(circle.Width, circle.Height) * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );

            if (hovering && options.DrawText)
            {
                spriteBatch.DrawString(
                    font,
                    string.Format("(x: {0:F2}, y: {1:F2})", options.RelativeCenterPos.X / 25, options.RelativeCenterPos.Y / -25),
                    options.RenderPos + TEXT_OFFSET,
                    Color.Teal
                    );
                if (options.RenderExtra)
                {
                    spriteBatch.DrawString(
                    font,
                    string.Format("degrees: {0:F2}", options.Degrees),
                    options.RenderPos + TEXT_OFFSET + new Vector2(0, 20),
                    Color.Teal
                    );
                    spriteBatch.DrawString(
                        font,
                        string.Format("transform <{0:F1}, {1:F1}>", options.Transform.X, options.Transform.Y),
                        options.RenderPos + TEXT_OFFSET + new Vector2(0, 40),
                        Color.Teal
                        );
                }
            }
        }
    }
}
