using arc.app.Common;
using arc.domain;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Security
{
    internal class EditUserPreferenceCommand : ICommandWithTypeReturningInteger<Preference>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, Preference command, ILogWriter logWriter)
        {
            var sql = @"update Users set MoreData = cast(@MoreData as json), LastModifiedDate = now() where Id = @Id";
            await connect.ExecuteAsync(sql, command);

            return command.Id;
        }
    }
}
