using arc.common.Models.Lists;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// This class defines the query to retrieve a list by its ID.
    /// Implements the IQueryReturningType interface for ListByIdQueryModel.
    /// </summary>
    internal class ListByIdQuery : IQueryReturningType<ListByIdQueryModel>
    {
        /// <summary>
        /// Executes the query to retrieve a list by its ID.
        /// </summary>
        /// <param name="connect">The NpgsqlConnection object for database connection.</param>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the ListByIdQueryModel object.</returns>
        /// <remarks>
        /// Table Maintenance uses two different sources of parent options, and both are resolved here.
        /// OptionName is the name of another list, taken from list.parentid, and is used when a table is parented
        /// by a different table. InternalHierarchyParentOptionName is deliberately this list's own name, because a
        /// list flagged with internalhierarchy parents itself: its parent rows and its child rows are both rows of
        /// the same list, joined through listitemparentchild. When neither is set the form shows no Parent field.
        /// </remarks>
        public async Task<ListByIdQueryModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var listId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "metaflistid").First();

            var sql = @"select l1.id, l1.Name, l1.description, l2.name as optionName, l1.internalhierarchy, l1.parentid as ParentListId
                    from list l1
                    left outer join list l2 on l2.Id = l1.ParentId
                    where l1.Id = @ListId";

            var tempResult = await connect.QueryAsync<ListByIdQueryModel>(sql, new { ListId = int.Parse(listId.Value) });

            var result = tempResult.First();
            result.Enabled = "Yes";
            if (result.InternalHierarchy)
            {
                result.InternalHierarchyParentOptionName = result.Name;
            }

            return result;
        }
    }
}
