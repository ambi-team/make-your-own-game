using System;

public sealed class DoorNextLevel : Component, Component.ITriggerListener
{
	[Property] public bool canChangeLoyality = true;
	[Property] public string pathToScene = "";
	[Property] public bool plusLoyality = true;
	[Property] public Game Game;

	public event Action OnFinished;

	public void OnTriggerEntered(Collider other)
	{
		OnFinished?.Invoke();

		Scene.LoadFromFile(pathToScene);
	}
}