using Npgsql;
using System.Threading.Tasks;

namespace arc.data
{
    public interface ICommandWithTypeAndStringParameters<T>
    {
        Task<int> ExecuteAsync(NpgsqlConnection connect, T command, params string[] args);
    }
}
