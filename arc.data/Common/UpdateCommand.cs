using arc.common.ExtensionMethods;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Common
{
    /// <summary>
    /// Updates a record using the model and parameters passed.
    /// </summary>
    /// <typeparam name="TClass">The type of item that represents a row in the database.</typeparam>
    internal class UpdateCommand<TClass> : ICommandWithTypeAndStringParameters<TClass>
    {
        /// <summary>
        /// Executes the command to update a record.
        /// </summary>
        /// <param name="connect">Database connection</param>
        /// <param name="command">Model to use to update the record</param>
        /// <param name="args">String arguments. There need to be two for this method
        /// 1. The name of the table to be updated
        /// 2. The attribute on the model to use to select the record to update
        /// </param>
        /// <returns>The number of rows affected</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, TClass command, params string[] args)
        {
            var sql = command.GenerateUpdateStatement(args[0], args[1]);
            return await connect.ExecuteAsync(sql, command);
        }
    }
}
