using arc.domain.Configuration.ReportsConfig;

namespace arc.app.Config.Graphs
{
    public interface IGraphConfigFactory
    {
        GraphConfig GetGraph(string name);
    }
}
