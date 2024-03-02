using System;

public sealed class AchievementComponent : Component
{
	protected override void OnStart()
	{
		Dictionary<int, TestJson> objs = new() { 
			[1] = new TestJson(123, true, "Titanovsky"), 
			[2] = new TestJson(44444444, false, "Шитзу") 
		};

		SaveData.Save("slot.json", objs);

		var a = FileSystem.Data.ReadJson<Dictionary<int, TestJson>>("slot.json");

		foreach (TestJson item in a.Values)
			Log.Info(item.PropStr);
	}
}