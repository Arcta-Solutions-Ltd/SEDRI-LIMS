using Npgsql;
using System.Threading.Tasks;

namespace arc.data
{
    public interface ICommandReturningInteger
    {
        Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args);
    }
}
