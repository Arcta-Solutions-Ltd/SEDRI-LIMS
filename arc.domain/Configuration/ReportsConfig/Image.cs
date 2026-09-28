namespace arc.domain.Configuration.ReportsConfig;

public class Image
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] ImageData { get; set; }
    public string Format { get; set; }
}
