public sealed class WheelChallengeSystem : Component
{
	[Property] public List<WheelPuzzleSystem> sysList;
	[Property] public GameObject door;

	public void Complete()
	{
		int i = 0;
		foreach (var item in sysList)
		{
			if (item.isCompleted) i++;
		}

		if (i == sysList.Count) door.Destroy();
	}
}