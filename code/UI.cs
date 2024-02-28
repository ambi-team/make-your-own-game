using System;

public sealed class UI : Component
{
	protected override void OnStart()
	{ 
		Achievement.OnUnlocked += (achiv) => {
			Log.Info($"{achiv.Name} unlocked by UI  TODO: add panel");
		};
	}
}