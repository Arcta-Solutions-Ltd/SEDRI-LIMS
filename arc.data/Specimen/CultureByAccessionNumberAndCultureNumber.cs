using arc.data.model.Culture;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen;
/// <summary>
/// This class defines the query to retrieve culture data by accession number and culture number.
/// Implements the IQueryReturningType interface for CultureDataModel.
/// </summary>
internal class CultureByAccessionNumberAndCultureNumber : IQueryReturningType<CultureDataModel>
{
    /// <summary>
    /// Executes the query to retrieve culture data by accession number and culture number.
    /// </summary>
    /// <param name="connect">The NpgsqlConnection object for database connection.</param>
    /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
    /// <returns>A task that represents the asynchronous operation. 
    /// The task result contains the CultureDataModel object.</returns>
    public async Task<CultureDataModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var accessionNumber = queryFilters.GetStringValue("accessionnumber");
        var cultureNumber = queryFilters.GetStringValue("culturenumber");

        var sql = $"select c.* from Culture c inner join specimen s on s.Id = c.specimenId where s.AccessionNumber = @AccessionNumber and c.CultureNumber = @CultureNumber";

        var result = await connect.QueryFirstOrDefaultAsync<CultureDataModel>(sql, new { AccessionNumber = accessionNumber, CultureNumber = int.Parse(cultureNumber) });

        return result;
    }
}

