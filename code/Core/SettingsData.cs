using System.Text.Json.Serialization;

public class SettingsData : ISaveData
{
	#region Props/Vars
	[JsonIgnore] public bool hasLoaded = false;
	[JsonIgnore] private string filename = "settings";

	
	public string _LanguageKey = "en";
    public string LanguageKey 
	{
		get { return _LanguageKey; }
		set { _LanguageKey = value; Save(); }
	}

    /// <summary>
    /// От 60 до 180 
    /// </summary>
    public float _FOV = Preferences.FieldOfView;
	public float FOV
	{
		get { return _FOV; }
		set { _FOV = value; Save(); }
	}

	/// <summary>
	/// От 1 до 20
	/// </summary>
	public float _MouseSensitivity = Preferences.Sensitivity;
	public float MouseSensitivity
	{
		get { return MouseSensitivity; }
		set { MouseSensitivity = value; Save(); }
	}

	/// <summary>
	/// От 0 до 100
	/// </summary>
	public float _Volume = 50f;
	public float Volume
	{
		get { return _Volume; }
		set { _Volume = value; Save(); }
	}

	public bool _EnableShadows = true;
	public bool EnableShadows
	{
		get { return _EnableShadows; }
		set { _EnableShadows = value; Save(); }
	}

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