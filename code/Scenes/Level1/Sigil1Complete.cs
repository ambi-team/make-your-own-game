using Monologs;

public sealed class Sigil1Complete : Component
{
	#region Props/Vars
	[Property] public GameObject door1;
	[Property] public GameObject door2;

	[Property] public bool sigil1 = false;
	[Property] public bool sigil2 = false;

	[Property] public MonologResource replica1;
	[Property] public MonologResource replica2;
	[Property] public MonologResource replica3;
	[Property] public MonologSystem sys;

	// monologoue
	#endregion

	public void SpawnDoors()
	{
		if (sigil1 && sigil2)
		{
			door1.Enabled = true;
			door2.Enabled = true;

			sys.AddToQueue(replica1);
			sys.AddToQueue(replica2);
			sys.AddToQueue(replica3);

			sys.Play();
		}
	}

	protected override void OnStart()
	{
		door1.Enabled = false;
		door2.Enabled = false;
	}
}