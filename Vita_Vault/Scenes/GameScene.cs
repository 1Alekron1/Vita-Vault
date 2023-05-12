using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;
using Vita_Vault.Managers;
using Vita_Vault.Models;

namespace Vita_Vault.Scenes;

internal class GameScene : Component
{
    private Map _map;
    private Player _player;
    private Texture2D background;
    private Matrix _translation;

    internal override void LoadContent(ContentManager Content)
    {
        _map = new Map();
        _map.LoadContent(Content);
        _player = new Player();
        _player.LoadContent(Content);
    }

    private void CalculateTranslation()
    {
        var dx = (Data.ScreenWidth / 2) - _player.Position.X;
        dx = MathHelper.Clamp(dx, -_map.MapSize.X + Data.ScreenWidth + (_map.TileSize.X / 2), _map.TileSize.X / 2);
        var dy = (Data.ScreenHeight / 2) - _player.Position.Y;
        dy = MathHelper.Clamp(dy, -_map.MapSize.Y + Data.ScreenHeight + (_map.TileSize.Y / 2), _map.TileSize.Y / 2);
        _translation = Matrix.CreateTranslation(dx, dy, 0f);
    }

    internal override void Update(GameTime gameTime)
    {
        InputManager.Update(_player);
        _player.Map = _map;
        _player.Update(gameTime);
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        _map.Draw(spriteBatch);
        _player.Draw(spriteBatch);
        spriteBatch.End();
    }
}