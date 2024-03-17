using System;

public sealed class TriggerAdvanceTeleport : Component, Component.ITriggerListener
{
	#region Props/Vars
	[Property] public GameObject Game { get; set; }
	[Property] public GameObject PosToSpawn { get; set; }

	[Property] public Action<GameObject> OnTeleportStarted { get; set; }
	[Property] public Action<GameObject> OnTeleportStoped { get; set; }
	#endregion

	#region Logic
	public void OnTriggerEnter(Collider other)
	{
		Player ply;

		if (!other.Components.TryGet(out ply) || PosToSpawn is null) return;

		OnTeleportStarted?.Invoke(ply.GameObject);

		ply.GameObject.Transform.Position = PosToSpawn.Transform.Position;

		OnTeleportStoped?.Invoke(ply.GameObject);
	}

	public void OnTriggerExit(Collider other)
	{
	}
	#endregion
}