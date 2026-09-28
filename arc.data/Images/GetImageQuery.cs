using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.ReportsConfig;
using arc.data.model.Image;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Images;

internal class GetImageQuery : IQueryReturningType<ImageDataModel>
{
    public async Task<ImageDataModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var name = queryFilters.GetStringValue("name");

        var sql = @"Select * from Images Where Name = @name";

        return await connect.QueryFirstAsync<ImageDataModel>(sql, new { name });
    }
}
