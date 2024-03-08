using System;

public sealed class TriggerObject : Component, Component.ITriggerListener
{
	#region Props/Vars
	[Property] public GameObject Game { get; set; }

	[Property] public Action<GameObject> OnTriggerEntered { get; set; }
	[Property] public Action<GameObject> OnTriggerExited { get; set; }
	#endregion

	#region Logic
	public void OnTriggerEnter(Collider other)
	{
		OnTriggerEntered?.Invoke(other.GameObject);
	}

	public void OnTriggerExit(Collider other)
	{
		OnTriggerExited?.Invoke(other.GameObject);
	}
	#endregion
}