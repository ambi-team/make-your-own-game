public class StatsData : ISaveData
{
	#region Props/Vars
	private string filename = "stats";

	#region Entered The Red Door
	private bool _level1EnteredTheRedDoor = false;
	public bool Level1EnteredTheRedDoor
	{
		get { return _level1EnteredTheRedDoor; }
		set { _level1EnteredTheRedDoor = value; Save(); }
	}

	private bool _level2EnteredTheRedDoor = false;
	public bool Level2EnteredTheRedDoor
	{
		get { return _level2EnteredTheRedDoor; }
		set { _level2EnteredTheRedDoor = value; Save(); }
	}

	private bool _level3EnteredTheRedDoor = false;
	public bool Level3EnteredTheRedDoor
	{
		get { return _level3EnteredTheRedDoor; }
		set { _level3EnteredTheRedDoor = value; Save(); }
	}

	private bool _level4EnteredTheRedDoor = false;
	public bool Level4EnteredTheRedDoor
	{
		get { return _level4EnteredTheRedDoor; }
		set { _level4EnteredTheRedDoor = value; Save(); }
	}
	#endregion

	#region Finish Additional Challenges
	private bool _level1AddChallengeFinished = false;
	public bool Level1AddChallengeFinished
	{
		get { return _level1AddChallengeFinished; }
		set { _level1AddChallengeFinished = value; Save(); }
	}

	private bool _level2AddChallengeFinished = false;
	public bool Level2AddChallengeFinished
	{
		get { return _level2AddChallengeFinished; }
		set { _level2AddChallengeFinished = value; Save(); }
	}

	private bool _level3AddChallengeFinished = false;
	public bool Level3AddChallengeFinished
	{
		get { return _level3AddChallengeFinished; }
		set { _level3AddChallengeFinished = value; Save(); }
	}

	private bool _level4AddChallengeFinished = false;
	public bool Level4AddChallengeFinished
	{
		get { return _level4AddChallengeFinished; }
		set { _level4AddChallengeFinished = value; Save(); }
	}
	#endregion

	#region Which Sigil
	private int _level2Sigil = 0; // 0 - Фотокарточка леса, 1 - Фигурка молнии, 2 - Танцующий кактус
	public int Level2Sigil {
		get { return _level2Sigil; }
		set { _level2Sigil = value; Save(); } 
	}

	private int _level3Sigil = 0; // 0 - Открытка “С новым годом!”, 1 - Вейп, 2 - Каменный обелиск
	public int Level3Sigil
	{
		get { return _level3Sigil; }
		set { _level3Sigil = value; Save(); }
	}

	private int _level4Sigil = 0; // 0 - Медалька, 1 - Кружка с кофе, 2 - Фигурка ангела
	public int Level4Sigil
	{
		get { return _level4Sigil; }
		set { _level4Sigil = value; Save(); }
	}
	#endregion

	#region Logic
	public StatsData() 
	{ 
	}
	#endregion
	#endregion

	#region ISaveData
	public void Save()
	{
		SaveData.Save(filename + ".json", this);

		Log.Info($"[Stats] saved to SaveData");
	}

	public void Load()
	{
		if (!FileSystem.Data.FileExists(filename + ".json"))
		{
			Save();

			return;
		}

		StatsData data = (StatsData)SaveData.Load<StatsData>(filename + ".json");

		Level1EnteredTheRedDoor = data.Level1EnteredTheRedDoor;
		Level2EnteredTheRedDoor = data.Level2EnteredTheRedDoor;
		Level3EnteredTheRedDoor = data.Level3EnteredTheRedDoor;
		Level4EnteredTheRedDoor = data.Level4EnteredTheRedDoor;

		Level1AddChallengeFinished = data.Level1AddChallengeFinished;
		Level2AddChallengeFinished = data.Level2AddChallengeFinished;
		Level3AddChallengeFinished = data.Level3AddChallengeFinished;
		Level4AddChallengeFinished = data.Level4AddChallengeFinished;

		Level2Sigil = data.Level2Sigil;
		Level3Sigil = data.Level3Sigil;
		Level4Sigil = data.Level4Sigil;

		Log.Info($"[Stats] loaded from SaveData");
	}
	#endregion
}