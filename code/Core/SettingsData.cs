using System.Text.Json.Serialization;

public class SettingsData : ISaveData
{
	#region Props/Vars
	[JsonIgnore] public bool hasLoaded = false;
	[JsonIgnore] private string filename = "settings";

	
	public string LanguageKey = "en";
	
	/// <summary>
	/// От 60 до 180 
	/// </summary>
	public float FOV = Preferences.FieldOfView;
	
	/// <summary>
	/// От 1 до 20
	/// </summary>
	public float MouseSensitivity = Preferences.Sensitivity;
	
	/// <summary>
	/// От 0 до 100
	/// </summary>
	public float Volume = 50f;

	public bool EnableShadows = true;
	
	[JsonIgnore] public Player ply;
	#endregion

	#region Logic
	public void SetPlayer(Player player)
	{
		ply = player;
	}

	public void Setup()
	{
		if (ply is null) return;

		ply.Camera.Camera.FieldOfView = FOV;
		ply.Camera.Sensivity = MouseSensitivity;

		Log.Info($"[Settings] Setup");
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
			
			Save();

			return;
		}

		var settings = (SettingsData) SaveData.Load<SettingsData>($"{filename}.json");
		
		LanguageKey = settings.LanguageKey;
		
		FOV = settings.FOV;
		MouseSensitivity = settings.MouseSensitivity;
		Volume = settings.Volume;

		EnableShadows = settings.EnableShadows;

		hasLoaded = true;

		Setup();
	}
	#endregion
}