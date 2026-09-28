using System;
using System.Collections.Generic;

namespace arc.common.Models.AST
{
    public class ASTModel
    {
        public int Id { get; set; }
        public string TestType { get; set; }
        public string EntryType { get; set; }
        public string TestMethod { get; set; }
        public int TestMethodId { get; set; }
        public string Antibiotic { get; set; }
        public int AntibioticId { get; set; }
        public int AntibioticGroupId { get; set; }
        public int Dosage { get; set; }
        public string Guidelines { get; set; }
        public int GuidelinesId { get; set; }
        public string Measurement { get; set; }
        public string MicComparison { get; set; }
        public string Susceptibility { get; set; }
        public int SusceptibilityId { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string AdditionalNotes { get; set; }
        public string IncludeInReport { get; set; }
        public string DisplayOnReport { get; set; }
        public int AppliedBreakpointId { get; set; }
        public List<ASTSpecialRowModel> SpecialRows { get; set; }
        public bool ExpertRuleLine { get; set; }
        public int ExpertRuleId {get; set;}
        public string ExpertRuleName { get; set; }
        public string ExpertRuleText { get; set; }
        public List<ExpertRuleActionReturnModel> RuleActions { get; set; }

        /// <summary>Manual susceptibility override audit for this AST line.</summary>
        public AstSusceptibilityOverrideModel SusceptibilityOverride { get; set; }
    }
}

//var comp = item.MicComparison == null ? "" : item.MicComparison.Trim();
//item.Measurement = comp + item.Measurement.Trim();
//item.TestMethod = item.TestMethodId.ToString();
//item.Antibiotic = item.AntibioticId.ToString();
//item.Guidelines = item.GuidelinesId.ToString();
//item.Susceptibility = item.SusceptibilityId.ToString();
//item.Category = item.CategoryId.ToString();