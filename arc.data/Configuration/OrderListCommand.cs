using arc.app.Common;
using arc.common.Models.Config;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    internal class OrderListCommand : ICommandWithTypeReturningInteger<List<FieldListModel>>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, List<FieldListModel> listOfFields, ILogWriter logWriter)
        {
            var orderNumber = 1;
            foreach(var field in listOfFields)
            {
                var sql = "update listitem set displayorder = @order where id = @id";
                await connect.ExecuteAsync(sql, new { Order = orderNumber, Id = int.Parse(field.Value) });

                orderNumber++;
            }

            return 0;
        }
    }
}
