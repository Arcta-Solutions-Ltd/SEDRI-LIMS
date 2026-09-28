using arc.app.Common;
using arc.common.Models.Language;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    internal class AddLanguageCommand : ICommandWithTypeReturningInteger<LanguageModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, LanguageModel command, ILogWriter logWriter)
        {

            var sql = @"insert into ListItem(listid, value, LastModifiedDate, fixed, enabled, deleted) values(76, @Name, now(), false, true, false) returning id";

            var listItem = await connect.QueryFirstAsync(sql, command);
            command.TranslationId = listItem.id;

            sql = @"insert into Language(translationid, pack, sourceid, lastmodifieddate) 
                            values(@TranslationId, cast(@Pack As Json), @SourceId, now()) returning id";

            var language = await connect.QueryFirstAsync(sql, command);

            return language.id;
        }
    }
}
