public static class SaveData
{
	private static BaseFileSystem d = FileSystem.Data;

	public static void Save(string filename, object value)
	{
		Log.Info(d.GetFullPath(filename));
		d.WriteJson(filename, value);
	}

	public static object Load<T>(string filename)
	{	
		return d.ReadJson<T>(filename);
	}
}