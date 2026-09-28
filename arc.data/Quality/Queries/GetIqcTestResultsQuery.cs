using arc.common.Models.QualityAssurance;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    internal class GetIqcTestResultsQuery : IQueryReturningType<List<IqcResultGridViewModel>>
    {
        public async Task<List<IqcResultGridViewModel>> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("id");

            var sql = @"SELECT
                ir.id as id,
                qo.standardsbody,
				qo.primarystrain,
                TRIM(CONCAT(g.name,case when g.name is not null and s.name is null and ad.name is null then ' spp.' else '' end, ' ',s.name,' ',ss.name,' ',TRIM(se.name), ad.name)) as organismname,
                a.antibioticname,
                ir.value,
                li.value as testmethod,
                qa.micrangelower,
                qa.micrangeupper,
                qa.inhibitionzonediameterrangelower as diskrangelower,
                qa.inhibitionzonediameterrangeupper as diskrangeupper
                FROM iqctests it
                LEFT JOIN iqcresults ir on ir.iqctestid = it.id
                LEFT JOIN qcantibiotics qa on qa.id = ir.qcantibioticid
                LEFT JOIN antibiotic a on a.id = qa.antibioticid
                LEFT JOIN qcorganisms qo on qo.id = qa.qcorganismid
                LEFT JOIN iqctestprofiles itp on itp.id = it.iqctestprofileid
                LEFT JOIN listitem li on li.id = itp.testmethodlistitemid
                INNER JOIN organism og on qo.organismid = og.id
                LEFT OUTER JOIN genus g on g.Id = og.genusId
                LEFT OUTER JOIN species s on s.Id = og.speciesId
                LEFT OUTER JOIN subspecies ss on ss.id = og.subspeciesId
                LEFT OUTER JOIN serotype se on se.Id = og.serotypeId
                LEFT OUTER JOIN additional ad on ad.Id = og.additionalId
                WHERE it.id = @Id
                ORDER by ir.id desc;
				";

            var iqcResults = await connection.QueryAsync<dynamic>(sql, new { Id = id });

            var viewModelResults = new List<IqcResultGridViewModel>();
            foreach (var result in iqcResults)
            {
                string organismName = $"{result.organismname} ({result.standardsbody}, {result.primarystrain})";
                string antibioticName = result.antibioticname;
                string standardsBody = result.standardsbody;
                string testMethod = result.testmethod;
                decimal? diskRangeLower = result.diskrangelower;
                decimal? diskRangeUpper = result.diskrangeupper;
                decimal? micRangeLower = result.micrangelower;
                decimal? micRangeUpper = result.micrangeupper;
                decimal? value = result.value;

                var upperLowerValues = GetUpperAndLowerValueRangeValues(testMethod, diskRangeLower, diskRangeUpper, micRangeLower, micRangeUpper);
                var isInRange = value != null ? IsInRange(result.value, upperLowerValues.Item1, upperLowerValues.Item2) : true;
                viewModelResults.Add(new IqcResultGridViewModel()
                {
                    Id = result.id,
                    OrganismName = organismName,
                    AntibioticName = antibioticName,
                    Value = value,
                    TestMethod = testMethod,
                    RangeLower = upperLowerValues.Item1,
                    RangeUpper = upperLowerValues.Item2,
                    WithinRange = isInRange,
                    AlertCategoryId = isInRange ? 0 : value == 0 ? 0 : 990,
                    AlertTitle = isInRange ? null : $"@GenExpVal@: {upperLowerValues.Item1}-{upperLowerValues.Item2}",
                    Colour = isInRange ? null : value == 0 ? null : "#FF0000",
                    StandardsBody = standardsBody,

                });
            }
            return viewModelResults;
        }

        private Tuple<decimal?, decimal?> GetUpperAndLowerValueRangeValues(string testMethod, decimal? diskRangeLower, decimal? diskRangeUpper, decimal? micRangeLower, decimal? micRangeUpper)
        {
            return testMethod.ToLower() switch
            {
                "disk" => Tuple.Create(diskRangeLower, diskRangeUpper),
                "mic" => Tuple.Create(micRangeLower, micRangeUpper),
                _ => throw new Exception("Not a recognised test type"),
            };
        }

        private bool IsInRange(decimal? num, decimal? lower, decimal? upper)
        {
            if (num.HasValue && lower.HasValue && upper.HasValue)
            {
                return (num >= lower && num <= upper);
            }
            return true;
        }
    }
}
