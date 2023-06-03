using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class Shooting : Component
{
    public bool IsIdle;
    public Vector2 LvlOffset;
    private List<Bullet> _activeBullets;
    private List<Explosion> _activeExplosions;
    private ContentManager _content;
    private Map _map;
    private Vector2 _currentPosition;

    public void SetMap(Map map)
    {
        _map = map;
    }

    public void SetPosition(Vector2 position)
    {
        _currentPosition = position;
    }

    internal override void LoadContent(ContentManager content)
    {
        _activeBullets = new List<Bullet>();
        _activeExplosions = new List<Explosion>();
        _content = content;
    }

    internal override void Update(GameTime gameTime)
    {
        var destroyed = new List<Bullet>();
        foreach (var bullet in _activeBullets)
        {
            if (bullet.IsDestroyed)
            {
                destroyed.Add(bullet);
                Explode(bullet);
            }
            else
            {
                bullet.LvlOffset = LvlOffset;
                bullet.SetPosition(_currentPosition);
                bullet.Update(gameTime);
            }
        }

        foreach (var explosion in _activeExplosions)
        {
            explosion.LvlOffset = LvlOffset;
            explosion.Update(gameTime);
        }

        foreach (var bullet in destroyed)
        {
            _activeBullets.Remove(bullet);
        }
    }


    internal override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var bullet in _activeBullets)
        {
            bullet.Draw(spriteBatch);
        }

        foreach (var explosion in _activeExplosions)
        {
            explosion.Draw(spriteBatch);
        }
    }

    internal void Shoot(Vector2 position, Vector2 destination)
    {
        if (!IsIdle) return;
        var bullet = new Bullet(position, destination, LvlOffset);
        bullet.LoadContent(_content);
        bullet.SetMap(_map);
        _activeBullets.Add(bullet);
        IsIdle = false;
    }

    internal void Stop()
    {
        IsIdle = true;
    }


    private void Explode(Bullet bullet)
    {
        var explosion = new Explosion();
        explosion.LvlOffset = LvlOffset;
        explosion.SetPosition(bullet.Position);
        explosion.LoadContent(_content);
        _activeExplosions.Add(explosion);
    }
}