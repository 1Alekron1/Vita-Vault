using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class Particle : Component
{
    public bool IsFinished;
    public Vector2 LvlOffset;
    private readonly ParticleData _data;
    private Vector2 _position;
    private float _lifespanLeft;
    private float _lifespanAmount;
    private Color _color;
    private float _opacity;
    private float _scale;
    private readonly Vector2 _origin;
    private readonly Vector2 _direction;

    internal Particle(Vector2 pos, ParticleData data)
    {
        _data = data;
        _lifespanLeft = data.Lifespan;
        _lifespanAmount = 1f;
        _position = pos;
        _color = data.ColorStart;
        _opacity = ParticleData.OpacityStart;
        _origin = new Vector2(_data.Texture.Width / 2, _data.Texture.Height / 2);

        if (data.Speed != 0)
        {
            _data.Angle = MathHelper.ToRadians(_data.Angle);
            _direction = new Vector2((float)Math.Sin(_data.Angle), -(float)Math.Cos(_data.Angle));
        }
        else _direction = Vector2.Zero;
    }

    internal override void LoadContent(ContentManager content)
    {
    }

    internal override void Update(GameTime gameTime)
    {
        var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _lifespanLeft -= time;
        if (_lifespanLeft <= 0f)
        {
            IsFinished = true;
            return;
        }

        _lifespanAmount = MathHelper.Clamp(_lifespanLeft / _data.Lifespan, 0, 1);
        _color = Color.Lerp(_data.ColorEnd, _data.ColorStart, _lifespanAmount);
        _opacity = MathHelper.Clamp(MathHelper.Lerp(ParticleData.OpacityEnd, ParticleData.OpacityStart, _lifespanAmount), 0, 1);
        _scale = MathHelper.Lerp(ParticleData.SizeEnd, ParticleData.SizeStart, _lifespanAmount) / _data.Texture.Width;
        _position += _direction * _data.Speed * time;
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_data.Texture, _position - LvlOffset, null, _color * _opacity, 0f, _origin, _scale,
            SpriteEffects.None, 1f);
    }
}