using Sandbox.UI;
using System;

public sealed class MainMenu : Component
{
	[Property] public ScreenPanel MainMenuPanel { get; set; }
	[Property] public Achievement ach;

	public void Show()
	{
		if (MainMenuPanel.Enabled)
		{
			Remove(); 

			return;
		}

		MainMenuPanel.Enabled = true;
	}

	public void Remove()
	{
		MainMenuPanel.Enabled = false;
	}

	protected override void OnUpdate()
	{
		if (Input.EscapePressed)
			Show();
	}

	protected override void OnStart()
	{
		if (MainMenuPanel is null)
			MainMenuPanel = GameObject.Components.Get<ScreenPanel>();

		MainMenuPanel.Enabled = false; //! always to false on start

		Achievement.LoadAll();
		Log.Info(ach.Count);
	}
}