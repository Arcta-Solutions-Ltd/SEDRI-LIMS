using arc.app.Common;
using arc.app.Reports.InclusionSelectors;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.AST
{
    internal class UpdateASTDisplayOnReportCommand : ICommandWithTypeReturningInteger<List<CultureDetailsAst>>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, List<CultureDetailsAst> command, ILogWriter logWriter = null)
        {
            foreach(var item in command)
            {
                var sql = @"update ast set DisplayOnReport = @Display where Id = @Id";
                await connect.ExecuteAsync(sql, new { item.Id, Display = item.PrintOnReport });
            }

            return 0;
        }
    }
}
