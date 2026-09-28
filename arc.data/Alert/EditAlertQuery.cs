using arc.domain.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Alert
{
    internal class EditAlertQuery : IQueryReturningType<AlertDetails>
    {
        public async Task<AlertDetails> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("id");

            var sql = @"select al.*, od.name as Order, f.name as Family, l.value as OrganismGroup from alert al
                        left outer join ordercat od on od.id = al.orderid
                        left outer join family f on f.id = al.familyid
                        left outer join listitem l on l.id = al.orggroupcodingid
                        where al.Id = @Id";

            var alert = await connect.QueryFirstAsync<AlertDetails>(sql, new { Id = id });

            sql = @"select * from alertlines where alertid = @AlertId";
            var alertLines = await connect.QueryAsync<SusceptibilityGrid>(sql, new { AlertId = id });
            alert.SusceptibilityGrid = alertLines.ToList();

            sql = @"select * from alerttestlines where alertid = @AlertId";
            var alertTestLines = await connect.QueryAsync<TestGrid>(sql, new { AlertId = id });
            alert.TestGrid = alertTestLines.ToList();

            return alert;
        }
    }
}
