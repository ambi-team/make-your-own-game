using System;

public sealed class ReverseTime : Component
{
	[Property] public GameObject ply;

	private bool onRecording;
	private int index = 0;
	private float time = 10f;
	private TimeUntil TimeRecording { get; set; }

	//todo 1. Сделать OnRecordInputsPlayers
	//todo 2. Записывать действия игрока 20 секунд (ключ = единица времени; значение = инпут) 
	//todo 3. Переделать PlayerController
	//todo 4. Воспроизвести + не забыть про предметы

	public void Record()
	{
		if (onRecording) return;

		index = 0;
		TimeRecording = time;
		onRecording = true;

		Log.Info($"[ReverseTime] Record");
	}

	public void Stop()
	{
		if (!onRecording) return;

		onRecording = false;
		Log.Info($"[ReverseTime] Stop {index}");
	}

	protected override void OnStart()
	{
		Record();
	}

	protected override void OnFixedUpdate()
	{
		if (!onRecording) return;
		if (TimeRecording)
		{
			Stop();

			return;
		}

		index++;
		Log.Info($"[ReverseTime] index {index} | timeRecording {TimeRecording}");
	}
}