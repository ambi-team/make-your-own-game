using System;

public sealed class Player : Component
{
	[Property] public Achievement achiv_kill;

	[Property] public Action OnKill { get; set; }

    protected override void OnStart()
	{
		int i = 0;
		foreach (var achv in Achievement.GetAll().Values)
		{
			i++;
			Log.Info($"{i} {achv}");
		}

		achiv_kill?.Unlock();
	}

	public void Kill()
	{
		OnKill();
	}
}
