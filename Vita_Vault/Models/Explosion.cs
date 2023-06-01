using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal class Explosion : Component
{
    private PerlinNoise _noise;
    public GraphicsDevice _graphicsDevice;
    private Vector2 _position;
    public Vector2 LvlOffset { get; set; }

    internal override void LoadContent(ContentManager Content)
    {
        _noise = new PerlinNoise(_position, LvlOffset);
        _noise._graphicsDevice = _graphicsDevice;
        _noise.LoadContent(Content);
    }

    internal override void Update(GameTime gameTime)
    {
        if (!_noise.IsDestroyed)
        {
            _noise.LvlOffset = LvlOffset;
            _noise.Update(gameTime);
        }
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        if (!_noise.IsDestroyed) _noise.Draw(spriteBatch);
    }

    public void SetPosition(Vector2 currentPostion)
    {
        _position = currentPostion;
    }
}