using arc.common.Models.Export;
using System.Linq;

namespace arc.common.Utils
{
    public static class ExportRunRequestDtoExtension
    {
        public static ExportRunRequestModel ToExportRunRequestModel(this ExportRunRequestDto exportRunRequestDto)
        {
            return new ExportRunRequestModel
            {
                StartDate = exportRunRequestDto.Parameters.StartDate,
                EndDate = exportRunRequestDto.Parameters.EndDate,
                ExportProfileId = exportRunRequestDto.ExportProfileId,
                LocationIds = exportRunRequestDto.Parameters.LocationIds?.Split(',').ToList(),
                OrganisationIds = exportRunRequestDto.Parameters.OrganisationIds?.Split(",").ToList(),
                SpecimenStateIds = exportRunRequestDto.Parameters.SpecimenStateIds?.Split(",").ToList(),
                SpecimenTypeIds = exportRunRequestDto.Parameters.SpecimenTypeIds?.Split(",").ToList(),
                TagIds = exportRunRequestDto.Parameters.TagIds?.Split(",").ToList(),
                TestIds = exportRunRequestDto.Parameters.TestIds?.Split(",").ToList()
            };
        }
    }
}
