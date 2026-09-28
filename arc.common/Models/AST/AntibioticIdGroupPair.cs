namespace arc.common.Models.AST;

/// <summary>
/// Maps an antibiotic to an antibiotic group listitem id (<c>antibioticcoding.codingid</c>, list 82).
/// </summary>
public class AntibioticIdGroupPair
{
    public int Id { get; set; }
    public int GroupId { get; set; }
}
