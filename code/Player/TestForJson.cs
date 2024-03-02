using System.Text.Json.Serialization;

public class TestForJson
{
    public int A { get; set; }
	public bool B { get; set; }
	public float C { get; set; }
    [Property] public string Name { get; private set; }
	[JsonIgnore] public ScreenPanel a { get; set; }

    public TestForJson(int a, bool b, float c, string name)
	{
		A = a;
		B = b;
		C = c;
		Name = name;
	}
}
