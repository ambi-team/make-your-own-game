using System;

public sealed class Collectables : Component, Component.ITriggerListener
{
	[Property] public Game Game;

	public event Action OnEntered;

	private void OnTriggerEnter(Collider other)
	{
		if (other.GameObject != Game.Ply.GameObject) return;

		OnEntered?.Invoke();
	}
}