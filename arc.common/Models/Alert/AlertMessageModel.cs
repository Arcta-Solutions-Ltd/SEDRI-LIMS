namespace arc.common.Models.Alert
{
    public class AlertMessageModel
    {
        public int AlertTypeId { get; set; }
        public int PositionId { get; set; }
        public int ReportPositionId { get; set; }
        public string Colour { get; set; }
        public string Message { get; set; }
        public string SpecimenOrganism { get; set; }
    }
}
