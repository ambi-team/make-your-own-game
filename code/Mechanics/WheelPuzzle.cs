public sealed class WheelPuzzle : Component, IUsable
{
	#region Prop/Vars
	[Property, Description("Сторона для прохождения")] public int sideCorrect = 4;
	public int side = 1;
	private int sideMax = 12;

	private float degress = 0;
	private float degreesMaxForRotate;
	private Angles startAng;
	#endregion

	#region Logic
	public void ToRotate()
	{
		side = (side >= sideMax) ? 1 : ++side;

		degress += degreesMaxForRotate;

		GameObject.Transform.LocalRotation = Rotation.From(startAng.WithYaw(degress));
	}

	public bool IsComplete()
	{
		return (side == sideCorrect);
	}
	#endregion

	#region Components
	protected override void OnStart()
	{
		degreesMaxForRotate = 360 / sideMax;

		startAng = GameObject.Transform.Rotation.Angles();
	}

	public void Use(Player ply)
	{
		ToRotate();
	}
	#endregion
}