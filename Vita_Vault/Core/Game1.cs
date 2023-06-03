using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vita_Vault.Managers;

namespace Vita_Vault.Core
{
    internal class Game1 : Game
    {
        private static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameStateManager _gsm;

        internal Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Constants.Content = Content;
            Constants.GraphicsDevice = GraphicsDevice;
            _graphics.PreferredBackBufferWidth = Data.ScreenWidth;
            _graphics.PreferredBackBufferHeight = Data.ScreenHeight;
            _graphics.ApplyChanges();
            var path = Directory.GetCurrentDirectory();
            for (var i = 0; i < 3; i++)
                path = Path.GetDirectoryName(path);
            Constants.DirectoryPath = path;
            Constants.SavePath = Path.Combine(Constants.DirectoryPath, "Content\\Save.txt");
            ;
            _gsm = new GameStateManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gsm.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Data.Exit)
                Exit();

            _gsm.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _gsm.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}