using arc.common.Models.AST;
using System.Collections.Generic;

namespace arc.app.AST
{
    /// <summary>
    /// Represents an antibiotic line from a test pattern, optionally enriched with special consideration breakpoints.
    /// </summary>
    public class AntibioticLineWithBreakpointsModel
    {
        public int Id { get; set; }
        public int testOrder { get; set; }
        public int? AntibioticId { get; set; }
        public string Dosage { get; set; }
        public int TestMethodId { get; set; }
        public int GuidelinesId { get; set; }
        public int CategoryId { get; set; }
        public string PrintOnReport { get; set; }

        /// <summary>
        /// Special consideration rows derived from breakpoints matching Organism, Antibiotic, Guideline and Dosage (Disk) or Organism, Antibiotic, Guideline (MIC).
        /// Displayed directly under the parent test pattern line on the AST screen.
        /// </summary>
        public List<ASTRowModel> EmbeddedASTRows { get; set; }
    }
}
