using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    public interface IExportHandler
    {
        Task<int> HandleAsync(string message, EventConfig eventData);
    }
}
