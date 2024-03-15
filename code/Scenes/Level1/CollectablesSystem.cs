using Sandbox.Localization;

public sealed class CollectablesSystem : Component
{
	[Property] public List<Collectables> collectables;
	[Property] public SoundEvent collectSound;
	[Property] public bool isFinished = false;
	private List<bool> finishedCollectables = new();

	[Property] public LocalizedStrings ab;

	private int finished = 0;
	private int max = 4;

	public void FinishChallenge()
	{
		finished++;
		Sound.Play(collectSound);

		if (finished < max) return;

		isFinished = true;
	}

	protected override void OnStart()
	{
		foreach (var obj in collectables)
		{
			finishedCollectables.Add(false);
			obj.OnEntered += FinishChallenge;
		}

		Log.Info(ab);

		SettingsSingleton.Data.LanguageKey = "ru";
		Log.Info(ab);

		SettingsSingleton.Data.LanguageKey = "fr";
		Log.Info(ab);
	}
}