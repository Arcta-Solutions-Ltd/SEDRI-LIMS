using arc.app.Common;
using arc.common.Models.Specimen;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen
{
    /// <summary>
    /// Replaces every tag on a specimen with the supplied set, deleting the existing tags first.
    /// </summary>
    internal class SetSpecimenTagsCommand : ICommandWithTypeReturningInteger<SetSpecimenTagsModel>
    {
        /// <summary>
        /// Runs the command against the supplied connection.
        /// </summary>
        /// <param name="connect">An open connection to the database.</param>
        /// <param name="command">Specimen id and the tags to apply.</param>
        /// <param name="logWriter">Optional log writer.</param>
        /// <returns>The id of the last tag row inserted, or zero when no tags were applied.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, SetSpecimenTagsModel command, ILogWriter logWriter = null)
        {
            await connect.ExecuteAsync(
                "delete from SpecimenTag where SpecimenId = @SpecimenId",
                new { command.SpecimenId });

            var ids = command.ListItemIds?.Where(i => i > 0).Distinct().ToArray() ?? [];
            var lastId = 0;

            foreach (var listItemId in ids)
            {
                var result = await connect.QueryFirstOrDefaultAsync<dynamic>(
                    "insert into SpecimenTag(SpecimenId, ListItemId, LastModifiedDate) values(@SpecimenId, @ListItemId, now()) returning id",
                    new { command.SpecimenId, ListItemId = listItemId });

                if (result != null)
                {
                    lastId = (int)result.id;
                }
            }

            logWriter?.LogInfo($"Set {ids.Length} tags on specimen {command.SpecimenId}", "SetSpecimenTagsCommand", "ExecuteAsync");
            return lastId;
        }
    }
}
