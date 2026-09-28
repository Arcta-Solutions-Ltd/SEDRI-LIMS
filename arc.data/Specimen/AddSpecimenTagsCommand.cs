using arc.app.Common;
using arc.common.Models.Specimen;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen
{
    /// <summary>
    /// Adds tags to a specimen, keeping the tags already present and skipping any that are already applied.
    /// </summary>
    internal class AddSpecimenTagsCommand : ICommandWithTypeReturningInteger<SetSpecimenTagsModel>
    {
        /// <summary>
        /// Runs the command against the supplied connection.
        /// </summary>
        /// <param name="connect">An open connection to the database.</param>
        /// <param name="command">Specimen id and the tags to add.</param>
        /// <param name="logWriter">Optional log writer.</param>
        /// <returns>The id of the last tag row inserted, or zero when nothing was added.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, SetSpecimenTagsModel command, ILogWriter logWriter = null)
        {
            var idsToAdd = command.ListItemIds?.Where(i => i > 0).Distinct().ToArray() ?? [];
            if (idsToAdd.Length == 0)
            {
                return 0;
            }

            var existingIds = (await connect.QueryAsync<int>(
                "select ListItemId from SpecimenTag where SpecimenId = @SpecimenId",
                new { command.SpecimenId })).ToHashSet();

            var lastId = 0;
            foreach (var listItemId in idsToAdd.Where(id => !existingIds.Contains(id)))
            {
                var result = await connect.QueryFirstOrDefaultAsync<dynamic>(
                    "insert into SpecimenTag(SpecimenId, ListItemId, LastModifiedDate) values(@SpecimenId, @ListItemId, now()) returning id",
                    new { command.SpecimenId, ListItemId = listItemId });

                if (result != null)
                {
                    lastId = (int)result.id;
                }
            }

            logWriter?.LogInfo($"Added tags to specimen {command.SpecimenId}", "AddSpecimenTagsCommand", "ExecuteAsync");
            return lastId;
        }
    }
}
