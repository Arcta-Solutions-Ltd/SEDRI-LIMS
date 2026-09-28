using arc.common.Models.Specimen;
using arc.data.Configuration;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace arc.data.Specimen;

internal class GrowthForAllCulturesInSpecimenCommand
{
    private readonly IOptionsMonitor<DataOptions> _options;

    public GrowthForAllCulturesInSpecimenCommand(IOptionsMonitor<DataOptions> options)
    {
        _options = options;
    }

    public async Task ExecuteAsync(List<CultureListModel> cultureList, int specimenQuantityId, int specimenId, int newStateId)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);

        var sql = "";
        foreach (var culture in cultureList)
        {
            sql = "Update Culture set SpecimenQuantityId = @specimenQuantityId Where Id = @id";
            await connect.ExecuteAsync(sql, new { id = int.Parse(culture.Id), specimenQuantityId });
        }

        sql = @"update specimen set StateId = @StateId, lastmodifieddate = now() where id = @id";
        await connect.ExecuteAsync(sql, new { StateId = newStateId, id = specimenId });

        scope.Complete();
    }
}
