using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

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
    public Vector2 LvlOffset;

    // Jumping / Gravity
    private float airSpeed;
    private float _gravity;
    private float _jumpSpeed;
    private float _fallSpeedAfterCollision;
    public bool inAir = true;
    private Map _map;


    internal override void LoadContent(ContentManager Content)
    {
        _texture = Content.Load<Texture2D>("player");
        Position = new Vector2(500, 500);
        UpdateHitBox();
        _hitBoxWidth = 55;
        _hitBoxHeight = 63;
        _xOffset = 52;
        _yOffset = 16;
        _gravity = 35;
        _jumpSpeed = -15;
        _fallSpeedAfterCollision = 225;
    }

    internal override void Update(GameTime gameTime)
    {
        UpdatePos(gameTime);
        UpdateHitBox();
    }

    private void UpdateHitBox()
    {
        _rectangleToDraw = new Rectangle((int)(Position.X - _xOffset - LvlOffset.X), (int)(Position.Y - _yOffset - LvlOffset.Y), _texture.Width,
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

        if (!inAir && !CollisionHelper.OnFloor(HitBox, _map)) inAir = true;

        if (inAir)
        {
            if (CollisionHelper.CanMoveHere(HitBox.X, HitBox.Y + airSpeed, HitBox.Width, HitBox.Height, _map))
            {
                Position.Y += airSpeed;
                airSpeed += shiftY;
            }
            else
            {
                Position.Y = CollisionHelper.GetPosNextToRoofOrFloor(HitBox, airSpeed, _map);
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
        if (CollisionHelper.CanMoveHere(Position.X + xSpeed, Position.Y, HitBox.Width, HitBox.Height, _map))
            Position.X += xSpeed;
        else
        {
            Position.X = CollisionHelper.GetPosNextToWall(HitBox, xSpeed, _map);
        }
    }


    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _rectangleToDraw, Color.White);
    }

    public void SetMap(Map map)
    {
        _map = map;
    }
}