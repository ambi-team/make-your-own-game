using System;
using System.Numerics;

public sealed class Player : Component, Component.ICollisionListener 
{
	[Property] public Achievement achiv_kill;
	[Property] public HighlightOutline ho;
	[Property] public GameObject ply;

	[Property] public Action OnKill { get; set; }

	private CharacterController character;
	private CameraMovement camera;

    protected override void OnStart()
	{
		ho.Width = 0f;

		if (ply is not null) 
		{
			character = ply.Components.GetInChildren<CharacterController>();
			camera = ply.Components.GetInChildren<CameraMovement>();
		}
	}

	public void Kill()
	{
		OnKill();
	}

	protected override void OnUpdate()
	{
		SceneTraceResult trResult = camera.traceResult;
		if (!trResult.Hit) return;

		GameObject obj = trResult.GameObject;

		Log.Info($"{obj} {GameObject}");
		if (ho.Width < 0.6f && obj == GameObject)
		{
			ho.Width = 0.6f;
		} else
		{
			ho.Width = 0f;
		}
	}

	public void OnCollisionStart(Collision other)
	{
		var obj = other.Other.GameObject;
		if (obj != ply) return;

		character.Velocity = -obj.Transform.Local.Forward * 1000f;
		character.Move();

		Log.Info("PUSH !");
	}

	public void OnCollisionUpdate(Collision other)
	{
		//
	}

	public void OnCollisionStop(CollisionStop other)
	{
		//
	}
}
