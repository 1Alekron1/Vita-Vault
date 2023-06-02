using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;
internal class Explosion : Component
{
    private PerlinNoise _noise;
    private SecondExplosion _particles;
    private Vector2 _position;
    public Vector2 LvlOffset { get; set; }

    internal override void LoadContent(ContentManager Content)
    {
        _noise = new PerlinNoise(_position, LvlOffset);
        _noise.LoadContent(Content);
        _particles = new SecondExplosion(_position, LvlOffset);
        _particles.LoadContent(Content);
    }

    internal override void Update(GameTime gameTime)
    {
        if (!_noise.IsDestroyed)
        {
            _noise.LvlOffset = LvlOffset;
            _noise.Update(gameTime);
        }
        _particles.Update(gameTime);
        _particles.LvlOffset = LvlOffset;
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        if (!_noise.IsDestroyed) _noise.Draw(spriteBatch);
        _particles.Draw(spriteBatch);
    }

    public void SetPosition(Vector2 currentPostion)
    {
        _position = currentPostion;
    }
}