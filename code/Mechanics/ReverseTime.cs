public sealed class ReverseTime : Component
{
	#region Props/Vars
	[Property] public Player ply;

	public bool onRecording;
	private int indexRecord = 0;

	public bool onPlaying;
	private int indexPlay = 0;

	//? can clean this actions (List instead of Dictionary)
	//! different collections for actions, cuz we at the same time down buttons
	public Dictionary<float, bool> actionsForward = new();
	public Dictionary<float, bool> actionsBackward = new();
	public Dictionary<float, bool> actionsLeft = new();
	public Dictionary<float, bool> actionsRight = new();
	public Dictionary<float, bool> actionsJump = new();
	public Dictionary<float, bool> detectRuns = new();
	public Dictionary<float, bool> detectDucks = new();
	public Dictionary<float, Rotation> eyeRotations = new();

	private float defaultTimeRecoding = 20f;
	private Vector3 startPos;
	private Rotation startEyeRotation;
	private TimeUntil TimeRecording { get; set; }
	#endregion

	//todo 3. Переделать PlayerController
	//todo 4. Воспроизвести + не забыть про предметы

	#region Record logic
	public void StartRecord()
	{
		if (onRecording) return;
		if (onPlaying) return;

		startPos = ply.GameObject.Transform.Position;
		startEyeRotation = ply.Camera.Head.Transform.Rotation;

		indexRecord = 0;
		TimeRecording = defaultTimeRecoding;
		onRecording = true; // native start

		Log.Info($"[ReverseTime] Start recording {indexRecord}");
	}

	public void Record()
	{
		if (!onRecording) return;
		if (TimeRecording)
		{
			StopRecord();

			return;
		}

		indexRecord++;

		var eyeRot = ply.Camera.Head.Transform.Rotation;
		if (!eyeRotations.TryAdd(indexRecord, eyeRot))
			eyeRotations[indexRecord] = eyeRot;

		var downForward = Input.Down("Forward");
		if (!actionsForward.TryAdd(indexRecord, downForward))
			actionsForward[indexRecord] = downForward;

		var downBackward = Input.Down("Backward");
		if (!actionsBackward.TryAdd(indexRecord, downBackward))
			actionsBackward[indexRecord] = downBackward;

		var downLeft = Input.Down("Left");
		if (!actionsLeft.TryAdd(indexRecord, downLeft))
			actionsLeft[indexRecord] = downLeft;

		var downRight = Input.Down("Right");
		if (!actionsRight.TryAdd(indexRecord, downRight))
			actionsRight[indexRecord] = downRight;

		var isRun = Input.Down("Run");
		if (!detectRuns.TryAdd(indexRecord, isRun))
			detectRuns[indexRecord] = isRun;

		var isDuck = Input.Down("Duck");
		if (!detectDucks.TryAdd(indexRecord, isDuck))
			detectDucks[indexRecord] = isDuck;

		var isJump = Input.Pressed("Jump");
		if (!actionsJump.TryAdd(indexRecord, isJump))
			actionsJump[indexRecord] = isJump;

		Log.Info($"[ReverseTime] Index {indexRecord} | TimeRecording {TimeRecording}");
	}

	public void StopRecord()
	{
		if (!onRecording) return;

		onRecording = false;

		Log.Info($"[ReverseTime] Stop recording {indexRecord}");

		StartPlay();
	}
	#endregion

	#region Play logic
	public void StartPlay()
	{
		if (onPlaying) return;

		ply.Movement.Duck(false);
		ply.GameObject.Transform.Position = startPos;
		ply.Camera.Head.Transform.Rotation = startEyeRotation;
		indexPlay = 0;
		onPlaying = true; // native start

		Log.Info($"[ReverseTime] Start play {indexRecord}");
	}

	public void Play()
	{
		if (!onPlaying) return;
		if (indexPlay >= indexRecord)
		{
			StopPlay();

			return;
		}

		indexPlay++;

		PlayerMovement mov = ply.Movement;

		//todo remove
		//string action = actions[indexPlay];
		//if (action == "use")
		//{
		//	//
		//} 
		//else if (action == "attack1")
		//{
		//	//
		//} 
		//else if (action == "attack2")
		//{
		//	//
		//} 
		//else if (action == "Forward")
		//{
		//	mov.MoveForward();
		//} 
		//else if (action == "Backward")
		//{
		//	mov.MoveBackward();
		//} 
		//else if (action == "Left")
		//{
		//	mov.MoveLeft();
		//} 
		//else if (action == "Right")
		//{
		//	mov.MoveRight();
		//} 
		//else if (action == "Jump")
		//{
		//	mov.Jump();
		//}

		Rotation eyeRot = eyeRotations[indexPlay];
		ply.Camera.Head.Transform.Rotation = eyeRot;

		//todo fix

		bool isRun = detectRuns[indexPlay];
		if (isRun) mov.Run();

		bool isDuck = detectDucks[indexPlay];
		mov.Duck(isDuck);

		bool isJump = actionsJump[indexPlay];
		if (isJump) mov.Jump();

		bool downForward = actionsForward[indexPlay];
		if (downForward) mov.MoveForward();

		bool downBackward = actionsBackward[indexPlay];
		if (downBackward) mov.MoveBackward();

		bool downLeft = actionsLeft[indexPlay];
		if (downLeft) mov.MoveLeft();

		bool downRight = actionsRight[indexPlay];
		if (downRight) mov.MoveRight();
	}

	public void StopPlay()
	{
		if (!onPlaying) return;

		ply.Movement.Duck(false);
		onPlaying = false;

		Log.Info($"[ReverseTime] Stop play {indexRecord}");
	}
	#endregion

	#region Components
	protected override void OnStart()
	{
		StartRecord(); //todo remove
	}

	protected override void OnFixedUpdate()
	{
		if (onRecording)
			Record();
		else if (onPlaying)
			Play();
	}
	#endregion
}