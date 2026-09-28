namespace arc.common.Models.Lists;

/// <summary>
/// Request model for AddSpecimenTag and AddPatientTag events.
/// Id is the record id (specimen or patient). TagId or TagIds are from the picker (single or comma-separated).
/// TagName is when entering a new tag. Either TagId/TagIds or TagName must be provided.
/// </summary>
public class AddTagRequestModel
{
    /// <summary>Record id (specimen or patient). May be sent as number from frontend.</summary>
    public int Id { get; set; }

    /// <summary>Existing tag id(s) from picker. Single id or comma-separated ids for multi-select.</summary>
    public string TagId { get; set; }

    /// <summary>New tag name when user types in the text field.</summary>
    public string TagName { get; set; }

    /// <summary>When true, adds tags to existing ones without replacing. Used for batch add tag.</summary>
    public bool AddOnly { get; set; }
}
