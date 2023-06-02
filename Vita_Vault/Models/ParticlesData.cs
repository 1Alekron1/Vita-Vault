using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

public struct ParticleData
{
    private static Texture2D _defaultTexture;
    public Texture2D texture = _defaultTexture ??= Constants.Content.Load<Texture2D>("particle");
    public float lifespan = 2f;
    public Color colorStart = Color.LightYellow;
    public Color colorEnd = Color.DarkOrange;
    public float opacityStart = 1f;
    public float opacityEnd = 0f;
    public float sizeStart = 40f;
    public float sizeEnd = 10f;
    public float speed = 100f;
    public float angle = 0f;

    public ParticleData()
    {
    }
}