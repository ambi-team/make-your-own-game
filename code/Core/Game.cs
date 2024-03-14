public sealed class Game : Component
{
	#region Props/Vars
	[Property] public StatsData stats;
	[Property] public SettingsData settings;
	[Property] public Player ply;
	#endregion

	#region Logic
	public void Load()
	{
		stats = new();
		stats.Load();

		StatsSingleton.Data = stats;

		settings = new();
		settings.Load();

		SettingsSingleton.Data = settings;
	}
	#endregion

	#region Components
	protected override void OnStart()
	{
		//todo add load
	}
	#endregion
}