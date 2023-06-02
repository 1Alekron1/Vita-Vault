using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class SecondExplosion : Component
{
    private List<Particle> _particles = new();
    private ParticleEmitterData _data = new();
    private Vector2 _position;
    public Vector2 LvlOffset;
    public void AddParticle(Particle p)
    {
        _particles.Add(p);
    }

    public SecondExplosion(Vector2 position, Vector2 lvlOffset)
    {
        _position = position;
        LvlOffset = lvlOffset;
    }
    public static float RandomFloat(float min, float max)
    {
        var random = new Random();
        return (float)(random.NextDouble() * (max - min)) + min;
    }
    
    private void Emit(Vector2 pos)
    {
        ParticleData d = _data.particleData;
        d.lifespan =  RandomFloat(_data.lifespanMin, _data.lifespanMax);
        d.speed = RandomFloat(_data.speedMin, _data.speedMax);
        d.angle = RandomFloat(_data.angle - _data.angleVariance, _data.angle + _data.angleVariance);

        Particle p = new(pos, d);
        AddParticle(p);
    } 
    
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

        _particles.RemoveAll(p => p.isFinished);
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var particle in _particles)
        {
            particle.Draw(spriteBatch);
        }
    }
    
}
