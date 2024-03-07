using System;

public sealed class TriggerCollision : Component, Component.ICollisionListener
{
	#region Props/Vars
	[Property] public GameObject Game { get; set; }

	[Property] public Action<GameObject> OnCollisionStarted { get; set; }
	[Property] public Action<GameObject> OnCollisionStoped { get; set; }
	[Property] public Action<GameObject> OnCollisionUpdated { get; set; }
	#endregion

	#region Logic
	public void OnCollisionStart(Collision other)
	{
		OnCollisionStarted?.Invoke(other.Other.GameObject);
	}

	public void OnCollisionUpdate(Collision other)
	{
		OnCollisionUpdated?.Invoke(other.Other.GameObject);
	}

	public void OnCollisionStop(CollisionStop other)
	{
		OnCollisionStoped?.Invoke(other.Other.GameObject);
	}
	#endregion
}