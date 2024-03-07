using System;

public sealed class TriggerButton : Component, IUsable
{
	[Property] public GameObject Game { get; set; }

	[Property] public Action OnTrigger { get; set; }

	public void Use(Player ply)
	{
		OnTrigger?.Invoke();
	}
}