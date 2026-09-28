using arc.common.Models.Lists;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// This class defines the query to retrieve a list by its value.
    /// Implements the IQueryReturningType interface for ListByIdQueryModel.
    /// </summary>
    internal class ListByValueQuery : IQueryReturningType<ListByIdQueryModel>
    {
        /// <summary>
        /// Executes the query to retrieve a list by its value.
        /// </summary>
        /// <param name="connect">The NpgsqlConnection object for database connection.</param>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the ListByIdQueryModel object.</returns>
        public async Task<ListByIdQueryModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var listValue = queryFilters.Parameters.Where(p => p.Key.ToLower() == "value").First();

            var sql = @"select l1.id, l1.Name, l1.description, l2.name as optionName, l1.parentid as ParentListId from list l1
                    left outer join list l2 on l2.Id = l1.ParentId
                    where LOWER(l1.Name) = LOWER(@ListValue)";

            var result = await connect.QueryAsync<ListByIdQueryModel>(sql, new { ListValue = listValue.Value });

            return result.FirstOrDefault();
        }
    }
}
