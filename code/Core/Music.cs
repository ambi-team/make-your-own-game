public sealed class Music : Component
{
	[Property] public SoundEvent music;

	protected override void OnStart()
	{
		music.UI = true;
		music.Volume = 0.06f;
		Sound.Play(music);
	}
}