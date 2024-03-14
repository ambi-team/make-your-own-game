using System.Text.Json.Serialization;

public class SettingsData : ISaveData
{
	#region Props/Vars
	public float FOV = 75f;
	[JsonIgnore] public Player ply;
	#endregion

	#region Logic
	public void SetPlayer(Player player)
	{
		ply = player;
	}
	#endregion

	#region ISaveData
	public void Save()
	{
		//todo
	}

	public void Load()
	{
		//todo
	}
	#endregion
}