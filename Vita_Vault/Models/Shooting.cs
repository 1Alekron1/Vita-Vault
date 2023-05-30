using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class Shooting : Component
{
    private List<Bullet> activeBullets;
    private bool _isIdle;
    private ContentManager _content; 
    public Vector2 LvlOffset;
    private Map _map;
    private Vector2 _currentPostion;
    
    internal override void LoadContent(ContentManager Content)
    {
        activeBullets = new();
        _content = Content;
    }

    internal override void Update(GameTime gameTime)
    {
        var destroyed = new List<Bullet>();
        foreach (var bullet in activeBullets)
        {
            if (bullet.IsDestroyed) destroyed.Add(bullet);
            else
            {
                bullet.LvlOffset = LvlOffset;
                bullet.SetPosition(_currentPostion);
                bullet.Update(gameTime);
            }
        }

        foreach (var bullet in destroyed)
        {
            activeBullets.Remove(bullet);
        }
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var bullet in activeBullets)
        {
            bullet.Draw(spriteBatch);
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