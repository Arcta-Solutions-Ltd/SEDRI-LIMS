namespace arc.domain.Configuration.BarcodeConfig
{
    public class BarcodeAssignmentConfig
    {
        public bool Enabled { get; set; }
        public int Width { get; set; }
        public string Discriminator { get; set; }
        public long SeedValue { get; set; }
    }
}
