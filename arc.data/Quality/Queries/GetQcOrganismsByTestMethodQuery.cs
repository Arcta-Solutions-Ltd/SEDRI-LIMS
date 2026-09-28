using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Quality;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Quality.Queries
{
    internal class GetQcOrganismsByTestMethodQuery : IQueryReturningType<List<QcOrganism>>
    {
        public async Task<List<QcOrganism>> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
        {
            var testMethod = queryFilters.GetStringValue("testmethod");

            var lookup = new Dictionary<int, QcOrganism>();

            var methodWhereClause = "";

            methodWhereClause = testMethod.ToLower() switch
            {
                "mic" => "WHERE micrangelower IS NOT NULL AND micrangeupper IS NOT NULL",
                "disk" => "WHERE inhibitionzonediameterrangelower IS NOT NULL AND inhibitionzonediameterrangeupper IS NOT NULL",
                _ => throw new Exception($"{nameof(testMethod)} parameters must be one of the following: {string.Join(", ", IqcTestProfile.TestMethods())}."),
            };
            await connection.QueryAsync<QcOrganism, QcAntibiotic, QcOrganism>($@"
                SELECT o.*, a.*
                FROM qcorganisms o
                INNER JOIN qcantibiotics a ON o.Id = a.QcOrganismId
                {methodWhereClause}
                ", (o, a) =>
            {
                if (!lookup.TryGetValue(o.Id, out var qcOrganism))
                    lookup.Add(o.Id, qcOrganism = o);
                if (a != null && !qcOrganism.QcAntibiotics.Any(x => x.Id == a.Id))
                    qcOrganism.QcAntibiotics.Add(a);
                return qcOrganism;
            }, new { methodWhereClause });

            return lookup.Values.ToList();
        }
    }
}
