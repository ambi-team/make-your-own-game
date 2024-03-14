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
		throw new System.NotImplementedException();
	}

	public void Load()
	{
		throw new System.NotImplementedException();
	}
	#endregion
}