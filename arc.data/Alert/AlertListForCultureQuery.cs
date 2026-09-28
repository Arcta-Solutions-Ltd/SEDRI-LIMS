using arc.common.Models.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Alert
{
    internal class AlertListForCultureQuery : IQueryReturningType<List<AlertMessageModel>>
    {
        public async Task<List<AlertMessageModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var cultureId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            //var sql = @"select al.alerttypeId, al.alertmessage as message, alt.colour, alt.positionId, alt.reportpositionid from culturealert ca
            //            inner join alert al on al.id = ca.alertid
            //            inner join alerttype alt on al.alerttypeid = alt.id
            //            inner join culture c on c.Id = ca.cultureId
            //            where c.Id = @CultureId";

            var sql = @"select al.alerttypeId, al.alertmessage as message, alt.colour, alt.positionId, alt.reportpositionid,
                        TRIM(CONCAT(g.name, case when g.name is not null and sp.name is null then ' spp.' else '' end,' ', sp.name, ' ', ss.name, ' ', TRIM(se.name), a.name)) as specimenorganism from culture c
                        inner join culturealert ca on ca.cultureid = c.id
                        inner join alert al on al.id = ca.alertid
                        inner join alerttype alt on al.alerttypeid = alt.id
                        left outer join organism o on o.id = c.specimenorganismid
                        left outer join genus g on g.Id = o.genusId
                        left outer join species sp on sp.Id = o.speciesId
                        left outer join subspecies ss on ss.id = o.subspeciesId
                        left outer join serotype se on se.Id = o.serotypeId
                        left outer join additional a on a.Id = o.additionalId
	                    where c.Id = @CultureId";

            var result = await connect.QueryAsync<AlertMessageModel>(sql, new { CultureId = int.Parse(cultureId.Value) });

            var alertList = result.ToList();

            foreach (var alert in alertList)
            {
                if (alert.SpecimenOrganism != "")
                {
                    alert.Message = alert.SpecimenOrganism + ": " + alert.Message;
                }
            }

            return alertList;
        }
    }
}
