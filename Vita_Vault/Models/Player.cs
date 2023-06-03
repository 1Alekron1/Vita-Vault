using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class Player : Component
{
    private const float Speed = 500f;

    public Vector2 Position;
    public Vector2 LastStablePosition;
    public Vector2 DirectionY;
    public bool Left, Right, jump;
    public Vector2 LvlOffset;

    private Texture2D _texture;
    private Texture2D _textureFlipped;
    private int _hitBoxWidth;
    private int _hitBoxHeight;
    private int _xOffset;
    private int _yOffset;
    private Rectangle HitBox { get; set; }
    private Rectangle _rectangleToDraw;
    private float _airSpeed;
    private float _gravity;
    private float _jumpSpeed;
    private bool _inAir = true;
    private Map _map;
    private Texture2D _currentSprite;

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_currentSprite, _rectangleToDraw, Color.White);
    }

    internal void SetMap(Map map)
    {
        _map = map;
    }

    internal override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("player");
        _textureFlipped = content.Load<Texture2D>("playerFliped");
        Position = new Vector2(500, 500);
        var path = Constants.SavePath;
        if (File.Exists(path) && new FileInfo(path).Length != 0) LoadCoords();
        UpdateHitBox();
        _hitBoxWidth = 55;
        _hitBoxHeight = 63;
        _xOffset = 52;
        _yOffset = 16;
        _gravity = 35;
        _jumpSpeed = -15;
        _currentSprite = _texture;
    }


    internal override void Update(GameTime gameTime)
    {
        UpdatePos(gameTime);
        UpdateHitBox();
    }

    private void UpdateHitBox()
    {
        _rectangleToDraw = new Rectangle((int)(Position.X - _xOffset - LvlOffset.X),
            (int)(Position.Y - _yOffset - LvlOffset.Y), _texture.Width,
            _texture.Height);
        HitBox = new Rectangle((int)Position.X, (int)Position.Y,
            _hitBoxWidth, _hitBoxHeight);
    }

    private void UpdatePos(GameTime gameTime)
    {
        var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var shiftX = time * Speed;
        var shiftY = time * _gravity;
        if (jump) Jump();
        if (!_inAir) LastStablePosition = new Vector2(Position.X, Position.Y);
        if (!Left && !Right && !_inAir) return;


        float xSpeed = 0;
        if (Left)
        {
            xSpeed -= shiftX;
            _currentSprite = _textureFlipped;
        }

        if (Right)
        {
            xSpeed += shiftX;
            _currentSprite = _texture;
        }

        if (!_inAir && !CollisionHelper.OnFloor(HitBox, _map)) _inAir = true;

        if (_inAir)
        {
            if (CollisionHelper.CanMoveHere(HitBox.X, HitBox.Y + _airSpeed, HitBox.Width, HitBox.Height, _map))
            {
                Position.Y += _airSpeed;
                _airSpeed += shiftY;
            }
            else
            {
                Position.Y = CollisionHelper.GetPosNextToRoofOrFloor(HitBox, _airSpeed, _map);
                if (_airSpeed >= 0) ResetInAir();
                else _airSpeed = 0;
            }
        }

        UpdateXPos(xSpeed);
    }

    private void Jump()
    {
        if (_inAir) return;
        LastStablePosition = new Vector2(Position.X, Position.Y);
        _inAir = true;
        _airSpeed = _jumpSpeed;
    }

    private void ResetInAir()
    {
        _inAir = false;
        _airSpeed = 0;
    }

    private void UpdateXPos(float xSpeed)
    {
        if (CollisionHelper.CanMoveHere(Position.X + xSpeed, Position.Y, HitBox.Width, HitBox.Height, _map))
            Position.X += xSpeed;
        else Position.X = CollisionHelper.GetPosNextToWall(HitBox, xSpeed, _map);
    }


    private void LoadCoords()
    {
        var text = File.ReadAllText(Constants.SavePath).Split();
        if (text[1] == "0") return;
        Position.X = float.Parse(text[0]);
        Position.Y = float.Parse(text[1]);
    }
}