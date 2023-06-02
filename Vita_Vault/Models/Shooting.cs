using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class Shooting : Component
{
    private List<Bullet> activeBullets;
    private List<Explosion> activeExplosions;
    public bool _isIdle;
    private ContentManager _content;
    public Vector2 LvlOffset;
    private Map _map;
    private Vector2 _currentPostion;

    internal override void LoadContent(ContentManager Content)
    {
        activeBullets = new();
        activeExplosions = new();
        _content = Content;
    }

    internal override void Update(GameTime gameTime)
    {
        var destroyed = new List<Bullet>();
        foreach (var bullet in activeBullets)
        {
            if (bullet.IsDestroyed)
            {
                destroyed.Add(bullet);
                Explode(bullet);
            }
            else
            {
                bullet.LvlOffset = LvlOffset;
                bullet.SetPosition(_currentPostion);
                bullet.Update(gameTime);
            }
        }

        foreach (var explosion in activeExplosions)
        {
            explosion.LvlOffset = LvlOffset;
            explosion.Update(gameTime);
        }
        
        foreach (var bullet in destroyed)
        {
            activeBullets.Remove(bullet);
        }
        
    }

    private void Explode(Bullet bullet)
    {
        var explosion = new Explosion();
        explosion.LvlOffset = LvlOffset;
        explosion.SetPosition(bullet.Position);
        explosion.LoadContent(_content);
        activeExplosions.Add(explosion);
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var bullet in activeBullets)
        {
            bullet.Draw(spriteBatch);
        }

        foreach (var explosion in activeExplosions)
        {
            explosion.Draw(spriteBatch);
        }
    }

    internal void Shoot(Vector2 position, Vector2 destination)
    {
        if (_isIdle)
        {
            var bullet = new Bullet(position, destination, LvlOffset);
            bullet.LoadContent(_content);
            bullet.SetMap(_map);
            activeBullets.Add(bullet);
            _isIdle = false;
        }
    }

    internal void Stop()
    {
        _isIdle = true;
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