﻿using System;
using System.Text.Json.Serialization;

public class Achievement : ISaveData
{
	private static Dictionary<string, Achievement> achievements = new();

	public string ID { get; set; }
	[JsonIgnore] public string Name { get; set; }
	[JsonIgnore] public string Description { get; set; }
	public bool IsLock { get; set; } = true;
	[JsonIgnore] public int MaxCount { get; set; } = 1;
	public int Count { get; set; } = 0;

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

	public Achievement() { }

	public static Achievement CreateOrGet(string id = "", string name = "Unknow", string desc = "Description", int maxCount = 1)
	{
		Achievement achievement;
		if (achievements.TryGetValue(id, out achievement))
		{
			if (achievement.CanLoad()) achievement.Load();

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
		Log.Info($"[Achievement] Set count from {oldCount} to {Count} for {this}");

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

	public void Save()
	{
		SaveData.Save(filename + ".json", achievements);
		Log.Info($"save {achievements[ID].Count}");


		Log.Info($"[Achievement] saved all to SaveData");
	}

	private bool CanLoad()
	{
		if (!FileSystem.Data.FileExists(filename + ".json")) return false;

		var achievements = (Dictionary<string, Achievement>)SaveData.Load<Dictionary<string, Achievement>>(filename + ".json");

		Log.Info($"CanLoad: {achievements.ContainsKey(ID)}");

		return achievements.ContainsKey(ID);
	}

	public void Load()
	{
		var achievements = (Dictionary<string, Achievement>) SaveData.Load<Dictionary<string, Achievement>>(filename + ".json");
        foreach (var item in achievements)
        {
			Log.Info($"Loaded: {item.Key} {item.Value.Count}");
        }
        var achDataFromSave = achievements[ID];

		Log.Info($"[Achievement] loaded {this} from SaveData");

		IsLock = achDataFromSave.IsLock;
		SetCount(achDataFromSave.Count);
	}

	public static void LoadAll()
	{
		foreach (Achievement ach in GetAll().Values)
			ach.Load();
	}
}