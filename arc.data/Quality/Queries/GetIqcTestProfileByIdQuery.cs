using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Quality;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Quality.Queries
{
    internal class GetIqcTestProfileByIdQuery : IQueryReturningType<IqcTestProfile>
    {
        public async Task<IqcTestProfile> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("iqctestprofileid");

            var sql = @$"SELECT itp.*,  itpqo.*, itpqa.* FROM iqctestprofiles itp
                    INNER JOIN iqctestprofileqcorganisms itpqo on itpqo.iqctestprofileid = itp.id
                    INNER JOIN iqctestprofileqcantibiotics itpqa on itpqa.iqctestprofileqcorganismid = itpqo.id
                    WHERE itp.id = @id";

            IqcTestProfile iqcTestProfile = null;

            await connection.QueryAsync<IqcTestProfile, IqcTestProfileQcOrganism, IqcTestProfileQcAntibiotic, IqcTestProfile>(sql,
                map: (itp, itpqco, itpqca) =>
                {
                    iqcTestProfile ??= itp;
                    if (itpqco != null && !iqcTestProfile.IqcTestProfileQcOrganisms.Any(x => x.Id == itpqco.Id))
                    {
                        iqcTestProfile.IqcTestProfileQcOrganisms.Add(itpqco);
                    }
                    if (itpqca != null &&
                    !iqcTestProfile.IqcTestProfileQcOrganisms.Where(x => x.Id == itpqca.IqcTestProfileQcOrganismId).First().IqcTestProfileQcAntibiotics.Any(x => x.Id == itpqca.Id))
                    {
                        iqcTestProfile.IqcTestProfileQcOrganisms.Where(x => x.Id == itpqca.IqcTestProfileQcOrganismId).First().IqcTestProfileQcAntibiotics.Add(itpqca);
                    }
                    return itp;
                },
                param: new { id });
            return iqcTestProfile;
        }
    }
}
