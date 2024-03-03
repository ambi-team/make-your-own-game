using System;
using System.Text.Json.Serialization;

public class AchievementData
{
	public int ID { get; set; }
	public bool IsLock { get; set; } = true;
	public int Count { get; set; } = 0;

	public AchievementData() { }

	public Achievement GetAchievement()
	{
		return Achievement.GetAll()[ID];
	}
}