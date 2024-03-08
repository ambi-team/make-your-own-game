public static class SaveData
{
	private static BaseFileSystem d = FileSystem.Data;

	public static void Save(string filename, object value)
	{
		d.WriteJson(filename, value);
	}

	public static object Load<T>(string header)
	{	
		return d.ReadJson<T>(header);
	}
}