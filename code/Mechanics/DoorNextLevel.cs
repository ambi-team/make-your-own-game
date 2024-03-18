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

		Sound.StopAll(0f);
		Log.Info($"[DoorNextLevel] Stopped all music");

		Log.Info($"[DoorNextLevel] Prepare to next {scene} scene");
		Scene.Load(scene);
		Log.Info($"[DoorNextLevel] Loaded next scene {scene}");
	}

	public void OnTriggerEntered(Collider other)
	{
		if (other.GameObject != Game.Ply.GameObject) return;

		NextLevel();
	}
}