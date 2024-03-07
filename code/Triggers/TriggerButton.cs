using System;

public sealed class TriggerButton : Component
{
	[Property] public GameObject Game { get; set; }

	[Property] public Action OnTrigger { get; set; }

	protected override void OnStart()
	{
		OnTrigger?.Invoke();
	}
}