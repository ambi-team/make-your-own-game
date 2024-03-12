using System;

namespace Monologs;

public sealed class MonologTrigger : Component, Component.ITriggerListener
{
	[Property] public GameObject Game { get; set; }
	[Property] public List<MonologResource> Monologs { get; set; }

	[Property] public Action<GameObject> OnTriggerEntered { get; set; }
	[Property] public Action<GameObject> OnTriggerExited { get; set; }


	public void OnTriggerEnter(Collider other)
	{
		OnTriggerEntered?.Invoke(other.GameObject);
	}

	public void OnTriggerExit(Collider other)
	{
		OnTriggerExited?.Invoke(other.GameObject);
	}
}