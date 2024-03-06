using System;
using System.Threading;

public sealed class ReverseTime : Component
{
	[Property] public Player ply;

	public bool onRecording;
	public bool onPlaying;

	private int index = 0;
	private int indexPlay = 0;

	private Vector3 startPos;
	private Rotation startEyeRotation;
	private float defaultTimeRecoding = 20f;
	private TimeUntil TimeRecording { get; set; }

	public Dictionary<float, bool> actionsForward = new();
	public Dictionary<float, bool> actionsBackward = new();
	public Dictionary<float, bool> actionsLeft = new();
	public Dictionary<float, bool> actionsRight = new();
	public Dictionary<float, bool> actionsJump = new();
	public Dictionary<float, bool> detectRuns = new();
	public Dictionary<float, bool> detectDucks = new();
	public Dictionary<float, Rotation> eyeRotations = new();

	//todo 3. Переделать PlayerController
	//todo 4. Воспроизвести + не забыть про предметы

	#region Record logic
	public void StartRecord()
	{
		if (onRecording) return;
		if (onPlaying) return;

		startPos = ply.GameObject.Transform.Position;
		startEyeRotation = ply.Camera.Head.Transform.Rotation;

		index = 0;
		TimeRecording = defaultTimeRecoding;
		onRecording = true; // native start

		Log.Info($"[ReverseTime] Start recording {index}");
	}

	public void Record()
	{
		if (!onRecording) return;
		if (TimeRecording)
		{
			StopRecord();

			return;
		}

		index++;

		var eyeRot = ply.Camera.Head.Transform.Rotation;
		if (!eyeRotations.TryAdd(index, eyeRot))
			eyeRotations[index] = eyeRot;

		var downForward = Input.Down("Forward");
		if (!actionsForward.TryAdd(index, downForward))
			actionsForward[index] = downForward;

		var downBackward = Input.Down("Backward");
		if (!actionsBackward.TryAdd(index, downBackward))
			actionsBackward[index] = downBackward;

		var downLeft = Input.Down("Left");
		if (!actionsLeft.TryAdd(index, downLeft))
			actionsLeft[index] = downLeft;

		var downRight = Input.Down("Right");
		if (!actionsRight.TryAdd(index, downRight))
			actionsRight[index] = downRight;

		var isRun = Input.Down("Run");
		if (!detectRuns.TryAdd(index, isRun))
			detectRuns[index] = isRun;

		var isDuck = Input.Down("Duck");
		if (!detectDucks.TryAdd(index, isDuck))
			detectDucks[index] = isDuck;

		var isJump = Input.Pressed("Jump");
		if (!actionsJump.TryAdd(index, isJump))
			actionsJump[index] = isJump;

		Log.Info($"[ReverseTime] Index {index} | TimeRecording {TimeRecording}");
	}

	public void StopRecord()
	{
		if (!onRecording) return;

		onRecording = false;

		Log.Info($"[ReverseTime] Stop recording {index}");

		StartPlay();
	}
	#endregion

	#region Play logic
	public void StartPlay()
	{
		if (onPlaying) return;

		ply.GameObject.Transform.Position = startPos;
		ply.Camera.Head.Transform.Rotation = startEyeRotation;
		indexPlay = 0;
		onPlaying = true; // native start

		Log.Info($"[ReverseTime] Start play {index}");
	}

	public void Play()
	{
		if (!onPlaying) return;
		if (indexPlay >= index)
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

		Log.Info($"[ReverseTime] Stop play {index}");
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
		{
			Record();
		} 
		else if (onPlaying)
		{
			Play();
		}
	}
	#endregion
}