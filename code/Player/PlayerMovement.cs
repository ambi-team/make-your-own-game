using Sandbox;
using Sandbox.Citizen;

public sealed class PlayerMovement : Component
{
	#region Props/Vars
	[Property] public bool IsPseudo { get; set; } = false;

	[Property] public float GroundControl { get; set; } = 4.0f;
	[Property] public float AirControl { get; set; } = 0.1f;
	[Property] public float MaxForce { get; set; } = 50.0f;
	[Property] public float Speed { get; set; } = 160.0f;
	[Property] public float RunSpeed { get; set; } = 290.0f;
	[Property] public float CrouchSpeed { get; set; } = 1.0f;
	[Property] public float JumpForce { get; set; } = 400.0f;

	[Property] public bool CanDuck { get; set; } = false;
	[Property] public bool CanJump { get; set; } = true;
	[Property] public bool CanSprinting { get; set; } = true;

	[Property] public GameObject Head { get; set; }
	[Property] public GameObject Body { get; set; }

	public bool IsCrouching = false;
	public bool IsSprinting = false;
	private bool isForceSpriting = false;
	private bool isForceDuck = false;

	private float defaultHeight = 0f;
	private float defaultHeightHalf = 0f;

	public Vector3 WishVelocity = Vector3.Zero;

	private CharacterController _characterController;
	private BoxCollider _collider; 
	private CitizenAnimationHelper _animationHelper;

	private bool moveForward = false;
	private bool moveBackward = false;
	private bool moveLeft = false;
	private bool moveRight = false;
	#endregion

	#region Logic
	public void MoveForward()
	{
		moveForward = true;
	}

	public void MoveBackward()
	{
		moveBackward = true;
	}

	public void MoveLeft()
	{
		moveLeft = true;
	}

	public void MoveRight()
	{
		moveRight = true;
	}

	public void BuildWishVelocity()
	{
		WishVelocity = 0;

		Rotation rot = Head.Transform.Rotation;

		if ((Input.Down("Forward") && !IsPseudo) || moveForward)
		{
			WishVelocity += rot.Forward;
			moveForward = false;
		}

		if ((Input.Down("Backward") && !IsPseudo) || moveBackward)
		{
			WishVelocity += rot.Backward;
			moveBackward = false;
		}

		if ((Input.Down("Left") && !IsPseudo) || moveLeft)
		{
			WishVelocity += rot.Left;
			moveLeft = false;
		}

		if ((Input.Down("Right") && !IsPseudo) || moveRight)
		{
			WishVelocity += rot.Right;
			moveRight = false;
		}

		WishVelocity = WishVelocity.WithZ(0);

		if (!WishVelocity.IsNearZeroLength) { WishVelocity = WishVelocity.Normal; }

		if (IsCrouching)
		{
			WishVelocity *= CrouchSpeed;
		} else if (IsSprinting || isForceSpriting)
		{
			WishVelocity *= RunSpeed;
			if (isForceSpriting) isForceSpriting = false;
		} else
		{
			WishVelocity *= Speed;
		}
	}

	public void Move()
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

		//? idk what is it
		if (_characterController.IsOnGround)
			_characterController.Velocity = _characterController.Velocity.WithZ(0);
		else
			_characterController.Velocity += gravity * Time.Delta * 0.5f;
	}

	public void RotateBody()
	{
		if (Body is null) return;

		var targerAngle = new Angles(0, Head.Transform.Rotation.Yaw(), 0);
		float rotationDifference = Body.Transform.Rotation.Distance(targerAngle);

		if (rotationDifference > 50.0f || _characterController.Velocity.Length > 10f)
			Body.Transform.Rotation = Rotation.Lerp(Body.Transform.Rotation, targerAngle, Time.Delta * 2.0f);
	}

	public void UpdateAnimation()
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

	public void Jump()
	{
		if ( !_characterController.IsOnGround) return;

		_characterController.Punch( Vector3.Up * JumpForce );
		_animationHelper?.TriggerJump();
	}

	public void Run()
	{
		isForceSpriting = true;
	}

	public void Duck(bool state)
	{
		isForceDuck = state;
	}

	public void UpdateDuck()
	{
		if ( _characterController is null ) return;

		if (!isForceDuck)
		{
			if (Input.Down("Duck") && !IsCrouching && !IsPseudo)
			{
				IsCrouching = true;
				_characterController.Height = defaultHeightHalf;
			}

			if (IsCrouching && !Input.Down("Duck") && !ChekOverPlayer() && !IsPseudo)
			{
				IsCrouching = false;
				_characterController.Height = defaultHeight;
			}

			if (IsCrouching && IsPseudo)
			{
				IsCrouching = false;
				_characterController.Height = defaultHeight;
			}
		} 
		else
		{
			IsCrouching = true;
			_characterController.Height = defaultHeightHalf;
		}

		//_collider.End = _collider.End.WithZ(_characterController.Height); //? maybe remove it
	}

	public bool ChekOverPlayer()
	{
		var camStack = Scene.Trace.Ray( Head.Transform.Position, Head.Transform.Position + new Vector3( 0, 0, 35 ) )
			.WithoutTags( "player", "trigger" )
			.Size(new Vector3(34, 34,60))
			.Run();

		return camStack.Hit;
	}
	#endregion

	#region Components
	protected override void OnAwake()
	{
		_characterController = Components.Get<CharacterController>();
		_animationHelper = Components.Get<CitizenAnimationHelper>();
		_collider = Components.Get<BoxCollider>();
	}
	
	protected override void OnFixedUpdate()
	{
		// from OnUpdate to here
		if (CanSprinting) IsSprinting = (!IsPseudo) ? Input.Down("Run") : false;
		if (CanDuck) UpdateDuck();
		if (Input.Pressed("Jump") && CanJump && !IsPseudo) Jump();

		RotateBody();
		UpdateAnimation();
		// //////////////////////////////////////////////////////////////

		BuildWishVelocity();
		Move();
	}

	protected override void OnStart()
	{
		defaultHeight = _characterController.Height;
		defaultHeightHalf = _characterController.Height / 2;
	}
	#endregion
}