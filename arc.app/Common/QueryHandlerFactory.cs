using arc.app.Configuration;
using arc.app.Reports;

namespace arc.app.Common
{
    public class QueryHandlerFactory : IQueryHandlerFactory
    {
        private readonly IHandleQuery _queryHandler;
        private readonly IConfigHandler _configHandler;
        private readonly IReportHandler _reportHandler;

        public QueryHandlerFactory(IHandleQuery queryHandler, IConfigHandler configHandler, IReportHandler reportHandler)
        {
            _queryHandler = queryHandler;
            _configHandler = configHandler;
            _reportHandler = reportHandler;
        }

        public IQueryBase Create(string type)
        {
            return type.ToLower() switch
            {
                "config" => _configHandler,
                "report" => _reportHandler,
                _ => _queryHandler,
            };
        }
    }
}
