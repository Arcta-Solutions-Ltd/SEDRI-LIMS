namespace arc.domain.Configuration.ReportsConfig;

public class DataSectionGridConfig
{
    private string _name;

    public string Name
    {
        get => _name;
        set => _name = value?.Replace(" ", "");
    }
    public string Data { get; set; }
    public string Description { get; set; }
}
