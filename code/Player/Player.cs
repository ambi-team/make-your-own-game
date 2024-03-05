public sealed class Player : Component, Component.ICollisionListener 
{
    public CharacterController Character { get; set; }
	public PlayerMovement Movement { get; set; }
	public CameraMovement Camera { get; set; }

    protected override void OnStart()
	{
		if (Character is null)
			Character = GameObject.Components.GetInChildren<CharacterController>();

		if (Movement is null)
			Movement = GameObject.Components.GetInChildren<PlayerMovement>();

		if (Camera is null)
			Camera = GameObject.Components.GetInChildren<CameraMovement>();
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
