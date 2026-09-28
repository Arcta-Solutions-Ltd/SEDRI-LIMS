using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class ResistantAntibioticQuery : IQueryReturningType<List<OptionsConfig>>
    {
        public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var susceptibilityId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "susceptibilityid").First();

            var sql = @"With tab as (
						select ast.antibioticid
						from specimen s
						inner join culture c on s.id = c.specimenid
						inner join ast on c.id = ast.cultureid and ast.susceptibilityid = @Sus
						group by ast.antibioticid)
						select tab.antibioticid as key, ant.antibioticname as text from tab
						inner join antibiotic ant on ant.id = tab.antibioticid
						order by text";

            var result = await connect.QueryAsync<OptionsConfig>(sql, new { Sus = int.Parse(susceptibilityId.Value) });

            return result.ToList();
        }
    }
}
