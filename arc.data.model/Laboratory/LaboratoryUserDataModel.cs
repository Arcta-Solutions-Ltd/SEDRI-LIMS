namespace arc.data.model.Laboratory;

/// <summary>
/// Represents the fields in the laboratoryuser table in the database.
/// </summary>
public class LaboratoryUserDataModel : IdBase
{
    /// <summary>
    /// Gets or sets foreign key linking the laboratory table to the laboratoryuser table.
    /// </summary>
    public int LaboratoryId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the users table to the laboratoryuser table.
    /// </summary>
    public int UserId { get; set; }
}
