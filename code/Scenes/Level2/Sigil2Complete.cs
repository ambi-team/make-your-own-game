using Monologs;

public sealed class Sigil2Complete : Component
{
	#region Props/Vars
	[Property] public GameObject door1;
	[Property] public GameObject door2;
	
	[Property] public bool sigil1 = false;
	[Property] public bool sigil2 = false;

	[Property] public MonologResource replica1;
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

			sys.Play();
		}
	}
}