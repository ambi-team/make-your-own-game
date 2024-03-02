using System;

public sealed class AchievementComponent : Component
{
	protected override void OnStart()
	{
		TestJson[] objs = { new TestJson(123, true, "dsads"), new TestJson(999, false, "dasdsdsaadsds") };

		SaveData.Save("slot.json", objs);
	}
}