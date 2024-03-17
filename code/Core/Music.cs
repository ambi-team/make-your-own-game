public sealed class Music : Component
{
	[Property] public SoundEvent music;

	protected override void OnStart()
	{
		Sound.Play(music);
		music.UI = true;
		music.Volume = 0.25f;
	}
}