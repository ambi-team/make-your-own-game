public sealed class Game : Component
{
	#region Props/Vars
	[Property] public StatsData Stats { get; set; }
	[Property] public SettingsData Settings { get; set; }
	[Property] public Player Ply { get; set; }
	#endregion

	#region Logic
	public void Load()
	{
		Log.Info($"[Game] Loading...");

		Stats.Load();
		StatsSingleton.Data = Stats;

		Settings.Load();
		SettingsSingleton.Data = Settings;

		Log.Info($"[Game] Loaded!");
	}
	#endregion

	#region Components
	protected override void OnStart()
	{
		if (Stats is null || Settings is null)
		{
			Stats = new();
			Settings = new();
		}

		Load();
	}
	#endregion
}