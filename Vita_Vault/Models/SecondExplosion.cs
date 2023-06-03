using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class SecondExplosion : Component
{
    private readonly List<Particle> _particles = new();
    private readonly ParticleEmitterData _data = new();
    private readonly Vector2 _position;
    public Vector2 LvlOffset;

    internal override void LoadContent(ContentManager Content)
    {
        for (int i = 0; i < 200; i++)
        {
            Emit(_position);
        }
    }

    internal override void Update(GameTime gameTime)
    {
        foreach (var particle in _particles)
        {
            particle.Update(gameTime);
            particle.LvlOffset = LvlOffset;
        }

        _particles.RemoveAll(p => p.IsFinished);
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var particle in _particles)
        {
            particle.Draw(spriteBatch);
        }
    }

    internal SecondExplosion(Vector2 position, Vector2 lvlOffset)
    {
        _position = position;
        LvlOffset = lvlOffset;
    }

    private static float RandomFloat(float min, float max)
    {
        var random = new Random();
        return (float)(random.NextDouble() * (max - min)) + min;
    }

    private void Emit(Vector2 pos)
    {
        ParticleData d = _data.ParticleData;
        d.Lifespan = RandomFloat(ParticleEmitterData.LifespanMin, ParticleEmitterData.LifespanMax);
        d.Speed = RandomFloat(ParticleEmitterData.SpeedMin, ParticleEmitterData.SpeedMax);
        d.Angle = RandomFloat(ParticleEmitterData.Angle - ParticleEmitterData.AngleVariance,
            ParticleEmitterData.Angle + ParticleEmitterData.AngleVariance);

        Particle p = new(pos, d);
        AddParticle(p);
    }

    private void AddParticle(Particle p)
    {
        _particles.Add(p);
    }
}