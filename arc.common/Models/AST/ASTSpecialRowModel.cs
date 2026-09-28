using System;

namespace arc.common.Models.AST
{
    /// <summary>
    /// Persisted special-consideration row (<c>specialastrow</c>) linked to a parent AST line.
    /// </summary>
    public class ASTSpecialRowModel
    {
        public string Name { get; set; }
        public string Susceptibility { get; set; }
        public int SusceptibilityId { get; set; }
        public int SpecialTypeId { get; set; }
        public string IncludeInReport { get; set; }
        /// <summary><c>breakpoint.id</c> that produced this special consideration susceptibility (0 when none).</summary>
        public int BreakpointId { get; set; }

        /// <summary>Manual susceptibility override audit for this embed line.</summary>
        public AstSusceptibilityOverrideModel SusceptibilityOverride { get; set; }
        //public string DisplayOnReport { get; set; }
    }
}
