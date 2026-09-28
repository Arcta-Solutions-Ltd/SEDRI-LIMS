using System.Collections.Generic;
using Newtonsoft.Json;

namespace arc.common.Models.Instruments
{
    /// <summary>
    /// Represents the structure of a response for a single record from an external data source.
    /// Author: Arcta Solutions Limited
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// Gets or sets the profile name the response should use.
        /// </summary>
        public string ProfileName { get; set; } = "";
        /// <summary>
        /// Gets or sets the date and time of the response.
        /// </summary>
        public string DateTime { get; set; } = "";
        /// <summary>
        /// Gets or sets the identifier that is used to match the response with an existing record in the database.
        /// </summary>
        public string Identifier { get; set; } = "";
        /// <summary>
        /// Gets or sets the culture number.
        /// </summary>
        public string CultureNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the specimen type.
        /// </summary>
        public string SpecimenType { get; set; } = "";
        /// <summary>
        /// Gets or sets the growth value.
        /// </summary>
        public string Growth { get; set; } = "";
        /// <summary>
        /// Gets or sets the organism code.
        /// </summary>
        public string OrganismCode { get; set; } = "";
        /// <summary>
        /// Gets or sets the organism id which corresponds to the key on the organism table.
        /// </summary>
        public int OrganismId { get; set; } = 0;
        /// <summary>
        /// Gets or sets the accession number which corresponds to the accession number of the specimen.
        /// </summary>
        public string AccessionNumber { get; set; } = "";
        /// <summary>
        /// Gets or sets the culture id which corresponds to the key on the culture table.
        /// </summary>
        public int CultureId { get; set; }
        /// <summary>
        /// Gets or sets the specimen id which corresponds to the key on the specimen table.
        /// </summary>
        public int SpecimenId { get; set; } = 0;
        /// <summary>
        /// When set, matches <c>instrumentresults.id</c> for inbound resolution (echo from outbound / machine interface).
        /// </summary>
        public int InstrumentResultId { get; set; }
        /// <summary>
        /// Gets or sets the list of antibiotics.
        /// </summary>
        public List<AntibioticResponseModel> Antibiotics { get; set; } = new List<AntibioticResponseModel>();
        /// <summary>
        /// Gets or sets the id of the organism list that should be used to match an organism code against the organism list contained in the system.
        /// </summary>
        public int OrganismList { get; set; } = 0;
        /// <summary>
        /// Gets or sets the id of the antibiotic list that should be used to match an antibiotic code against the antibiotic list contained in the system.
        /// </summary>
        public int AntibioticList { get; set; } = 0;
        /// <summary>
        /// True or false specifying whether the response can override any organism identification currently stored in the system.
        /// </summary>
        public bool AllowIdOverwrite { get; set; } = false;
        /// <summary>
        /// True or false specifying whether any existing AST results held on the system can be overwritten.
        /// </summary>
        public bool AllowAstOverwrite { get; set; } = false;
        /// <summary>
        /// True or false specifying whether the response whould fail to update the database if any antibiotics in 'Antibiotics' are not recognised by the system.
        /// </summary>
        public bool IgnoreUnrecognisedAntibiotics { get; set; } = false;
        /// <summary>
        /// True or false specifying whether to use the susceptibility calculation from the external source or to allow Sedrilims to calculate susceptibilies itself.
        /// </summary>
        public bool UseMachineSusceptibility { get; set; } = false;
        public string NeedsApproval { get; set; } = "No";
        /// <summary>
        /// Optional InstrumentMachine list item id from the interface profile (audit / correlation; same as appsettings <c>InstrumentId</c> in MIS).
        /// </summary>
        public int? InstrumentMachineId { get; set; }
        /// <summary>
        /// Optional file attachment ids (from <c>POST api/file/upload</c>) to link as source instrument files for this result. Matching uses integer ids only.
        /// </summary>
        public List<int> SourceFileAttachmentIds { get; set; } = new List<int>();
        /// <summary>
        /// When non-null, replaces stored culture resistance mechanisms for this ingest (empty list clears). When null, existing mechanisms are left unchanged (older clients).
        /// </summary>
        [JsonProperty("resistanceMechanisms")]
        public List<ResistanceMechanismModel> ResistanceMechanisms { get; set; }
        public string Event { get; set; }

        public string DefaultGrowth { get; set; }
    }
}
