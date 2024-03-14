using System;
using System.Text.Json.Serialization;
using Sandbox.Localization;

public class Achievement : ISaveData
{
	#region Props/Vars
	[JsonIgnore] public string ID { get; set; }
	[JsonIgnore] public LocalizedStrings Name { get; set; }
	[JsonIgnore] public LocalizedStrings Description { get; set; }
	public bool IsLock { get; set; } = true;
	[JsonIgnore] public int MaxCount { get; set; } = 1;
	public int Count { get; set; } = 0;

	private static Dictionary<string, Achievement> achievements = new();
	private static string filename = "achievements"; // without postfix
	#endregion

	#region Hooks
	public static event Action<Achievement> OnUnlocked;
	public static event Action<Achievement, int> OnSetCount;
	#endregion

	#region Logic 
	private Achievement(string id, LocalizedStrings name, LocalizedStrings description, bool isLock, int maxCount, int count)
	{
		ID = id;
		Name = name;
		Description = description;
		IsLock = isLock;
		MaxCount = maxCount;
		Count = count;
	}

	public Achievement() { }

	public static Achievement CreateOrGet(string id = "", LocalizedStrings name = null, LocalizedStrings desc = null, int maxCount = 1)
	{
		Achievement achievement;
		if (achievements.TryGetValue(id, out achievement))
		{
			if (achievement.CanLoad()) 
				achievement.Load();

			if (name != null)
				achievement.Name = name;
			
			if (desc != null)
				achievement.Description = desc;
			
			return achievement;
		}

		if (maxCount <= 0) 
		{
			Log.Error("maxCount should be more than 0");
			return null; 
		}

		achievement = new Achievement(id, name, desc, true, maxCount, 0);
		achievements.Add(id, achievement);

		Log.Info($"[Achievement] Created {achievement}");

		if (achievement.CanLoad())
			achievement.Load();
		else
			achievement.Save();

		return achievement;
	}

	public void SetCount(int count)
	{
		if (!IsLock) return;

		int oldCount = Count;
		Count = count;

		OnSetCount?.Invoke(this, count);
		Log.Info($"[Achievement] Set count from {oldCount} >> {Count} for {this}");

		Save();

		if (Count >= MaxCount)
			Unlock();
	}

	public void AddCount(int add = 1)
	{
		SetCount(Count + add);
	}

	public void Unlock()
	{
		IsLock = false;

		OnUnlocked?.Invoke(this);
		Log.Info($"[Achievement] Unlocked {this}");

		Save();
	}

	public void Lock()
	{
		IsLock = true;

		Save();
	}

	public static IReadOnlyDictionary<string, Achievement> GetAll()
	{
		return achievements;
	}

	public override string ToString()
	{
		return $"{Name} ({ID})";
	}
	#endregion

	#region ISaveData
	public void Save()
	{
		SaveData.Save(filename + ".json", achievements);

		Log.Info($"[Achievement] saved all to SaveData");
	}

	private bool CanLoad()
	{
		if (!FileSystem.Data.FileExists(filename + ".json")) return false;

		var achievementsFromSave = (Dictionary<string, Achievement>)SaveData.Load<Dictionary<string, Achievement>>(filename + ".json");

		return achievementsFromSave.ContainsKey(ID);
	}

	public void Load()
	{
		var achievementsFromSave = (Dictionary<string, Achievement>) SaveData.Load<Dictionary<string, Achievement>>(filename + ".json");
        var achDataFromSave = achievementsFromSave[ID];

		var count = achDataFromSave.Count;
		var isLock = achDataFromSave.IsLock;

		Log.Info($"[Achievement] loaded {this} [Count: {count}, IsLock: {isLock}] from SaveData");

		IsLock = isLock;
		SetCount(count);
	}

	public static void LoadAll()
	{
		foreach (Achievement ach in GetAll().Values)
			ach.Load();
	}
	#endregion
}