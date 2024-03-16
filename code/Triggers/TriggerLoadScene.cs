using System;

public sealed class TriggerLoadScene : Component, Component.ITriggerListener
{
	#region Props/Vars
	[Property] public SceneFile scene;
	#endregion

	#region Logic
	public void OnTriggerEnter(Collider other)
	{
		Player ply;

		if (!other.Components.TryGet(out ply)) return;

		Scene.Load(scene);
	}
	#endregion
}