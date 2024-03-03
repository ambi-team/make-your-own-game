using Sandbox.UI;
using System;

public sealed class MainMenu : Component
{
	[Property] public ScreenPanel MainMenuPanel { get; set; }

	private Achievement achi_press_esc = Achievement.CreateOrGet("press_esc", "The holy button", "Don't press Escape", 1);
	
	public void Show()
	{
		if (MainMenuPanel.Enabled)
		{
			Remove(); 

			return;
		}

        //Log.Info(achi_press_esc.Count);
        //achi_press_esc.Load();

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

		//Achievement.LoadAll();
		//Log.Info(ach.Count);
	}
}