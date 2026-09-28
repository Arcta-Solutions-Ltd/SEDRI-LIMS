namespace arc.common.Models.Laboratory;
public class LaboratoryDetailsModel
{
    /// <summary>
    /// Represents the identifier for the laboratory group.
    /// This value is used to categorize or associate related laboratory data.
    /// </summary>
    public string GroupId { get; set; }

    /// <summary>
    /// Represents the identifier for an associated list.
    /// This value is linked to additional data that may be relevant to the laboratory details.
    /// </summary>
    public string AssociatedListId { get; set; }
}
