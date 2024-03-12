public sealed class BarrierWheelPuzzle : Component
{
	[Property] public WheelPuzzleSystem sys1;
	[Property] public WheelPuzzleSystem sys2;
	[Property] public SoundEvent soundOpen;

	public void Open()
	{
		if (sys1 is null || sys2 is null) return;
		if (!sys1.isCompleted || !sys2.isCompleted) return;

		GameObject.Destroy();

		if (soundOpen is not null) Sound.Play(soundOpen);
	}
}