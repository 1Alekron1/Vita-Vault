using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class Bullet : Component
{
    private Texture2D _texture;
    private Vector2 _offset;
    public Vector2 _position { get; private set; }
    private Vector2 _speed;
    private float _speedScale;
    public Vector2 LvlOffset;
    private Map _map;
    private Vector2 _currentPostion;
    private Vector2 _loadingBoundaries;
    public GraphicsDevice _graphicsDevice;

    public bool IsDestroyed { get; private set; }

    internal Bullet(Vector2 position, Vector2 destination, Vector2 lvlOffset)
    {
        LvlOffset = lvlOffset;
        _position = position;
        _position += _offset;
        _speedScale = 1000;
        _speed = destination - position + LvlOffset;
        _speed.Normalize();
        _speed *= _speedScale;
    }

    internal override void LoadContent(ContentManager Content)
    {
        _loadingBoundaries = new Vector2(1600, 1000);
        _texture = Content.Load<Texture2D>("1");
    }

    internal override void Update(GameTime gameTime)
    {
        var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var shift = time * _speed;
        if (!CollisionHelper.CanMoveHere(_position.X + shift.X, _position.Y + shift.Y, _texture.Width, _texture.Height,
                _map) || !IsInLoadingBoundaries())
        {
            IsDestroyed = true;
            //TODO Explosion
        }

        _position += shift;
    }

    private bool IsInLoadingBoundaries()
    {
        var diff = _currentPostion - _position;
        if (Math.Abs(diff.X) > _loadingBoundaries.X || Math.Abs(diff.Y) > _loadingBoundaries.Y) return false;
        return true;
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position - LvlOffset, Color.White);
    }

    public void SetMap(Map map)
    {
        _map = map;
    }

    public void SetPosition(Vector2 position)
    {
        _currentPostion = position;
    }
}