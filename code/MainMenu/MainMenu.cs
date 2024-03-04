using Sandbox.UI;

public sealed class MainMenu : Component
{
	[Property] public ScreenPanel MainMenuPanel { get; set; }

	private Achievement achi_press_esc = Achievement.CreateOrGet("press_esc_4_times", "The holy button", "Don't press Escape on four times", 4);
	
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
	}
}