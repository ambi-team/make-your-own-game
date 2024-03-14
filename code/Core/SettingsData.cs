using System.Text.Json.Serialization;

public class SettingsData : ISaveData
{
	#region Props/Vars
	public string LanguageKey = "en";
	
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
	#endregion

	#region ISaveData
	public void Save()
	{
		SaveData.Save("settings.json", this);
	}

	public void Load()
	{
		var settings = (SettingsData) SaveData.Load<SettingsData>("settings.json");
		
		LanguageKey = settings.LanguageKey;
		
		FOV = settings.FOV;
		MouseSensitivity = settings.MouseSensitivity;
		Volume = settings.Volume;

		EnableShadows = settings.EnableShadows;
	}
	#endregion
}