namespace SparkPlug.Contracts;

public class Include : IInclude
{
    public Include(string name, string[] columns, Include[] includes)
    {
        Name = name;
        Select = columns;
        Includes = includes;
    }
    public string Name { get; set; }
    public string[] Select { get; set; }
    public Include[] Includes { get; set; }
}
