namespace arc.common.Models.AST;

/// <summary>
/// Maps a stored antibiotic group id (listitem id or legacy <c>antibioticgroup.id</c>) to the resolved <c>antibioticgroup.id</c> table id.
/// </summary>
public class AntibioticGroupStoredTableIdPair
{
    public int StoredId { get; set; }
    public int TableId { get; set; }
}
