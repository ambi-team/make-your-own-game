using Sandbox;

public sealed class Last : Component
{
	[Property] public GameObject map;
	protected override void OnStart()
	{
		foreach (var item in map.Children)
		{
			if (item.Tags.Has("skybox"))
				item.Enabled = false;
		}
	}
}