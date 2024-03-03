using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

[GameResource("Achievement", "achiv", "Description.", Icon = "Drag Indicator")]
public partial class Achievement : GameResource, ISaveData
{
	[JsonIgnore] private static Dictionary<int, Achievement> achievements = new();
	[JsonIgnore, Hide] public AchievementData data;

	public string Name { get; private set; }
	public string Description { get; private set; }
	public int MaxCount { get; private set; } = 1;
	[Hide] public int ID { get; private set; }
	[Hide] public bool IsLock { get; private set; } = true;
	[Hide] public int Count { get; private set; } = 0;

	// hooks
	public static event Action<Achievement> OnUnlocked;
	public static event Action<Achievement, int> OnAddedCount;
	public static event Action<Achievement> OnPostLoaded;
	public static event Action<Achievement> OnReloaded;

	private static string filename = "achievements"; // without postfix


	protected override void PostLoad()
	{
		base.PostLoad();

		ID = ResourceId;

		if (string.IsNullOrEmpty(Name)) 
			Name = ResourceName;

		Log.Info($"[Achievement] the first loaded {this}");

		achievements.Add(ID, this);
		data = new AchievementData() { ID = ID, Count = Count, IsLock = IsLock };

		Save();

		OnPostLoaded?.Invoke(this);
	}

	protected override void PostReload()
	{
		base.PostReload();

		Log.Info($"[Achievement] reloaded {this}");

		if (data is null)
			data = new AchievementData() { ID = ID, Count = Count, IsLock = IsLock };

		Load();

		OnReloaded?.Invoke(this);
	}

	public void AddCount(int count)
	{
		Count += count;
		data.Count = Count;

		OnAddedCount?.Invoke(this, count);

		Save();

		if (Count >= MaxCount && IsLock)
			Unlock();
	}

	public void Unlock()
	{
		IsLock = false;
		data.IsLock = IsLock;

		OnUnlocked?.Invoke(this);

		Save();
	}

	public void Lock()
	{
		IsLock = true;
		data.IsLock = IsLock;

		Save();
	}

	public static IReadOnlyDictionary<int, Achievement> GetAll()
	{
		return achievements;
	}

	public override string ToString()
	{
		return $"{Name} ({ID})";
	}

	public void Save()
	{
		Dictionary<int, AchievementData> objs = new();
		foreach (var item in achievements)
		{
			objs[item.Key] = item.Value.data;
		}

		SaveData.Save(filename + ".json", objs);
		
		Log.Info($"[Achievement] saved to SaveData");
	}

	public void Load()
	{
		var achies = (Dictionary<int, AchievementData>) SaveData.Load<Dictionary<int, AchievementData>>(filename + ".json");
		var achDataFromSave = achies[ID];

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