public class TestJson
{
    public int PropInt { get; set; }
	public bool PropBool { get; }
	public string PropStr { get; set; }

	public TestJson(int a, bool b, string c)
	{
		PropInt = a; PropBool = b; PropStr = c;
	}

	public TestJson() { }
}