using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace SpinnyWeb
{
    public class SpinnyWebGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D _line;
        Texture2D _circle;
        SpriteFont _font;
        SpriteFont _debugFont;

        GridManager gridManager;
        PointManager pointManager;
        VisualDebuggingManager visualDebuggingManager;

        KeyboardState lastKeyboardState;

        private float lastMouseScroll;

        public SpinnyWebGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        protected override void LoadContent()
        {
            _debugFont = Content.Load<SpriteFont>("DebugFont");
            _font = Content.Load<SpriteFont>("DefaultFont");
            _line = Content.Load<Texture2D>("images/line_new");
            _circle = Content.Load<Texture2D>("images/circle");

            gridManager = new GridManager(2, _line, _circle);
            pointManager = new PointManager(_circle, _line, _font, ScreenSize() * 0.5f);
            visualDebuggingManager = new VisualDebuggingManager(_debugFont);

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            if (WasKeyJustPressed(Keys.OemPlus, keyboardState))
                pointManager.AddPoint(new Vector2(mouseState.X, mouseState.Y));//gridManager.RandomPos(ScreenSize()));

            if (WasKeyJustPressed(Keys.G, keyboardState))
                gridManager.snapping = !gridManager.snapping;

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

            string gridText = string.Format("[G] Grid: {0}", gridManager.snapping ? "Enabled" : "Disabled");
            string pointText = string.Format("[+] Points: {0}", pointManager.points.Count - 1);

            visualDebuggingManager.AddText("[S] to open source page (spin.mrussell.net/source)");
            visualDebuggingManager.AddText(gridText);
            visualDebuggingManager.AddText(pointText);
            visualDebuggingManager.AddText("");
            visualDebuggingManager.AddText("Drag points to move them");
            visualDebuggingManager.AddText("Scroll to rotate around pivot");

            gridManager.Draw(spriteBatch, ScreenSize());
            pointManager.Draw(spriteBatch, gridManager, ScreenSize());
            visualDebuggingManager.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool WasKeyJustPressed(Keys key, KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }

        private Vector2 ScreenSize()
        {
            return new Vector2(Window.ClientBounds.Size.X, Window.ClientBounds.Size.Y);
        }
    }
}
