using arc.domain.Configuration.QueryFiltersConfig;

namespace arc.data.Utils;
internal class ListQueryOrderByUtil
{
    /// <summary>
    /// Returns the generic orderby clause used for list view queries.
    /// </summary>
    /// <param name="queryFilters">The configuration for the query filters.</param>
    /// <param name="defaultColumn">The default column for ordering.</param>
    /// <returns></returns>
    public static string GetOrderByClause(QueryFilterConfig queryFilters, string defaultColumn)
    {
        var orderBy = queryFilters.OrderBy;
        var orderDescending = queryFilters.OrderDescending;
        orderBy = orderBy == "Default" ? "makedefault" : orderBy;

        var orderClause = (!string.IsNullOrEmpty(orderBy) && orderBy != " " && orderBy != "Default") ? orderBy : defaultColumn;
        orderClause += orderDescending == true ? " desc, " : " asc, ";
        orderClause += defaultColumn + " asc";

        return orderClause;
    }
}
