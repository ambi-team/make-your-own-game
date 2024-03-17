using Monologs;

public sealed class StartFuckingTheSecondWorld : Component
{
	[Property] public MonologResource triggerPlus;
	[Property] public MonologResource triggerNegative;
	[Property] public MonologResource mon2;
	[Property] public MonologResource mon3;
	[Property] public MonologResource mon4;
	[Property] public MonologResource mon5;
	[Property] public MonologSystem sys;
	[Property] public GameObject map;

	public void PlayMons()
	{
		if (StatsSingleton.Data.Level1EnteredTheRedDoor)
			sys.AddToQueue(triggerNegative);
		else
			sys.AddToQueue(triggerPlus);

		sys.AddToQueue(mon2);
		sys.AddToQueue(mon3);
		sys.AddToQueue(mon4);
		sys.AddToQueue(mon5);

		sys.Play();
	}

	protected override void OnStart()
	{
		foreach (var item in map.Children)
		{
			if (item.Tags.Has("skybox"))
				item.Enabled = false;
		}
	}
}