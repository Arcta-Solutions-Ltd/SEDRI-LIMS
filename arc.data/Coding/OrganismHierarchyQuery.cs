using arc.common.Models.Coding;
using arc.data.Utils;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class OrganismHierarchyQuery : IQueryReturningType<List<int>>
    {
        public async Task<List<int>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();
            var returnValue = new List<int> { int.Parse(id.Value) };

            var sql = @"select * from organism where Id = @Id";

            var organismRecord = await connect.QueryFirstAsync<Organism>(sql, new { Id = int.Parse(id.Value) });

            bool isBottomOfHierarchy = organismRecord.SerotypeId != 0 || organismRecord.SubSpeciesId != 0;
            bool useSpecies = organismRecord.SpeciesId != 0 && !isBottomOfHierarchy;
            bool useGenus = !useSpecies && !isBottomOfHierarchy;
            
            if (useSpecies)
            {
                sql = @"select* from organism where SpeciesId = @SpeciesId";

                var speciesRecord = await connect.QueryAsync<Organism>(sql, new { SpeciesId = organismRecord.SpeciesId });
                foreach( var species in speciesRecord.ToList() )
                {
                    returnValue.Add(species.Id);
                }
            }

            if (useGenus)
            {
                sql = @"select * from organism where GenusId = @GenusId";

                var genusRecord = await connect.QueryAsync<Organism>(sql, new { GenusId = organismRecord.GenusId });
                foreach (var species in genusRecord.ToList())
                {
                    returnValue.Add(species.Id);
                }
            }

            return returnValue.ToList();
        }
    }
}

