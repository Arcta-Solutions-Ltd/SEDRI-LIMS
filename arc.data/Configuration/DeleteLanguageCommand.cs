using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    internal class DeleteLanguageCommand : ICommand
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var sql = @"delete from ListItem Where Id = @TranslationId";

            await connect.ExecuteAsync(sql, new { Translationid = int.Parse(args[0]) });

            sql = @"delete from Language Where TranslationId = @TranslationId";

            return await connect.ExecuteAsync(sql, new { Translationid = int.Parse(args[0]) });
        }
    }
}
