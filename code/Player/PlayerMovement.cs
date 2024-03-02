using Sandbox;
using Sandbox.Citizen;
using System.Diagnostics;

public sealed class PlayerMovement : Component
{

	[Property] public float GroundControl { get; set; } = 4.0f;
	[Property] public float AirControl { get; set; } = 0.1f;
	[Property] public float MaxForce { get; set; } = 50.0f;
	[Property] public float Speed { get; set; } = 160.0f;
	[Property] public float RunSpeed { get; set; } = 290.0f;
	[Property] public float CrouchSpeed { get; set; } = 1.0f;
	[Property] public float JumpForce { get; set; } = 400.0f;

	[Property] bool CanDuck { get; set; } = true;
	[Property] bool CanJump { get; set; } = true;
	[Property] bool CanSprinting { get; set; } = true;

	[Property] public GameObject Head { get; set; }
	[Property] public GameObject Body { get; set; }

	public bool IsCrouching = false;
	public bool IsSprinting = false;

	public Vector3 WishVelocity = Vector3.Zero;

	private CharacterController _characterController;
	private CitizenAnimationHelper _animationHelper;

	protected override void OnAwake()
	{
		_characterController = Components.Get<CharacterController>();
		_animationHelper = Components.Get<CitizenAnimationHelper>();
	}

	protected override void OnUpdate()
	{
		if ( CanSprinting ) IsSprinting = Input.Down( "Run" );
		if ( CanDuck ) UpdateDuck();
		if ( Input.Pressed( "Jump" ) && CanJump ) Jump();

		RotateBody();
		UpdateAnimation();
	}

	protected override void OnFixedUpdate()
	{
		BuildWishVelocity();
		Move();
	}

	void BuildWishVelocity()
	{
		WishVelocity = 0;

		Rotation rot = Head.Transform.Rotation;

		if ( Input.Down( "Forward" ) ) WishVelocity += rot.Forward;
		if ( Input.Down( "Backward" ) ) WishVelocity += rot.Backward;
		if ( Input.Down( "Left" ) ) WishVelocity += rot.Left;
		if ( Input.Down( "Right" ) ) WishVelocity += rot.Right;

		WishVelocity = WishVelocity.WithZ(0);

		if (!WishVelocity.IsNearZeroLength) { WishVelocity = WishVelocity.Normal; }


		if ( IsCrouching ) WishVelocity *= CrouchSpeed; 
		else if ( IsSprinting ) WishVelocity *= RunSpeed; 
		else WishVelocity *= Speed;

	}

	void Move()
	{
		var gravity = Scene.PhysicsWorld.Gravity;

		if (_characterController.IsOnGround) 
		{
			_characterController.Velocity = _characterController.Velocity.WithZ(0);
			_characterController.Accelerate( WishVelocity );
			_characterController.ApplyFriction( GroundControl );
		}
		else
		{
			_characterController.Velocity += gravity * Time.Delta * 0.5f;
			_characterController.Accelerate( WishVelocity.ClampLength( MaxForce ) );
			_characterController.ApplyFriction( AirControl );
		}

		_characterController.Move();

		if ( !_characterController.IsOnGround )
		{
			_characterController.Velocity += gravity * Time.Delta * 0.5f;
		}
		else
		{
			_characterController.Velocity = _characterController.Velocity.WithZ( 0 );
		}
	}

	void RotateBody()
	{
		if ( Body is null ) return;

		var targerAngle = new Angles(0, Head.Transform.Rotation.Yaw(), 0);
		float rotationDifference = Body.Transform.Rotation.Distance( targerAngle );

		if( rotationDifference > 50.0f || _characterController.Velocity.Length > 10f )
		{
			Body.Transform.Rotation = Rotation.Lerp( Body.Transform.Rotation, targerAngle, Time.Delta * 2.0f );
		}
	}

	void UpdateAnimation()
	{
		if ( _animationHelper is null ) return;

		_animationHelper.WithWishVelocity( WishVelocity );
		_animationHelper.WithVelocity( _characterController.Velocity );
		_animationHelper.AimAngle = Head.Transform.Rotation;
		_animationHelper.IsGrounded = _characterController.IsOnGround;
		_animationHelper.WithLook( Head.Transform.Rotation.Forward, 1, 0.75f, 0.5f );
		_animationHelper.MoveStyle = CitizenAnimationHelper.MoveStyles.Run;
		_animationHelper.DuckLevel = IsCrouching ? 1f : 0f;
	}

	void Jump()
	{
		if ( !_characterController.IsOnGround) return;

		_characterController.Punch( Vector3.Up * JumpForce );
		_animationHelper?.TriggerJump();
	}

	void UpdateDuck()
	{
		if ( _characterController is null ) return;

		if ( Input.Down( "Duck") && !IsCrouching)
		{
			IsCrouching = true;
			_characterController.Height /= 2.0f;
		}

		if ( IsCrouching && !Input.Down( "Duck" ) && !ChekOverPlayer() )
		{
			IsCrouching = false;
			_characterController.Height *= 2.0f;
		}



	}

	bool ChekOverPlayer()
	{
		var camStack = Scene.Trace.Ray( Head.Transform.Position, Head.Transform.Position + new Vector3( 0, 0, 30 ) )
			.WithoutTags( "player", "trigger" )
			.Size( 34 )
			.Run();

		return camStack.Hit;
	}

}