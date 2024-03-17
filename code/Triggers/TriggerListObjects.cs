using System;

public sealed class TriggerListObjects : Component, Component.ITriggerListener
{
	#region Props/Vars
	[Property] public List<GameObject> objs;
	[Property] public Game Game { get; set; }

	[Property] public Action<Player> OnTriggerEntered { get; set; }
	[Property] public Action<Player> OnTriggerExited { get; set; }
	#endregion

	#region Logic
	public void OnTriggerEnter(Collider other)
	{
		Player ply;
		if (!other.Components.TryGet(out ply)) return;

		OnTriggerEntered?.Invoke(ply);
	}

	public void OnTriggerExit(Collider other)
	{
		Player ply;
		if (!other.Components.TryGet(out ply)) return;

		OnTriggerExited?.Invoke(ply);
	}
	#endregion
}