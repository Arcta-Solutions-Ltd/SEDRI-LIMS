namespace arc.common.Models.Specimen;

/// <summary>
/// Input for create-culture command (insert a new culture row for specimen and culture type).
/// </summary>
public class CreateCultureForSpecimenAndTypeModel
{
    public int SpecimenId { get; set; }
    public int TypeId { get; set; }
}
