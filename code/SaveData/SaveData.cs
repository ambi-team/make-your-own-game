public static class SaveData
{
	private static BaseFileSystem data = FileSystem.Data;

	public static void Save(string header, object value)
	{
		data.WriteJson(header, value);
	}

	public static object Load<T>(string header)
	{
		var json = data.ReadAllText(header);

		return json;
	}
}