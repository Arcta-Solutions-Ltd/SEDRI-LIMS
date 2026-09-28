namespace arc.app.Common;

/// <summary>
/// Defines a factory interface for creating query execution objects based on a query name.
/// </summary>
/// <param name="queryName">The name of the query to retrieve a corresponding <see cref="IQueryRun"/> instance.</param>
/// <returns>An <see cref="IQueryRun"/> object configured to execute the specified query.</returns>

public interface ISpecialQueryFactory
{
    IQueryRun GetQuery(string queryName);
}
