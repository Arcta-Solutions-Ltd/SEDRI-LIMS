using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Utils
{
    internal class GetOrganismIdFromHierarchy
    {
        private NpgsqlConnection _connect;

        internal GetOrganismIdFromHierarchy(NpgsqlConnection connect)
        {
            _connect = connect;
        }

        internal async Task<int> Get(int genusId, int speciesId, int subspeciesId, int serotypeId, int additionalId)
        {
            string sql;
            var result = 0;
            if (additionalId > 0)
            {
                sql = "select id from Organism where additionalId = @AdditionalId";
            }
            else
            {
                sql = "select id from Organism where genusId = @GenusId";
                sql += speciesId > 0 ? " and speciesId = @SpeciesId" : " and speciesid is null";
                sql += subspeciesId > 0 ? " and subspeciesId = @SubSpeciesId" : " and subspeciesid is null";
                sql += serotypeId > 0 ? " and serotypeId = @SerotypeId" : " and serotypeid is null";
            }

            if (additionalId > 0 || genusId > 0)
            {
                result = await _connect.QueryFirstAsync<int>(sql, new { GenusId = genusId, SpeciesId = speciesId, SubSpeciesId = subspeciesId, SerotypeId = serotypeId, AdditionalId = additionalId });
            }

            return result;
        }
    }
}
