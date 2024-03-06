public sealed class CameraMovement : Component
{
	#region Props/Vars
	[Property] public bool IsPseudo { get; set; } = false;
	[Property] public PlayerMovement Player { get; set; }
	[Property] public GameObject Body { get; set; }
	[Property] public GameObject Head { get; set; }
	[Property] public float Distance { get; set; } = 0f;

	public bool IsFirstPerson => Distance == 0f;
	public Vector3 CurrentOffset = Vector3.Zero;

	public SceneTraceResult traceResult;
	private float distanceTraceResult = 255f;

	private CameraComponent Camera;
	private ModelRenderer BodyRenderer;
	#endregion

	#region Components
	protected override void OnAwake()
	{
		Camera = Components.Get<CameraComponent>();
		BodyRenderer = Body.Components.Get<ModelRenderer>();
	}

	protected override void OnUpdate()
	{
		var eyeAngles = Head.Transform.Rotation.Angles();

		eyeAngles.pitch += Input.MouseDelta.y * 0.1f;
		eyeAngles.yaw -= Input.MouseDelta.x * 0.1f;
		eyeAngles.roll = 0f;

		eyeAngles.pitch = eyeAngles.pitch.Clamp( -89.9f, 89.9f );
		Head.Transform.Rotation = eyeAngles.ToRotation();

		var targetOffset = Vector3.Zero;
		if ( Player.IsCrouching ) targetOffset += Vector3.Down * 32.0f;
		CurrentOffset = Vector3.Lerp( CurrentOffset, targetOffset, Time.Delta * 10.0f );

		if (Camera is not null )
		{
			Vector3 camPos = Head.Transform.Position + CurrentOffset;
			var camStack = Scene.Trace.Ray( camPos, camPos + new Vector3( 0, 0, 30 ) );

			if ( !IsFirstPerson )
			{
				BodyRenderer.RenderType = ModelRenderer.ShadowRenderType.On;

				var camForward = eyeAngles.ToRotation().Forward;
				var camTrace = Scene.Trace.Ray( camPos, camPos - (camForward * Distance) )
					.WithoutTags( "player", "trigger" )
					.Size( 17 )
					.Run();

				if ( camTrace.Hit )
				{
					camPos = camTrace.HitPosition;
				}
				else
				{
					camPos = camTrace.EndPosition;
				}
			}
			else
			{
					BodyRenderer.RenderType = ModelRenderer.ShadowRenderType.ShadowsOnly;
			}

			Camera.Transform.Position = camPos;
			Camera.Transform.Rotation = eyeAngles.ToRotation();

			// by titanovsky
			traceResult = Scene.Trace.Ray(camPos, camPos + (Camera.Transform.Rotation.Forward * distanceTraceResult))
				.HitTriggers()
			.Run();
		}
	}
	#endregion
}