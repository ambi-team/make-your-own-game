public sealed class CollectablesSystem : Component
{
	#region Props/Vars
	[Property] public List<Collectables> collectables;
	[Property] public SoundEvent collectSound;
	[Property] public bool isFinished = false;
	private List<bool> finishedCollectables = new();

	private int finished = 0;
	private int max = 4;
	#endregion

	#region Logic
	public void FinishChallenge()
	{
		finished++;
		Sound.Play(collectSound);

		if (finished < max) return;

		isFinished = true;
		//todo
	}
	#endregion

	#region Components
	protected override void OnStart()
	{
		foreach (var obj in collectables)
		{
			finishedCollectables.Add(false);
			obj.OnEntered += FinishChallenge;
		}
	}
	#endregion
}