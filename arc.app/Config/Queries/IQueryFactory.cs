using arc.domain.Configuration.QueryConfig;

namespace arc.app.Config.Queries
{
    public interface IQueryFactory
    {
        QueryConfig GetQuery(string queryName);
    }
}
