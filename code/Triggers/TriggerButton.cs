using System;

public sealed class TriggerButton : Component, IUsable
{
	[Property] public GameObject Game { get; set; }

	[Property] public Action OnTrigger { get; set; }

	public void OnUsed(Player ply)
	{
		Log.Info("OnUsed 2");
		OnTrigger?.Invoke();
	}
}