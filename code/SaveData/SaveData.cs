public static class SaveData
{
	public static void Save(string filename, object value)
	{
		FileSystem.Data.WriteJson(filename, value);
	}

	public static object Load<T>(string header)
	{
		var json = FileSystem.Data.ReadAllText(header);

		return json;
	}
}