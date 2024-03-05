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
	private float defaultTimeRecoding = 6f;
	private TimeUntil TimeRecording { get; set; }

	public Dictionary<float, string> actions = new();
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
		if (actions.Count > 0)
		{
			for (int i = 0; i < actions.Count; i++)
				actions[i] = "";
		}
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

		var action = GetCurrentAction();
		if (!actions.TryAdd(index, action)) 
			actions[index] = action;

		var isRun = Input.Down("Run");
		if (!detectRuns.TryAdd(index, isRun))
			detectRuns[index] = isRun;

		var isDuck = Input.Down("Duck");
		if (!detectDucks.TryAdd(index, isDuck))
			detectDucks[index] = isDuck;

		var eyeRot = ply.Camera.Head.Transform.Rotation;
		if (!eyeRotations.TryAdd(index, eyeRot))
			eyeRotations[index] = eyeRot;

		Log.Info($"[ReverseTime] Index {index} | TimeRecording {TimeRecording}");
	}

	public void StopRecord()
	{
		if (!onRecording) return;

		onRecording = false;

		Log.Info($"[ReverseTime] Stop recording {index}");

		StartPlay();
	}

	private string GetCurrentAction()
	{
		string value = "";

		//? can clean this code, I didn't find any Input.GetCurrentAction()
		if (Input.Released("use"))
		{
			value = "use";
		} 
		else if (Input.Released("attack1"))
		{
			value = "attack1";
		} 
		else if (Input.Released("attack2"))
		{
			value = "attack2";
		} 
		else if (Input.Down("Forward"))
		{
			value = "Forward";
		} 
		else if (Input.Down("Backward"))
		{
			value = "Backward";
		} 
		else if (Input.Down("Left"))
		{
			value = "Left";
		} 
		else if (Input.Down("Right"))
		{
			value = "Right";
		} 
		else if (Input.Released("Jump"))
		{
			value = "Jump";
		} 

		return value;
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

		string action = actions[indexPlay];
		if (action == "use")
		{
			//
		} 
		else if (action == "attack1")
		{
			//
		} 
		else if (action == "attack2")
		{
			//
		} 
		else if (action == "Forward")
		{
			mov.MoveForward();
		} 
		else if (action == "Backward")
		{
			mov.MoveBackward();
		} 
		else if (action == "Left")
		{
			mov.MoveLeft();
		} 
		else if (action == "Right")
		{
			mov.MoveRight();
		} 
		else if (action == "Jump")
		{
			mov.Jump();
		} 

		bool isDuck = detectDucks[indexPlay];
		mov.Duck(isDuck);

		bool isRun = detectRuns[indexPlay];
		if (isRun) mov.Run();

		Rotation eyeRot = eyeRotations[indexPlay];
		ply.Camera.Head.Transform.Rotation = eyeRot;
	}

	public void StopPlay()
	{
		if (!onPlaying) return;

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