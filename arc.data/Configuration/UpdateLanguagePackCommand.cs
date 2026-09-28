using arc.app.Common;
using arc.common.Models.Language;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    public class UpdateLanguagePackCommand : ICommandWithTypeReturningInteger<LanguageModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, LanguageModel command, ILogWriter logWriter)
        {
            var sql = @"update Language set Pack = cast(@Pack As Json) Where TranslationId = @TranslationId";

            await connect.ExecuteAsync(sql, command);

            return command.TranslationId;
        }
    }
}
