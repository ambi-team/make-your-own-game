using System;

namespace Monologs;

public sealed class MonologTrigger : Component, Component.ITriggerListener
{
	[Property] public MonologSystem MonologSystem { get; set; }
	[Property] public List<MonologResource> Monologs { get; set; }
	[Property] public bool canRemove { get; set; } = false;

	[Property] public Action<GameObject> OnTriggerEntered { get; set; }
	[Property] public Action<GameObject> OnTriggerExited { get; set; }


	public void OnTriggerEnter(Collider other)
	{
		OnTriggerEntered?.Invoke(other.GameObject);

		foreach (var monolog in Monologs)
		{
			MonologSystem.AddToQueue(monolog);
		}
		
		MonologSystem.Play();

		if (canRemove) GameObject.Destroy();
	}

	public void OnTriggerExit(Collider other)
	{
		OnTriggerExited?.Invoke(other.GameObject);
	}
}