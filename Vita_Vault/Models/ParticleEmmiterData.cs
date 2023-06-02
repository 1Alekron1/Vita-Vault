namespace Vita_Vault.Models;

public struct ParticleEmitterData
{
    public ParticleData particleData = new();
    public float angle = 0f;
    public float angleVariance = 360f;
    public float lifespanMin = 0.1f;
    public float lifespanMax = 2f;
    public float speedMin = 150f;
    public float speedMax = 700f;

    public ParticleEmitterData()
    {
    }
}