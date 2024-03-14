public sealed class Game : Component
{
	#region Props/Vars
	[Property, RequireComponent] public StatsData Stats { get; set; }
	[Property, RequireComponent] public SettingsData Settings { get; set; }
	[Property, RequireComponent] public Player Ply { get; set; }
	#endregion

	#region Logic
	public void Load()
	{
		Stats.Load();
		StatsSingleton.Data = Stats;

		Settings.Load();
		SettingsSingleton.Data = Settings;
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