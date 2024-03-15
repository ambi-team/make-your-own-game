using System.Text.Json.Serialization;

public class SettingsData : ISaveData
{
	#region Props/Vars
	public string LanguageKey = "en";
	[JsonIgnore] public bool hasLoaded = false;
	[JsonIgnore] private string filename = "settings";

	public float FOV = 75f;
	public float MouseSensitivity = 1f;
	public float Volume = 1f;

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
		ply.Camera.Camera.FieldOfView = FOV;
		ply.Camera.Sensivity = MouseSensitivity;

		Log.Info($"[Settings] Setup");
	}
	#endregion

	#region ISaveData
	public void Save()
	{
		SaveData.Save($"{filename}.json", this);
	}

	public void Load()
	{
		if (!FileSystem.Data.FileExists(filename + ".json"))
		{
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
	}
	#endregion
}