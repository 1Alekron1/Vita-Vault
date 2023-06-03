namespace Vita_Vault.Models;

internal struct ParticleEmitterData
{
    public ParticleData ParticleData = new();
    public const float Angle = 0f;
    public const float AngleVariance = 360f;
    public const float LifespanMin = 0.1f;
    public const float LifespanMax = 2f;
    public const float SpeedMin = 150f;
    public const float SpeedMax = 700f;

    public ParticleEmitterData()
    {
    }
}