using arc.app.Common;
using arc.domain.Configuration.ReportsConfig;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class EditSectionCommand : ICommandWithTypeReturningInteger<ReportSectionConfig>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ReportSectionConfig command, ILogWriter logWriter)
        {

            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };

            //update report section
            var contents = JsonConvert.SerializeObject(command, settings);
            var sql = @"update Configs set Contents = to_json(@contents::jsonb), lastmodifieddate = now() Where  ConfigName = @Name";
            await connect.ExecuteAsync(sql, new { Contents = contents, Name = command.Name });

            return 0;
        }
    }
}
