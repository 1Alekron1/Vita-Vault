using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vita_Vault.Managers;

namespace Vita_Vault.Core
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager Graphics;
        private SpriteBatch _spriteBatch;
        private GameStateManager gsm;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Graphics.PreferredBackBufferWidth = Data.ScreenWidth;
            Graphics.PreferredBackBufferHeight = Data.ScreenHeight;
            Graphics.ApplyChanges();
            gsm = new GameStateManager();
            base.Initialize();
            Constants.Content = Content;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            gsm.graphicsDevice = GraphicsDevice;
            gsm.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || Data.Exit)
                Exit();

            gsm.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            gsm.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}