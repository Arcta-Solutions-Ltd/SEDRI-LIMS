using Newtonsoft.Json;
using System;

namespace arc.common.Models.Export
{
    public class ExportRunRequestDto
    {
        public string Name { get; set; }
        public ExportRunParameterDto Parameters { get; set; }
        public string ExportProfileId { get; set; }
    }
    public class ExportRunParameterDto
    {
        [JsonProperty("specimenTypeId")]
        public string SpecimenTypeIds { get; set; }

        [JsonProperty("specimenStateId")]
        public string SpecimenStateIds { get; set; }

        [JsonProperty("tagId")]
        public string TagIds { get; set; }

        [JsonProperty("organisationId")]
        public string OrganisationIds { get; set; }

        [JsonProperty("locationId")]
        public string LocationIds { get; set; }

        [JsonProperty("testId")]
        public string TestIds { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
