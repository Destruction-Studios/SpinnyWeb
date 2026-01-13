using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace SpinnyWeb
{
    public class SpinnyWebGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D _line;
        Texture2D _circle;
        SpriteFont _font;

        GridManager gridManager;
        PointManager pointManager;

        KeyboardState lastKeyboardState;

        private float lastMouseScroll;

        public SpinnyWebGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }


        protected override void Initialize()
        {
            

            base.Initialize();

        }

        protected override void LoadContent()
        {
            _font = Content.Load<SpriteFont>("DefaultFont");
            _line = Content.Load<Texture2D>("images/line_new");
            _circle = Content.Load<Texture2D>("images/circle");

            gridManager = new GridManager(2, _line, _circle);
            pointManager = new PointManager(_circle, _line, _font, ScreenSize() * 0.5f);

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.OemPlus) && lastKeyboardState.IsKeyUp(Keys.OemPlus))
                pointManager.AddPoint(gridManager.RandomPos(ScreenSize()));

            float mouseScroll = Mouse.GetState().ScrollWheelValue;
            pointManager.rotAdd = ((mouseScroll - lastMouseScroll) / 10) % 360;
            lastMouseScroll = mouseScroll;

            pointManager.Update(gridManager, ScreenSize());

            lastKeyboardState = keyboardState;
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(sortMode: SpriteSortMode.BackToFront);

            gridManager.Draw(spriteBatch, ScreenSize());
            pointManager.Draw(spriteBatch, ScreenSize());

            spriteBatch.DrawString(
                _font,
                "+ to add point",
                new Vector2(15, 15),
                Color.Brown);
            spriteBatch.DrawString(
                _font,
                "drag points to move them",
                new Vector2(15, 40),
                Color.Brown);
            spriteBatch.DrawString(
                _font,
                "scroll to rotate around pivot",
                new Vector2(15, 65),
                Color.Brown);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private Vector2 ScreenSize()
        {
            return new Vector2(Window.ClientBounds.Size.X, Window.ClientBounds.Size.Y);
        }
    }
}
