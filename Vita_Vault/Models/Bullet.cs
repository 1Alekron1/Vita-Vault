using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class Bullet : Component
{
    private Texture2D _texture;
    public Vector2 Position { get; private set; }
    private readonly Vector2 _speed;
    private readonly float _speedScale;
    public Vector2 LvlOffset;
    private Map _map;
    private Vector2 _currentPosition;
    private Vector2 _loadingBoundaries;
    public GraphicsDevice _graphicsDevice;

    public bool IsDestroyed { get; private set; }

    internal Bullet(Vector2 position, Vector2 destination, Vector2 lvlOffset)
    {
        LvlOffset = lvlOffset;
        Position = position;
        _speedScale = 1000;
        _speed = destination - position + LvlOffset;
        _speed.Normalize();
        _speed *= _speedScale;
    }

    internal override void LoadContent(ContentManager content)
    {
        _loadingBoundaries = new Vector2(1600, 1000);
        _texture = content.Load<Texture2D>("1");
    }

    internal override void Update(GameTime gameTime)
    {
        var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var shift = time * _speed;
        if (!CollisionHelper.CanMoveHere(Position.X + shift.X, Position.Y + shift.Y, _texture.Width, _texture.Height,
                _map) || !IsInLoadingBoundaries())
        {
            IsDestroyed = true;
        }

        Position += shift;
    }

    private bool IsInLoadingBoundaries()
    {
        var diff = _currentPosition - Position;
        if (Math.Abs(diff.X) > _loadingBoundaries.X || Math.Abs(diff.Y) > _loadingBoundaries.Y) return false;
        return true;
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position - LvlOffset, Color.White);
    }

    public void SetMap(Map map)
    {
        _map = map;
    }

    public void SetPosition(Vector2 position)
    {
        _currentPosition = position;
    }
}