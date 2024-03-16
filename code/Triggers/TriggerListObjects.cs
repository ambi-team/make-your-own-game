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
		if (other.GameObject != Game.Ply.GameObject) return;

		OnTriggerEntered?.Invoke(Game.Ply);
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.GameObject != Game.Ply.GameObject) return;

		OnTriggerExited?.Invoke(Game.Ply);
	}
	#endregion
}