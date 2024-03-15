public class FirefliesCell: Component
{
    [Property] public Light Light { get; set; }
    [Property] public ParticleEmitter Particles { get; set; }

    public void SwitchLight(bool enable)
    {
        Light.Enabled = enable;
    }

    public void SwitchParticles(bool enable)
    {
        Particles.Enabled = enable;
    }
}