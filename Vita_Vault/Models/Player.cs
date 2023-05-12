using System;
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
    public Rectangle Rectangle { get; private set; }
    private Rectangle _rectangleToDraw;
    public Vector2 Origin { get; protected set; }
    private Vector2 _gravity;
    public bool isJumping;


    internal override void LoadContent(ContentManager Content)
    {
        _texture = Content.Load<Texture2D>("hero");
        Position = new Vector2(100, 300);
        _gravity = new Vector2(0, 30);
        Origin = new(_texture.Width / 2, _texture.Height / 2);
    }

    internal override void Update(GameTime gameTime)
    {
        var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position += InputManager.Direction * time * Speed;
        if (InputManager.Jump && !isJumping)
        {
            Position += new Vector2(0, 13);
            isJumping = true;
            DirectionY = new Vector2(0, -13);
        }

        if (isJumping)
        {
            DirectionY += _gravity * time;
            DirectionY.Y = Math.Min(DirectionY.Y, 15);
            Position += DirectionY;
        }

        _rectangleToDraw = new Rectangle((int)Position.X - _rectangleToDraw.Width / 3, (int)Position.Y - _rectangleToDraw.Height / 4, _texture.Width,
            _texture.Height);
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y,
            _rectangleToDraw.Width / 3, 3 * _rectangleToDraw.Height / 4);
    }


    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _rectangleToDraw, Color.White);
    }
}