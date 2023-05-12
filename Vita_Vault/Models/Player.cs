using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;
using Vita_Vault.Managers;

namespace Vita_Vault.Models;

internal class Player : Component
{
    private const float Speed = 500f;
    private Texture2D _texture;
    public Vector2 Position;
    public Vector2 DirectionY;
    private int _hitBoxWidth;
    private int _hitBoxHeight;
    private int _xOffset;
    private int _yOffset;
    public bool left, right, jump;
    public Rectangle HitBox { get; private set; }
    private Rectangle _rectangleToDraw;

    // Jumping / Gravity
    private float airSpeed;
    private float _gravity;
    private float _jumpSpeed;
    private float _fallSpeedAfterCollision;
    public bool inAir = true;
    public Map Map { get; set; }


    internal override void LoadContent(ContentManager Content)
    {
        _texture = Content.Load<Texture2D>("player");
        Position = new Vector2(100, 300);
        _hitBoxWidth = 55;
        _hitBoxHeight = 70;
        _xOffset = 52;
        _yOffset = 9;
        _gravity = 30;
        _jumpSpeed = -12;
        _fallSpeedAfterCollision = 225;
    }

    internal override void Update(GameTime gameTime)
    {
        UpdatePos(gameTime);
        UpdateHitBox();
    }

    private void UpdateHitBox()
    {
        _rectangleToDraw = new Rectangle((int)Position.X - _xOffset, (int)Position.Y - _yOffset, _texture.Width,
            _texture.Height);
        HitBox = new Rectangle((int)Position.X, (int)Position.Y,
            _hitBoxWidth, _hitBoxHeight);
    }

    private void UpdatePos(GameTime gameTime)
    {
        var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var shiftX = time * Speed;
        var shiftY = time * _gravity;
        var shiftYUp = time * _fallSpeedAfterCollision;
        if (jump) Jump();
        if (!left && !right && !inAir) return;

        float xSpeed = 0;
        if (left)
            xSpeed -= shiftX;
        if (right)
            xSpeed += shiftX;

        if (!inAir && !CollisionHelper.OnFloor(HitBox, Map)) inAir = true;

        if (inAir)
        {
            if (CollisionHelper.CanMoveHere(HitBox.X, HitBox.Y + airSpeed, HitBox.Width, HitBox.Height, Map))
            {
                Position.Y += airSpeed;
                airSpeed += shiftY;
            }
            else
            {
                Position.Y = CollisionHelper.GetPosNextToRoofOrFloor(HitBox, airSpeed, Map);
                if (airSpeed > 0) ResetInAir();
                else airSpeed = 0;
            }
        }

        UpdateXPos(xSpeed);
    }

    private void Jump()
    {
        if (inAir) return;
        inAir = true;
        airSpeed = _jumpSpeed;
    }

    private void ResetInAir()
    {
        inAir = false;
        airSpeed = 0;
    }

    private void UpdateXPos(float xSpeed)
    {
        if (CollisionHelper.CanMoveHere(Position.X + xSpeed, Position.Y, HitBox.Width, HitBox.Height, Map))
            Position.X += xSpeed;
        else
        {
            Position.X = CollisionHelper.GetPosNextToWall(HitBox, xSpeed, Map);
        }
    }


    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _rectangleToDraw, Color.White);
    }
}