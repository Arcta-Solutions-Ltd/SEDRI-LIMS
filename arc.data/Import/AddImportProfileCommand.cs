//using arc.app.Common;
//using arc.common.Models.Export;
//using Dapper;
//using Npgsql;
//using System.Threading.Tasks;

//namespace arc.data.Import
//{
//    internal class AddImportProfileCommand : ICommandWithTypeReturningInteger<ExportProfileModel>
//    {
//        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExportProfileModel command, ILogWriter logWriter = null)
//        {
//            var sql = @"insert into importprofile(name, description, includeheaderrow, tablenameid, lastmodifieddate)
//                        values(@name, @description, @includeheaderrow, @tablenameid, now()) returning id";
//            var id = await connect.QueryFirstAsync(sql, command);
//            var importProfileId = (int)id.id;
//            return importProfileId;
//        }
//    }
//}
