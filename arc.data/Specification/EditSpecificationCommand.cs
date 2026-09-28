using arc.app.Common;
using arc.common.Models.Coding;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specification
{
    /// <summary>
    /// Command that updates an existing specification row in the database.
    /// </summary>
    internal class EditSpecificationCommand : ICommandWithTypeReturningInteger<SpecificationListModel>
    {
        /// <summary>
        /// Executes the update and sets lastmodifieddate to the current timestamp.
        /// </summary>
        /// <param name="connect">Active database connection.</param>
        /// <param name="command">The specification data to update (Id, GuidelinesId, DocumentId, VersionNumberId, PublicationYearId).</param>
        /// <param name="logWriter">Log writer for diagnostics.</param>
        /// <returns>The Id of the updated specification.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, SpecificationListModel command, ILogWriter logWriter)
        {

            var sql = @"update Specification s set guidelinesid = @GuidelinesId, documentid = @DocumentId, versionnumberid = @VersionNumberId, publicationyearid = @PublicationYearId, lastmodifieddate = now() where s.id = @Id";
            await connect.ExecuteAsync(sql, new { Id = command.Id, GuidelinesId = command.GuidelinesId, DocumentId = command.DocumentId, VersionNumberId = command.VersionNumberId, PublicationYearId = command.PublicationYearId });

            return command.Id;
        }
    }
}
