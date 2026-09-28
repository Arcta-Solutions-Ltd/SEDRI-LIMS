using arc.common.Models.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Graph
{
    public interface ITestNameInterpreter
    {
        Task<List<GraphModel>> InterpretAsync(List<GraphModel> rows);
    }
}
