using Monologs;

public sealed class StartSpawnDoors : Component
{
	[Property] public MonologSystem sys;

	[Property] public GameObject doorRed;
	[Property] public GameObject doorBlue;

	[Property] public GameObject triggerToLevel1;
	[Property] public MonologResource monologueTrigger;

	[Property] public SoundEvent spawnSound;

	protected override void OnStart()
	{
		doorRed.Enabled = false;
		doorBlue.Enabled = false;
		triggerToLevel1.Enabled = false;

		sys.OnMonologEnded += () => {
			doorRed.Enabled = true;
			doorBlue.Enabled = true;

			Sound.Play(spawnSound, doorBlue.Transform.Position);
		};

		sys.OnReplicaEnded += (monolog) => {
			if (monolog != monologueTrigger) return;

			triggerToLevel1.Enabled = true;
		};
	}
}