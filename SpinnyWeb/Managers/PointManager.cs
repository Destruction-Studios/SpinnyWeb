using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SpinnyWeb
{
    public class PointManager
    {
        private const float POINT_HITBOX_SCALE = .14f;
        private const float CORNER_SIZE = 25f;


        public float rotAdd;


        public Dictionary<int, GridPoint> points = new Dictionary<int, GridPoint>();
        private int currentId = 0;
        private Texture2D circle;
        private Texture2D line;
        private SpriteFont font;

        private int draggingId = -1;
        private MouseState lastMouseState;

        public PointManager(Texture2D circle, Texture2D line, SpriteFont font, Vector2 screenCenter) 
        {
            this.circle = circle;
            this.font = font;
            this.line = line;
            GridPivot pivot = new GridPivot(screenCenter);

            points.Add(currentId, pivot);
            currentId++;
        }

        public int AddPoint(Vector2 startPosition)
        {
            int pointId = currentId;
            GridPoint point = new GridPoint(startPosition);

            points.Add(pointId, point);

            currentId++;
            return pointId;
        }

        public bool IsMouseInsidePoint(int pointId, Vector2 mousePos)
        {
            GridPoint p = points[pointId];
            float scale = p.GetScale();
            Vector2 pos = p.position;

            float halfW = circle.Width * POINT_HITBOX_SCALE * 0.5f;
            float halfH = circle.Height * POINT_HITBOX_SCALE * 0.5f;

            float left = p.position.X - halfW;
            float right = p.position.X + halfW;
            float top = p.position.Y - halfH;
            float bottom = p.position.Y + halfH;

            return (mousePos.X >= left && mousePos.X <= right &&
                mousePos.Y >= top && mousePos.Y <= bottom);
        }

        public void Update(GridManager grid, Vector2 screenSize)
        {
            Vector2 screenCenter = screenSize * 0.5f;
            MouseState state = Mouse.GetState();
            Vector2 mousePos = new Vector2(state.X, state.Y);

            bool justPressed =
                state.LeftButton == ButtonState.Pressed &&
                lastMouseState.LeftButton == ButtonState.Released;

            bool justReleased =
                state.LeftButton == ButtonState.Released &&
                lastMouseState.LeftButton == ButtonState.Pressed;

            if (rotAdd != 0)
            {
                GridPivot pivot = (GridPivot)points[0];

                foreach (KeyValuePair<int, GridPoint> v in points)
                {
                    if (v.Key == 0)
                    {
                        continue;
                    }
                    GridPoint p = v.Value;

                    Vector2 dir = p.position - pivot.position;

                    float angle = MathF.Atan2(dir.Y, dir.X);
                    float distance = dir.Length();

                    angle += MathHelper.ToRadians(rotAdd);

                    p.SetPosition(pivot.position + new Vector2(
                        MathF.Cos(angle),
                        MathF.Sin(angle)
                    ) * distance);
                }
            }

            foreach (KeyValuePair<int, GridPoint> v in points)
            {
                if (IsMouseInsidePoint(v.Key, mousePos))
                {
                    v.Value.hovering = true;
                    if (justPressed)
                    {
                        draggingId = v.Key;
                        break;
                    }
                }
                else
                {
                    v.Value.hovering = false;
                }
            }

            if (state.LeftButton == ButtonState.Pressed && draggingId != -1)
            {
                if (points.TryGetValue(draggingId, out GridPoint grabbed))
                {
                    grabbed.SetPosition(mousePos);
                } else
                {
                    draggingId = -1;
                }
            }

            if (justReleased)
            {
                draggingId = -1;
            }

            lastMouseState = state;
        }

        public void DrawDirLine(SpriteBatch spriteBatch, Vector2 from, float atan, float dist, float thickness, Color color)
        {
            spriteBatch.Draw(
                line,
                from,
                null,
                color,
                atan - (MathHelper.Pi * .5f),
                new Vector2(.5f, 0),
                new Vector2(thickness, dist),
                SpriteEffects.None,
                .9f
            );
        }

        public void Draw(SpriteBatch spriteBatch, GridManager grid, Vector2 screenSize)
        {
            Vector2 screenCenter = screenSize * 0.5f;
            GridPivot pivot = (GridPivot) points[0];

            foreach (KeyValuePair<int, GridPoint> v in points)
            {
                GridPoint p = v.Value;
                bool isPivot = v.Key == 0;
                Vector2 pointPos = grid.SnapToGrid(p.position, screenSize * 0.5f);
                Vector2 pivotPos = grid.SnapToGrid(pivot.position, screenSize * 0.5f);

                Vector2 relativePivot = new Vector2(pointPos.X - pivotPos.X, pointPos.Y - pivotPos.Y);
                Vector2 dir = pointPos - pivotPos;

                float dist = dir.Length();
                float atan = MathF.Atan2(dir.Y, dir.X);

                bool drawText = (p.hovering || draggingId == v.Key);

                if (drawText && !isPivot)
                {
                    DrawDirLine(spriteBatch, pivotPos, atan, dist, 5, Color.Teal * .65f);

                    float dx = pointPos.X - pivotPos.X;
                    float dy = pointPos.Y - pivotPos.Y;
                    Vector2 vertStart = pivotPos + new Vector2(dx, 0);

                    DrawDirLine(spriteBatch, pivotPos, dx >= 0 ? 0 : MathF.PI, MathF.Abs(dx), 3, Color.Red * .4f);
                    
                    DrawDirLine(spriteBatch, vertStart, dy >= 0 ? MathF.PI * 0.5f : -MathF.PI * 0.5f, MathF.Abs(dy), 3, Color.Red * .4f);

                    // little right-angle L inside the corner
                    Vector2 relativeCenter = new Vector2(
                        pointPos.X - screenCenter.X,
                        pointPos.Y - screenCenter.Y
                        );

                    DrawDirLine(spriteBatch, vertStart + new Vector2(CORNER_SIZE * -MathF.Sign(relativePivot.X), relativePivot.Y < 0 ? -CORNER_SIZE : 0), MathF.PI * 0.5f, CORNER_SIZE, 2, Color.Blue * .4f);
                    DrawDirLine(spriteBatch, vertStart + new Vector2(relativePivot.X < 0 ? 0 : -CORNER_SIZE, CORNER_SIZE * MathF.Sign(relativePivot.Y)), 0, CORNER_SIZE, 2, Color.Blue * .4f);
                }

                GridPointDrawOptions options = new GridPointDrawOptions(
                    drawText,
                    !isPivot,
                    pivotPos,
                    MathHelper.ToDegrees(atan + (MathHelper.Pi * .5f)),
                    relativePivot / new Vector2(25, -25)
                    );

                v.Value.Draw(spriteBatch, grid, screenSize, circle, font, options);
            }
        }
    }
}
