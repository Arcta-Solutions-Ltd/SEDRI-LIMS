namespace arc.common.Models.AST;

/// <summary>
/// Identifies, by id only, an AST line (Disk/MIC or special-consideration line) that triggered an expert rule.
/// Used by the AST screen to draw the inline trigger icon and colour-coded hover next to the matching line.
/// </summary>
/// <remarks>
/// Matching on the client is performed by id (<see cref="TestMethod"/>, <see cref="AntibioticId"/>,
/// <see cref="SpecialConsiderationId"/>) so it remains correct when tags/list items are translated.
/// <see cref="SpecialConsiderationId"/> is normalized so that the &quot;no special consideration&quot; sentinel (973) is stored as 0,
/// matching a parent Disk/MIC line.
/// </remarks>
public class ExpertRuleTriggerModel
{
    /// <summary>Test method id of the triggering line: 681 Disk (zone) or 680 MIC.</summary>
    public int TestMethod { get; set; }

    /// <summary>Antibiotic id of the triggering line (0 when the condition did not target a specific antibiotic).</summary>
    public int AntibioticId { get; set; }

    /// <summary>Special consideration list id of the triggering line; 0 for a parent Disk/MIC line.</summary>
    public int SpecialConsiderationId { get; set; }
}
