public sealed class WheelPuzzle : Component, IUsable
{
	#region Prop/Vars
	public int angle = 1;
	private int angleMax = 12;
	private float pitch = 0; // cuz ToRotation will do normalize
	private float angleMaxForRotate;
	private Angles startAng;
	#endregion

	#region Logic
	public void ToRotate()
	{
		angle = (angle >= angleMax) ? 1 : ++angle;

		pitch += angleMaxForRotate;

		GameObject.Transform.LocalRotation = Rotation.From(startAng.WithPitch(pitch));
		Log.Info(angle);
	}
	#endregion

	#region Components
	protected override void OnStart()
	{
		angleMaxForRotate = 360 / angleMax;

		startAng = GameObject.Transform.Rotation.Angles();
	}

	public void Use(Player ply)
	{
		ToRotate();
	}
	#endregion
}