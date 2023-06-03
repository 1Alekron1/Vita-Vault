using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class PerlinNoise : Component
{
    private Texture2D[] _noise;
    private double _currentFrame;
    private Texture2D _currentTexture;
    private readonly Vector2 _position;
    public Vector2 LvlOffset;
    public bool IsDestroyed { get; private set; }

    public PerlinNoise(Vector2 position, Vector2 lvlOffset)
    {
        _position = position;
        LvlOffset = lvlOffset;
    }

    internal override void LoadContent(ContentManager content)
    {
        _noise = Constants.NoiseFrames.ToArray();
        _currentTexture = _noise[0];
    }

    internal override void Update(GameTime gameTime)
    {
        var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _currentFrame += 15 * time;
        if (_currentFrame > _noise.Length)
        {
            IsDestroyed = true;
            return;
        }

        _currentTexture = _noise[(int)_currentFrame];
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_currentTexture,
            _position - new Vector2(_currentTexture.Width / 2, _currentTexture.Height / 2) - LvlOffset, Color.White);
    }
}