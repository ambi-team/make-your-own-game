public sealed class Player : Component
{
	#region Props/Vars
	[Property] public bool IsPseudo { get; set; } = false;
	[Property] public float UseDistance { get; set; } = 70f;
	[Property] public CharacterController Character { get; set; }
	[Property] public PlayerMovement Movement { get; set; }
	[Property] public CameraMovement Camera { get; set; }
	#endregion

	#region Player Logic
	public void Use()
	{
		var pos = Movement.Head.Transform.Position; // Head from Movement, cuz IsPseudo haven't Camera
		var forward = Movement.Head.Transform.Rotation.Forward; 

		var traceResult = Scene.Trace.Ray(pos, pos + (forward * UseDistance))
			.WithoutTags("player", "trigger")
			.Size(4)
			.Run();

		var hitObj = traceResult.GameObject;
		if (hitObj is not null)
		{
			var comp = hitObj.Components.Get<IUsable>();
			if (comp is null) return;

			comp.Use(this);
		}
	}
	#endregion

	#region Components
	protected override void OnStart()
	{
		if (Character is null)
			Character = GameObject.Components.GetInChildrenOrSelf<CharacterController>();

		if (Movement is null)
			Movement = GameObject.Components.GetInChildrenOrSelf<PlayerMovement>();

		if (Camera is null && !IsPseudo)
			Camera = GameObject.Components.GetInChildrenOrSelf<CameraMovement>();

		Movement.IsPseudo = IsPseudo;
	}

	protected override void OnUpdate()
	{
		if (Input.Pressed("use") && !IsPseudo)
			Use();
	}
	#endregion
}