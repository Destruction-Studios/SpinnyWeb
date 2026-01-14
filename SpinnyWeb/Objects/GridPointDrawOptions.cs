using Microsoft.Xna.Framework;

namespace SpinnyWeb
{
    public struct GridPointDrawOptions
    {
        public bool DrawText;
        public Vector2 RenderPos;
        public float Degrees;
        public Vector2 Transform;
        public Vector2 RelativeCenterPos;
        public bool RenderExtra;

        public GridPointDrawOptions(
            bool drawText,
            bool renderExtra,
            Vector2 relCenPos,
            Vector2 renderPos,
            float degrees = 0,
            Vector2? transform = null
            )
        {
            this.RelativeCenterPos = relCenPos;
            this.RenderExtra = renderExtra;
            this.RenderPos = renderPos;
            this.DrawText = drawText;
            this.Degrees = degrees;
            this.Transform = transform ?? Vector2.Zero;
        }
    }
}
