public sealed class Player : Component, Component.ICollisionListener 
{
    public CharacterController Character { get; set; }
	public PlayerMovement Movement { get; set; }
	public CameraMovement Camera { get; set; }

    protected override void OnStart()
	{
		if (Character is null)
			Character = GameObject.Components.GetInChildrenOrSelf<CharacterController>();

		if (Movement is null)
			Movement = GameObject.Components.GetInChildrenOrSelf<PlayerMovement>();

		if (Camera is null)
			Camera = GameObject.Components.GetInChildrenOrSelf<CameraMovement>();
	}

	protected override void OnFixedUpdate()
	{
		//Movement.Run();
	}

	public void OnCollisionStart(Collision other)
	{
		//
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
