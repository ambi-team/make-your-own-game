using System;
using System.Text.Json.Serialization;

public class Achievement : ISaveData
{
	[JsonIgnore] private static Dictionary<string, Achievement> achievements = new();

	public string ID { get; private set; }
	public string Name { get; private set; }
	public string Description { get; private set; }
	public bool IsLock { get; private set; } = true;
	public int MaxCount { get; private set; } = 1;
	public int Count { get; private set; } = 0;

	// hooks
	public static event Action<Achievement> OnUnlocked;
	public static event Action<Achievement, int> OnSetCount;

	private static string filename = "achievements"; // without postfix

	private Achievement(string id, string name, string description, bool isLock, int maxCount, int count)
	{
		ID = id;
		Name = name;
		Description = description;
		IsLock = isLock;
		MaxCount = maxCount;
		Count = count;
	}

	public Achievement Create(string id, string name, string desc = "", int maxCount = 1)
	{
		if (maxCount <= 0) return null;
		if (achievements.ContainsKey(id)) return null;

		var achievement = new Achievement(id, name, desc, true, maxCount, 0);
		achievements.Add(id, achievement);

		return achievement;
	}

	public void SetCount(int count)
	{
		Count = count;

		OnSetCount?.Invoke(this, count);

		Save();

		if (Count >= MaxCount && IsLock)
			Unlock();
	}

	public void Unlock()
	{
		IsLock = false;

		OnUnlocked?.Invoke(this);

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

	public void Save()
	{
		SaveData.Save(filename + ".json", achievements);
		
		Log.Info($"[Achievement] saved to SaveData");
	}

	public void Load()
	{
		var achievements = (Dictionary<string, Achievement>) SaveData.Load<Dictionary<string, Achievement>>(filename + ".json");
		var achDataFromSave = achievements[ID];

		IsLock = achDataFromSave.IsLock;
		Count = achDataFromSave.Count;

		Log.Info($"[Achievement] loaded {this} from SaveData");

		if (Count >= MaxCount && IsLock)
			Unlock();
	}

	public static void LoadAll()
	{
		foreach (Achievement ach in GetAll().Values)
			ach.Load();
	}
}