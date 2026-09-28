using System.Collections.Generic;

namespace arc.domain.Instruments
{
    /// <summary>
    /// Persisted settings for one instrument profile row within <see cref="InstrumentConfig"/>.
    /// </summary>
    public class SingleInstrumentConfig
    {
        public string Id { get; set; }
        public string LaboratoryId { get; set; }
        public string InstrumentName { get; set; }
        /// <summary>List item id on the InstrumentMachine list (vendor/platform).</summary>
        public string InstrumentMachineId { get; set; }
        public string Barcode { get; set; }
        public string SpecimenTypeId { get; set; }
        public string CultureTypeId { get; set; }
        public string DirectTestId { get; set; }
        public string CultureTestId { get; set; }
        public string ConfigListId { get; set; }
        public string InterfaceTypeId { get; set; }
        public string NeedsApproval { get; set; }
        public string DefaultGrowth { get; set; }
        public string OrganismGroupId { get; set; }
        /// <summary>
        /// Set when context is loaded from a culture row: <c>Culture.OrgGroupCodingId</c> (organism group on the isolate), for matching profile <see cref="OrganismGroupId"/>.
        /// </summary>
        public string CultureOrgGroupCodingId { get; set; }
        public string AllowIdOverwrite { get; set; }    
        public string AllowAstOverwrite { get; set; }
        public string AntibioticGroupId { get; set; }
        public string IgnoreUnrecognisedAntibiotics { get; set; }
        public string IsEnabled { get; set; }
        /// <summary>Selected export profile id (exportprofile.id) when <see cref="InterfaceTypeId"/> is Custom (list item id 10).</summary>
        public string ExportProfileId { get; set; }
        /// <summary>Combination rule for <see cref="InterfaceCriteria"/> as an AndOr list item id (list 103): 987 = And (default), 988 = Or.</summary>
        public string InterfaceCriteriaAndOr { get; set; }
        /// <summary>Optional interface criteria rows evaluated against the selected export profile's fields (Custom interface type only).</summary>
        public List<InterfaceCriteriaLine> InterfaceCriteria { get; set; }
    }
}
