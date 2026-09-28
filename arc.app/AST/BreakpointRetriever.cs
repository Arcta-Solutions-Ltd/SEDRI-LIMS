using arc.app.Coding;
using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Coding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.AST
{
    public class BreakpointRetriever
    {
        private readonly IBreakpointRepository _breakpointRepository;

        public BreakpointRetriever(IBreakpointRepository breakpointRepository)
        {
            _breakpointRepository = breakpointRepository;
        }

        public async Task<List<Breakpoint>> GetBreakpointsFromCriteria(BreakpointCriteriaModel criteria)
        {
            if (criteria.SourceId == 0)
            {
                return new List<Breakpoint>();
            }

            var breakpointQueryFilters = new QueryFilterConfig
            {
                Parameters = new List<QueryValuesConfig>
                {
                    new QueryValuesConfig() { Key = "SpecimenTypeId", Value = criteria.SpecimenTypeId.ToString() },
                    new QueryValuesConfig() { Key = "OrganismId", Value = criteria.OrganismId.ToString() },
                    new QueryValuesConfig() { Key = "OrgGroupCodingId", Value = criteria.OrgGroupCodingId.ToString() },
                    new QueryValuesConfig() { Key = "AntibioticId", Value = criteria.AntibioticId.ToString() },
                    new QueryValuesConfig() { Key = "TestMethodId", Value = criteria.TestMethodId.ToString() },
                    new QueryValuesConfig() { Key = "GuidelinesId", Value = criteria.SourceId.ToString() },
                    new QueryValuesConfig() { Key = "Dosage", Value = criteria.Dosage }
                }
            };
            var breakpoints = await _breakpointRepository.GetBreakpointsForOrganismAsync(breakpointQueryFilters);
            return breakpoints;
        }
    }
}
