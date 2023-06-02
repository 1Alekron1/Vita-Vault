using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class PerlinNoise : Component
{
    public Texture2D[] noise;
    private double currentFrame;
    private Texture2D currentTexture;
    private Vector2 _position;
    public Vector2 LvlOffset;
    public bool IsDestroyed { get; private set; }

    public PerlinNoise(Vector2 position, Vector2 lvlOffset)
    {
        _position = position;
        LvlOffset = lvlOffset;
    }

    internal override void LoadContent(ContentManager Content)
    {
        noise = Constants.noiseFrames.ToArray();
        currentTexture = noise[0];
    }

    internal override void Update(GameTime gameTime)
    {
        var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        currentFrame += 15 * time;
        if (currentFrame > noise.Length)
        {
            IsDestroyed = true;
            return;
        }

        currentTexture = noise[(int)currentFrame];
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(currentTexture,
            _position - new Vector2(currentTexture.Width / 2, currentTexture.Height / 2) - LvlOffset, Color.White);
    }
}