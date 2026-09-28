using arc.common.Models.Coding;
using System.Collections.Generic;

namespace arc.common.Models.Instruments
{
    /// <summary>
    /// Represents the structure of AST Antibiotic responses that come from an external sourrce of data. Is repferenced from the ResponseModel.
    /// Author: Arcta Solutions Limited
    /// </summary>
    public class AntibioticResponseModel
    {
        /// <summary>
        /// Gets or sets the antibiotic code.
        /// </summary>
        public string Code { get; set; } = "";
        /// <summary>
        /// Gets or sets the Mic Sign ('>','>=','<', '<=','=','').
        /// </summary>
        public string MicSign { get; set; } = "";
        /// <summary>
        /// Gets or sets the Mic Value.
        /// </summary>
        public string MicValue { get; set; } = "";
        /// <summary>
        /// Gets or sets the Susceptibility such as: R for Resistant, I for Intermediate, S for Susceptible. Much mach a code from the susceptibility list in the system.
        /// </summary>
        public string Susceptibility { get; set; } = "";
        /// <summary>
        /// Gets or sets the antibiotic id which corresponds to the key on the antibiotic table..
        /// </summary>
        public int AntibioticId { get; set; } = 0;
        /// <summary>
        /// Gets or sets the list of Breakpoints.
        /// </summary>
        public List<BreakpointModel> Breakpoints { get; set; } = [];
        /// <summary>
        /// Yes/No for whether this AST line displays on the report; set from the instrument (e.g. Vitek2 suppressedDrug inverted).
        /// </summary>
        public string PrintOnReport { get; set; } = "Yes";
    }
}
