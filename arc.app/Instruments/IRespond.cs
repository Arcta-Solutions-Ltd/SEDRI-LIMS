using arc.common.Models;
using arc.common.Models.Instruments;
using System.Threading.Tasks;

namespace arc.app.Instruments
{
    /// <summary>
    /// Processes an inbound <see cref="ResponseModel"/> for an instrument profile.
    /// </summary>
    public interface IRespond
    {
        /// <summary>
        /// Runs the processor. <paramref name="token"/> is required for flows that invoke <see cref="IRunEventHandler"/> (e.g. direct test save); it may be null for AST-only paths.
        /// </summary>
        Task<bool> Run(ResponseModel response, TokenInfoModel token);
    }
}
