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

    internal override void LoadContent(ContentManager Content)
    {
        _map = new Map();
        _map.LoadContent(Content);
        _player = new Player();
        _player.LoadContent(Content);
    }

    internal override void Update(GameTime gameTime)
    {
        InputManager.Update();
        _player.Update(gameTime);
        var isAny = false;
        foreach (var tile in _map.CollisionArray)
        {
            if (_player.Rectangle.IsOnTopOf(tile))
            {
                if (_player.isJumping && _player.DirectionY.Y >= 0)
                {
                    _player.DirectionY = Vector2.Zero;
                    _player.isJumping = false;
                    _player.Position = new Vector2(_player.Position.X, tile.Top - _player.Rectangle.Height);
                }
                isAny = true;
                break;
            }
        }

        if (!isAny && !_player.isJumping) _player.isJumping = true;
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.End();
        spriteBatch.Begin();
        _map.Draw(spriteBatch);
        _player.Draw(spriteBatch);
    }
}