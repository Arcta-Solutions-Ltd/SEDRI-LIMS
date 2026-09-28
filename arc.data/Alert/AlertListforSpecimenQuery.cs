using arc.common.Models.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Alert
{
    internal class AlertListforSpecimenQuery : IQueryReturningType<List<AlertMessageModel>>
    {
        public async Task<List<AlertMessageModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var specimenId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            //var sql = @"select al.alerttypeId, al.alertmessage as message, alt.colour, alt.positionId, alt.reportpositionid from specimenalert sa
            //            inner join alert al on al.id = sa.alertid
            //            inner join alerttype alt on al.alerttypeid = alt.id
            //            inner join specimen s on s.Id = sa.specimenId
            //            where s.Id = @SpecimenId";

            var sql = @"select al.alerttypeId, al.alertmessage as message, alt.colour, alt.positionId, alt.reportpositionid,
                        TRIM(CONCAT(g.name, case when g.name is not null and sp.name is null then ' spp.' else '' end,' ', sp.name, ' ', ss.name, ' ', TRIM(se.name), a.name)) as specimenorganism from specimen s
                        inner join specimenalert sa on sa.specimenid = s.id
                        inner join alert al on al.id = sa.alertid
                        inner join alerttype alt on al.alerttypeid = alt.id
                        left outer join culture c on c.id = sa.cultureid
                        left outer join organism o on o.id = c.specimenorganismid
                        left outer join genus g on g.Id = o.genusId
                        left outer join species sp on sp.Id = o.speciesId
                        left outer join subspecies ss on ss.id = o.subspeciesId
                        left outer join serotype se on se.Id = o.serotypeId
                        left outer join additional a on a.Id = o.additionalId
                        where s.Id = @SpecimenId";

            var result = await connect.QueryAsync<AlertMessageModel>(sql, new { SpecimenId = int.Parse(specimenId.Value) });

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
