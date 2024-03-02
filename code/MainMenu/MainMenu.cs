using Sandbox.UI;
using System;

public sealed class MainMenu : Component
{
	[Property] public ScreenPanel pnl;

	private bool isShow = false;

	public void Show()
	{
		if (isShow)
		{
			Remove(); 

			return;
		}

		isShow = true;
		pnl.Enabled = true;
	}

	public void Remove()
	{
		isShow = false;
		pnl.Enabled = false;
	}

	protected override void OnUpdate()
	{
		if (Input.EscapePressed)
			Show();
	}

	protected override void OnStart()
	{
		pnl.Enabled = false;
	}
}