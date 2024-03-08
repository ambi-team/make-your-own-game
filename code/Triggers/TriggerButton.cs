using System;

public sealed class TriggerButton : Component, IUsable
{
	#region Props/Vars
	[Property] public GameObject Game { get; set; }

	[Property] public Action OnTrigger { get; set; }
	#endregion

	#region Logic
	public void Use(Player ply)
	{
		OnTrigger?.Invoke();
	}
	#endregion
}