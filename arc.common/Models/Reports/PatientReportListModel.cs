namespace arc.common.Models.Reports
{
    /// <summary>
    /// Row shape for report history grids scoped by patient, admission, or request record views.
    /// </summary>
    public class PatientReportListModel
    {
        public string AccessionNumber { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ReportConfig { get; set; }
        public string LastModifiedDate { get; set; }
    }
}
