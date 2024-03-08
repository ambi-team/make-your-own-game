using System;

public sealed class TriggerTeleport : Component, Component.ITriggerListener
{
	#region Props/Vars
	[Property] public GameObject Game { get; set; }
	[Property] public Vector3 PosToSpawn { get; set; } = Vector3.Zero;

	[Property] public Action<GameObject> OnTeleportStarted { get; set; }
	[Property] public Action<GameObject> OnTeleportStoped { get; set; }
	#endregion

	#region Logic
	public void OnTriggerEnter(Collider other)
	{
		Player ply;

		if (!other.Components.TryGet(out ply) || PosToSpawn == Vector3.Zero) return;

		OnTeleportStarted?.Invoke(ply.GameObject);

		ply.GameObject.Transform.Position = PosToSpawn;

		OnTeleportStoped?.Invoke(ply.GameObject);
	}

	public void OnTriggerExit(Collider other)
	{
	}
	#endregion
}