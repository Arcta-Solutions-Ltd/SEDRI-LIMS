namespace arc.data.model.Patient;

/// <summary>
/// Represents the fields in the patienttag table in the database.
/// Links patients to tags stored in the ListItem table (ListId=105).
/// </summary>
public class PatientTagDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the patient table to the patienttag table.
    /// </summary>
    public int? PatientId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the patienttag table.
    /// This links to the tag list (ListId=105) in the listitem table.
    /// </summary>
    public int? ListItemId { get; set; }
}
