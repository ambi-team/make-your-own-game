using System;

public sealed class Collectables : Component, Component.ITriggerListener
{
	[Property] public Game Game;

	public event Action OnEntered;

	public void OnTriggerEnter(Collider other)
	{
		if (other.GameObject != Game.Ply.GameObject) return;

		GameObject.Destroy();

		OnEntered?.Invoke();
	}
}