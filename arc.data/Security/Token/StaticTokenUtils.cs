using arc.common.ExtensionMethods;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Security.Token;

/// <summary>
/// Utility class for retrieving a language ID based on laboratory or organisation identifiers.
/// </summary>
internal class StaticTokenUtils
{
    /// <summary>
    /// Retrieves the language ID associated with a given laboratory or organisation.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="laboratoryId">The ID of the laboratory.</param>
    /// <param name="organisationId">The ID of the organisation.</param>
    /// <returns>The language ID retrieved from the database, or a default value if none is found.</returns>
    public static async Task<string> GetLanguageIdFormLabIdOrOrganisationIdAsync(NpgsqlConnection connect, string laboratoryId, string organisationId)
    {
        if (laboratoryId.IsIntegerGreaterThan(0) )
        {
            var sql = "select languageId from Laboratory where Id = @Id";
            var param = new { Id = int.Parse(laboratoryId) };
            var language = await connect.QueryAsync<string>(sql, param);
            return language.First();
        }
        else if (organisationId.IsIntegerGreaterThan(0))
        {
            var sql = "select languageId from Organisation where Id = @Id";
            var param = new { Id = int.Parse(organisationId) };
            var language = await connect.QueryAsync<string>(sql, param);
            return language.First();
        }
        else
        {
            return "669";
        }
    }
}
