namespace arc.common.Models.Laboratory;

/// <summary>
/// Model for organism scope culture test option configuration stored in laboratoryconfigs.
/// Contains organism scope fields (taxonomy or organism group) and the associated isolate test form names.
/// </summary>
public class OrganismScopeCultureTestOptionModel
{
    /// <summary>
    /// Gets or sets the order taxonomy ID.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the family taxonomy ID.
    /// </summary>
    public int FamilyId { get; set; }

    /// <summary>
    /// Gets or sets the genus taxonomy ID (from organism table).
    /// </summary>
    public int GenusId { get; set; }

    /// <summary>
    /// Gets or sets the species taxonomy ID (from organism table).
    /// </summary>
    public int SpeciesId { get; set; }

    /// <summary>
    /// Gets or sets the subspecies taxonomy ID (from organism table).
    /// </summary>
    public int SubspeciesId { get; set; }

    /// <summary>
    /// Gets or sets the serotype taxonomy ID (from organism table).
    /// </summary>
    public int SerotypeId { get; set; }

    /// <summary>
    /// Gets or sets the organism group coding ID (from organismcoding/listitem).
    /// </summary>
    public int OrgGroupCodingId { get; set; }

    /// <summary>
    /// Gets or sets the specific organism ID when scope is organism-level.
    /// </summary>
    public int OrganismId { get; set; }

    /// <summary>
    /// Gets or sets the comma-separated list of isolate test form names.
    /// </summary>
    public string AssociatedListId { get; set; }

    /// <summary>
    /// Gets or sets the human-readable organism scope description (e.g. for edit form display).
    /// Not persisted; populated by queries when loading for edit.
    /// </summary>
    public string GroupDescription { get; set; }
}
