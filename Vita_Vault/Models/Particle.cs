using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class Particle : Component
{
    private readonly ParticleData _data;
    private Vector2 _position;
    private float _lifespanLeft;
    private float _lifespanAmount;
    private Color _color;
    private float _opacity;
    public bool isFinished;
    private float _scale;
    private Vector2 _origin;
    private Vector2 _direction;
    public Vector2 LvlOffset;

    public Particle(Vector2 pos, ParticleData data)
    {
        _data = data;
        _lifespanLeft = data.lifespan;
        _lifespanAmount = 1f;
        _position = pos;
        _color = data.colorStart;
        _opacity = data.opacityStart;
        _origin = new(_data.texture.Width / 2, _data.texture.Height / 2);

        if (data.speed != 0)
        {
            _data.angle = MathHelper.ToRadians(_data.angle);
            _direction = new Vector2((float)Math.Sin(_data.angle), -(float)Math.Cos(_data.angle));
        }
        else
        {
            _direction = Vector2.Zero;
        }
    }
    
    internal override void LoadContent(ContentManager Content)
    {
        throw new NotImplementedException();
    }

    internal override void Update(GameTime gameTime)
    {
        var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _lifespanLeft -= time;
        if (_lifespanLeft <= 0f)
        {
            isFinished = true;
            return;
        }

        _lifespanAmount = MathHelper.Clamp(_lifespanLeft / _data.lifespan, 0, 1);
        _color = Color.Lerp(_data.colorEnd, _data.colorStart, _lifespanAmount);
        _opacity = MathHelper.Clamp(MathHelper.Lerp(_data.opacityEnd, _data.opacityStart, _lifespanAmount), 0, 1);
        _scale = MathHelper.Lerp(_data.sizeEnd, _data.sizeStart, _lifespanAmount) / _data.texture.Width;
        _position += _direction * _data.speed * time;
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_data.texture, _position - LvlOffset, null, _color * _opacity, 0f, _origin, _scale, SpriteEffects.None, 1f);
    }
}