public sealed class WheelPuzzle : Component, IUsable
{
	#region Prop/Vars
	public int angle = 0;
	private int angleMax = 12;
	private float angleMaxForRotate;
	private float pitch = 0; // cuz ToRotation will do normalize
	#endregion

	#region Logic
	public void ToRotate()
	{
		angle++;
		//if (angle > angleMax) angle = 0;

		var ang = GameObject.Transform.LocalRotation.Angles();
		if (pitch == 0) pitch = ang.pitch;
		pitch += 15;
		var yaw = ang.yaw;
		var roll = ang.roll;
		GameObject.Transform.LocalRotation = new Angles(pitch, yaw, roll).ToRotation();
		Log.Info(pitch);
	}
	#endregion

	#region Components
	protected override void OnStart()
	{
		angleMaxForRotate = 360 / angleMax;
	}

	public void Use(Player ply)
	{
		ToRotate();
	}
	#endregion
}