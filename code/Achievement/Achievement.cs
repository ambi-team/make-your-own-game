using System;
using System.Text.Json.Serialization;

[GameResource("Achievement", "achiv", "Description.")]
public partial class Achievement : GameResource
{
	[JsonIgnore] private static Dictionary<int, Achievement> achievements = new();

	public string Name { get; private set; }
	public string Description { get; private set; }
	[Range(1, 16000)] public int MaxCount { get; private set; } = 1;
	[JsonIgnore, Hide] public int ID { get; private set; }
	[JsonIgnore, Hide] public bool IsLock { get; private set; } = true;
	[JsonIgnore, Hide] public int Count { get; private set; } = 0;

	// hooks
	public static event Action<Achievement> OnUnlocked;
	public static event Action<Achievement, int> OnAddedCount;
	public static event Action<Achievement> OnLoaded;
	public static event Action<Achievement> OnCreated;

	protected override void PostLoad()
	{
		base.PostLoad();

		ID = ResourceId;
		if (string.IsNullOrEmpty(Name)) Name = ResourceName;

		Log.Info($"[Achievement] the first loaded {Name} ({ID})");

		achievements.Add(ID, this);

		OnLoaded(this);
		OnCreated(this);
	}

	protected override void PostReload()
	{
		base.PostReload();

		Log.Info($"[Achievement] reloaded {Name} ({ID})");

		OnLoaded(this);
	}

	public void AddCount(int count)
	{
		Count += count;

		OnAddedCount(this, count);

		if (Count >= MaxCount && IsLock)
			Unlock();
	}

	public void Unlock()
	{
		IsLock = false;

		OnUnlocked(this);
	}

	public void Lock()
	{
		IsLock = true;
	}

	public static IReadOnlyDictionary<int, Achievement> GetAll()
	{
		return achievements;
	}

	public override string ToString()
	{
		return $"{Name} ({ID})";
	}
}