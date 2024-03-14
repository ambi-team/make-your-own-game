public class SettingsData : ISaveData
{
	#region Props/Vars
	public float FOV = 75f;
	public Player ply;
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