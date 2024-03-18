using System.Text.Json.Serialization;

public class SettingsData : ISaveData
{
	#region Props/Vars
	[JsonIgnore] public bool hasLoaded = false;
	[JsonIgnore] private string filename = "settings";


	public string LanguageKey { get; set; } = "en";
	
	/// <summary>
	/// От 60 до 120 
	/// </summary>
	public float FOV { get; set; } = 90;

	/// <summary>
	/// От 1 до 5
	/// </summary>
	public float MouseSensitivity { get; set; } = 2f;

	/// <summary>
	/// От 0 до 100
	/// </summary>
	public float Volume { get; set; } = 50f;

	public bool EnableShadows { get; set; } = true;
	
	[JsonIgnore] public Player ply;
	#endregion

	#region Logic
	public void SetPlayer(Player player)
	{
		ply = player;
	}

	public void Apply()
	{
		if (ply is null)
		{
			Log.Info("Error: No Player in Settings!");
			return;
		}

		ply.Camera.Camera.FieldOfView = FOV;
		ply.Camera.Sensivity = MouseSensitivity / 1.4f;

		Log.Info($"[Settings] Apply");
	}
	#endregion

	#region ISaveData
	public void Save()
	{
		SaveData.Save($"{filename}.json", this);
		
		Log.Info("Settings Saved!");
	}

	public void Load()
	{
		if (!FileSystem.Data.FileExists(filename + ".json"))
		{
			Log.Info("Settings Not Found! Creating...");

			LanguageKey = "en";
			
			FOV = Preferences.FieldOfView;
			MouseSensitivity = Preferences.Sensitivity;
			Volume = 50f;

			EnableShadows = true;
			
			Save();
			Apply();

			return;
		}

		var settings = (SettingsData) SaveData.Load<SettingsData>($"{filename}.json");
		
		LanguageKey = settings.LanguageKey;
		
		FOV = settings.FOV;
		MouseSensitivity = settings.MouseSensitivity;
		Volume = settings.Volume;

		EnableShadows = settings.EnableShadows;

		hasLoaded = true;

		Apply();
	}
	#endregion
}