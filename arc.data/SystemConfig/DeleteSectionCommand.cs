using arc.common.Models.SystemConfig;
using arc.domain.Configuration.ReportsConfig;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class DeleteSectionCommand : ICommand
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var idList = args[0].Split("|");
            
            var sql = @"delete from configs Where ConfigName = @Name";
            await connect.ExecuteAsync(sql, new { Name = idList[1] });

            var reportName = idList[0];
            sql = @"select * from Configs where ConfigName = @ConfigName";
            var reportContents = await connect.QueryFirstAsync<ConfigsModel>(sql, new { configName = reportName });
            var report = JsonConvert.DeserializeObject<ReportConfig>(reportContents.Contents);
            report.DeleteSection(idList[1]);

            sql = @"update Configs set Contents = to_json(@contents::jsonb), lastmodifieddate = now() Where  ConfigName = @ConfigName";
            return await connect.ExecuteAsync(sql, new { Contents = JsonConvert.SerializeObject(report), ConfigName = reportName });
        }
    }
}
