using arc.common.Models.Lists;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// This class defines the query to retrieve list contents by its ID.
    /// Implements the IQueryReturningType interface for ListContentsModel.
    /// </summary>
    internal class ListContentsByIdQuery : IQueryReturningType<ListContentsModel>
    {
        /// <summary>
        /// Executes the query to retrieve list contents by its ID.
        /// </summary>
        /// <param name="connect">The NpgsqlConnection object for database connection.</param>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the ListContentsModel object.</returns>
        public async Task<ListContentsModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            var sql = @"select
                            li1.Id,
                            li1.value,
                            string_agg(distinct li2.value, ',') as parent,
                            Case When li1.Enabled Then 'Yes' Else 'No' end as Enabled
                        from listitem li1
                        left outer join listitemparentchild pc on pc.childid = li1.id
                        left outer join listitem li2 on pc.parentid = li2.Id
                        Where li1.Id = @Id and li1.Deleted = false
                        group by li1.Id, li1.value, li1.Enabled";

            var result = await connect.QueryAsync<ListContentsModel>(sql, new { Id = int.Parse(id.Value) });

            return result.First();
        }
    }
}
