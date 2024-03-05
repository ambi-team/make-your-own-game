using System;

public sealed class ReverseTime : Component
{
	[Property] public GameObject ply;

	public bool onRecording;
	public bool onPlaying;

	private int index = 0;

	private Vector3 startPos;
	//todo startEyeAngles
	private float defaultTimeRecoding = 5f;
	private TimeUntil TimeRecording { get; set; }

	public Dictionary<float, string> actions = new();
	//todo record mouse axis

	//todo 3. Переделать PlayerController
	//todo 4. Воспроизвести + не забыть про предметы

	#region Record logic
	public void StartRecord()
	{
		if (onRecording) return;
		if (onPlaying) return;

		if (actions.Count > 0)
		{
			for (int i = 0; i < actions.Count; i++)
				actions[i] = "";
		}
		index = 0;
		TimeRecording = defaultTimeRecoding;
		onRecording = true; // native start

		Log.Info($"[ReverseTime] Start {index}");
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

		var value = GetCurrentAction();

		if (!actions.TryAdd(index, value)) 
			actions[index] = value;

		Log.Info($"[ReverseTime] Index {index} | TimeRecording {TimeRecording}");
	}

	public void StopRecord()
	{
		if (!onRecording) return;

		onRecording = false;

		Log.Info($"[ReverseTime] Stop {index}");

		foreach (var key in actions.Keys)
			Log.Info($"Key {key} | Value {actions[key]}");
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
		else if (Input.Released("Forward"))
		{
			value = "Forward";
		} 
		else if (Input.Released("Backward"))
		{
			value = "Backward";
		} 
		else if (Input.Released("Left"))
		{
			value = "Left";
		} 
		else if (Input.Released("Right"))
		{
			value = "Right";
		} 
		else if (Input.Released("Jump"))
		{
			value = "Jump";
		} 
		else if (Input.Released("Run"))
		{
			value = "Run";
		} 
		else if (Input.Released("Walk"))
		{
			value = "Walk";
		} 
		else if (Input.Released("Duck"))
		{
			value = "Duck";
		}

		return value;
	}
	#endregion

	#region Play logic
	public void StartPlay()
	{

	}

	public void Play()
	{

	}

	public void StopPlay()
	{

	}
	#endregion

	#region Components
	protected override void OnStart()
	{
		//StartRecord(); //todo remove
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