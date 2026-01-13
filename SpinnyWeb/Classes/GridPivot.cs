using Microsoft.Xna.Framework;

namespace SpinnyWeb
{
    public class GridPivot : GridPoint
    {
        public GridPivot(Vector2 startPosition) : base(startPosition)
        {
            color = Color.Green;
            scale = 0.03f;
        }
    }
}
