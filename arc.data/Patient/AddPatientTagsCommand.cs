using arc.app.Common;
using arc.common.Models.Patient;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Patient
{
    /// <summary>
    /// Command to add tags to a patient, skipping any that already exist.
    /// </summary>
    internal class AddPatientTagsCommand : ICommandWithTypeReturningInteger<AddPatientTagsModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, AddPatientTagsModel command, ILogWriter logWriter)
        {
            var idsToAdd = command.ListItemIds?.Where(i => i > 0).Distinct().ToArray() ?? System.Array.Empty<int>();
            if (idsToAdd.Length == 0)
                return 0;

            var existingSql = @"select ListItemId from PatientTag where PatientId = @PatientId";
            var existingIds = (await connect.QueryAsync<int>(existingSql, new { PatientId = command.PatientId })).ToHashSet();
            var newIds = idsToAdd.Where(id => !existingIds.Contains(id)).ToArray();

            var lastId = 0;
            foreach (var listItemId in newIds)
            {
                var sql = @"insert into PatientTag(PatientId, ListItemId, LastModifiedDate) values(@PatientId, @ListItemId, now()) returning id";
                var result = await connect.QueryFirstOrDefaultAsync<dynamic>(sql, new { PatientId = command.PatientId, ListItemId = listItemId });
                if (result != null)
                    lastId = (int)result.id;
            }
            return lastId;
        }
    }
}
