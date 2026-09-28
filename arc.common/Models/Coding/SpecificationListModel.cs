namespace arc.common.Models.Coding;
public class SpecificationListModel
{
    public int Id { get; set; }
    public int GuidelinesId { get; set; }
    public string Guidelines { get; set; }
    public int DocumentId { get; set; }
    public string Document { get; set; }
    public int VersionNumberId { get; set; }
    public string VersionNumber { get; set; }
    public int PublicationYearId { get; set; }
    public string PublicationYear { get; set; }

}