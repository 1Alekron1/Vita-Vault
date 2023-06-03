using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vita_Vault.Core;

namespace Vita_Vault.Models;

internal struct ParticleData
{
    private static Texture2D _defaultTexture;
    public readonly Texture2D Texture = _defaultTexture ??= Constants.Content.Load<Texture2D>("particle");
    public float Lifespan = 2f;
    public Color ColorStart = Color.LightYellow;
    public Color ColorEnd = Color.DarkOrange;
    public const float OpacityStart = 1f;
    public const float OpacityEnd = 0f;
    public const float SizeStart = 40f;
    public const float SizeEnd = 10f;
    public float Speed = 100f;
    public float Angle = 0f;

    public ParticleData()
    {
    }
}