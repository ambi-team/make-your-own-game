public static class SaveData
{
	private static BaseFileSystem data = FileSystem.Data;

	public static void Save(string filename, object value)
	{
		data.WriteJson(filename, value);
	}

	public static object Load<T>(string header)
	{	
		return data.ReadJson<T>(header);
	}
}