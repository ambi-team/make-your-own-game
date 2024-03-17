using System;

public sealed class WheelPuzzleSystem : Component
{
	#region Props/Vars
	[Property] public WheelPuzzle wheelBig;
	[Property] public WheelPuzzle wheelMedium;
	[Property] public WheelPuzzle wheelSmall;
	[Property] public BarrierWheelPuzzle barrier;

	private bool readyWheelBig = false;
	private bool readyWheelMedium = false;
	private bool readyWheelSmall = false;

	[Property] public bool isCompleted = false;
	#endregion

	#region Hooks
	[Property] public Action OnCompeleted;
	#endregion

	#region System
	private void CheckChallenge(WheelPuzzle wheel)
	{
		var obj = wheel.GameObject;

		if (wheelBig.GameObject == obj)
			readyWheelBig = wheel.IsComplete();
		else if (wheelMedium.GameObject == obj)
			readyWheelMedium = wheel.IsComplete();
		else if (wheelSmall.GameObject == obj)
			readyWheelSmall = wheel.IsComplete();

		if (readyWheelBig && readyWheelMedium && readyWheelSmall)
		{
			wheelBig.canRotate = false;
			wheelMedium.canRotate = false;
			wheelSmall.canRotate = false;

			isCompleted = true;

			OnCompeleted?.Invoke();

			Log.Info($"[WheelPuzzleSystem] {this} has completed");
		}
	}
	#endregion

	#region Components
	protected override void OnStart()
	{
		if (wheelBig is null) { Log.Error($"[WheelPuzzleSystem] Require Wheel Big!"); return; }
		if (wheelMedium is null) { Log.Error($"[WheelPuzzleSystem] Require Wheel Medium!"); return; }
		if (wheelSmall is null) { Log.Error($"[WheelPuzzleSystem] Require Wheel Small!"); return; }

		wheelBig.OnRotated += CheckChallenge;
		wheelMedium.OnRotated += CheckChallenge;
		wheelSmall.OnRotated += CheckChallenge;
	}
	#endregion
}