using Sandbox.UI;
using System;

public sealed class MainMenu : Component
{
	[Property] public ScreenPanel pnl;

	public void Show()
	{
		if (pnl.Enabled)
		{
			Remove(); 

			return;
		}

		pnl.Enabled = true;
	}

	public void Remove()
	{
		pnl.Enabled = false;
	}

	protected override void OnUpdate()
	{
		if (Input.EscapePressed)
			Show();
	}

	protected override void OnStart()
	{
		pnl.Enabled = false; //! always to false on start
	}
}