using arc.common.Models.QualityAssurance;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    internal class GetDiskAntibioticsDetailsForIqcTestProfileQcOrganismQuery : IQueryReturningType<List<IqcTestProfileQcAntibioticTableRow>>
    {
        public async Task<List<IqcTestProfileQcAntibioticTableRow>> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("iqctestprofileqcorganismid");

            var sql = @"
                select li.id
                from iqctestprofiles itp
                inner join iqctestprofileqcorganisms itpqo on itpqo.iqctestprofileid = itp.id
                inner join listitem li on li.id = itp.testmethodlistitemid
                where itpqo.id = @id
                ";
            var listItemId = await connection.QueryFirstAsync<int>(sql, new { Id = id });
            if (listItemId != 681)
            {
                return new List<IqcTestProfileQcAntibioticTableRow>();
            }

            sql = @"
                select 
                itpqa.id,
                a.antibioticname as ""Name"",
                case when itpqa.enabled = true then '@GenYesA@' else '@GenNo@' end as ""Enabled"",
                qa.diskcontent as ""Content"",
                qa.inhibitionzonediameterrangelower as ""RangeLower"",
                qa.inhibitionzonediameterrangeupper as ""RangeUpper"",
                qa.inhibitionzonediametertargetlower as ""TargetLower"",
                qa.inhibitionzonediametertargetupper as ""TargetUpper""
                from iqctestprofileqcorganisms itpqo 
                inner join iqctestprofiles itp on itp.id = itpqo.iqctestprofileid
                inner join iqctestprofileqcantibiotics itpqa on itpqa.iqctestprofileqcorganismid = itpqo.id
                inner join qcantibiotics qa on qa.id = itpqa.qcantibioticid
                inner join antibiotic a on a.id = qa.antibioticid
                where itpqo.id = @id
			  ";

            var result = await connection.QueryAsync<IqcTestProfileQcAntibioticTableRow>(sql, new { Id = id });
            return result.ToList();
        }
    }
}
