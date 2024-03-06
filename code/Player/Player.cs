public sealed class Player : Component
{
	#region Props/Vars
	[Property] public bool IsPseudo { get; set; } = false;
	[Property] public CharacterController Character { get; set; }
	[Property] public PlayerMovement Movement { get; set; }
	[Property] public CameraMovement Camera { get; set; }
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
	}
	#endregion
}