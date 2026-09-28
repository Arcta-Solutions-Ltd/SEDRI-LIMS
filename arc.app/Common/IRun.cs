using arc.common;
using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface IRun
    {
        public Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null);
    }
}
