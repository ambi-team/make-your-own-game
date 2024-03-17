using System;

public sealed class DoorNextLevel : Component, Component.ITriggerListener
{
	[Property] public bool canChangeLoyality = true;
	[Property] public SceneFile scene;
	[Property] public bool plusLoyality = true;
	[Property] public Game Game;

	[Property] public Action OnFinished;

	public void NextLevel()
	{
		OnFinished?.Invoke();

		Scene.Load(scene);
	}

	public void OnTriggerEntered(Collider other)
	{
		if (other.GameObject != Game.Ply.GameObject) return;

		NextLevel();
	}
}