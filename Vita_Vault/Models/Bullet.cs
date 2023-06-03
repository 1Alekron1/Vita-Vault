using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class Bullet : Component
{
    public Vector2 LvlOffset;
    public Vector2 Position { get; private set; }
    public bool IsDestroyed { get; private set; }
    private Texture2D _texture;
    private readonly Vector2 _speed;
    private Map _map;
    private Vector2 _currentPosition;
    private Vector2 _loadingBoundaries;

    internal Bullet(Vector2 position, Vector2 destination, Vector2 lvlOffset)
    {
        LvlOffset = lvlOffset;
        Position = position;
        float speedScale = 1000;
        _speed = destination - position + LvlOffset;
        _speed.Normalize();
        _speed *= speedScale;
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
                _map) || !IsInLoadingBoundaries()) IsDestroyed = true;

        Position += shift;
    }

    private bool IsInLoadingBoundaries()
    {
        var diff = _currentPosition - Position;
        return !(Math.Abs(diff.X) > _loadingBoundaries.X) && !(Math.Abs(diff.Y) > _loadingBoundaries.Y);
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position - LvlOffset, Color.White);
    }

    internal void SetMap(Map map)
    {
        _map = map;
    }

    internal void SetPosition(Vector2 position)
    {
        _currentPosition = position;
    }
}