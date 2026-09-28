using arc.common.Models.AST;
using arc.common.Models.Coding;
using arc.common.Models.Instruments;
using System.Collections.Generic;

namespace arc.app.AST;

public class ASTQueryModel
{
    /// <summary>
    /// When set, lists isolate test form names applicable for the current culture type and organism.
    /// Used by ASTIsolateTests to filter displayed tests when organism scope config exists.
    /// </summary>
    public List<string> ApplicableIsolateTests { get; set; }
    public int CultureId { get; set; }
    public int OrganismId { get; set; }

    /// <summary>
    /// Display name of the isolate organism (preferred synonym, otherwise genus/species/subspecies/serotype).
    /// Shown as a heading at the top of the AST screen and used to anchor the intrinsic guidance-rule icon/hover.
    /// </summary>
    public string OrganismName { get; set; }
    public int SpecimenTypeId { get; set; }
    public int CultureTypeId { get; set; }
    public int LaboratoryId { get; set; }
    public List<CultureTestStatusModel> CultureTests { get; set; }
    //public string TestPattern { get; set; }
    //public int TestPatternId { get; set; }
    //public string DiskTestUsePattern { get; set; }
    //public string StripTestUsePattern { get; set; }
    public string ASTAdditionalNotes { get; set; }
    public string CompletedDate { get; set; }
    public string CompletedTime { get; set; }
    public string ASTCommentOne { get; set; }
    //public int ASTCommentOneId { get; set; }
    public string ASTCommentTwo { get; set; }
    //public int ASTCommentTwoId { get; set; }
    public int SelectedTestPatternId { get; set; }
    //public TestPatternWithBreakpointsModel TestPatternToSelect { get; set; }
    public List<TestPatternScopeModel> TestPatternOptions { get; set; }
    public List<ASTRowModel> DiskResults { get; set; } = [];
    public List<ASTRowModel> MicResults { get; set; } = [];

    public List<ASTRowModel> ExpertRuleResults { get; set; } = [];

    /// <summary>
    /// Expert rules grouped by logical rule id with one <see cref="ExpertRuleGroupModel.ApplyRule"/> per group and one AST row per action line.
    /// </summary>
    public List<ExpertRuleGroupModel> ExpertRuleGroups { get; set; } = [];

    /// <summary>
    /// Expert rules with no actions: informational only; shown as top alerts on the AST screen, not in the Expert Rules section.
    /// </summary>
    public List<ExpertRuleCommentAlertModel> ExpertRuleCommentAlerts { get; set; } = [];

    /// <summary>
    /// Instrument-reported resistance mechanisms for this culture (read-only in the portal).
    /// </summary>
    public List<ResistanceMechanismModel> ResistanceMechanisms { get; set; } = [];

    /// <summary>
    /// When 'Yes', manual susceptibility changes require audit reason entry on the AST screen.
    /// </summary>
    public string RecordSusceptibilityChangeAudit { get; set; } = "No";


}
