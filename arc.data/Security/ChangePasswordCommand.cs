using arc.app.Common;
using arc.domain.Security.User;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Security
{
    public class ChangePasswordCommand : ICommandWithTypeReturningInteger<User>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, User command, ILogWriter logWriter)
        {
            var sql = @"update Users set Password = @Password, LastModifiedDate = now() where Id = @Id";
            await connect.ExecuteAsync(sql, new { command.Id, command.Password });

            return command.Id;
        }
    }
}
