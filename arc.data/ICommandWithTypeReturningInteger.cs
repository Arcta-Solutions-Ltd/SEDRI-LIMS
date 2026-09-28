using arc.app.Common;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data;

public interface ICommandWithTypeReturningInteger<T>
{
    Task<int> ExecuteAsync(NpgsqlConnection connect, T command, ILogWriter logWriter = null);
}



