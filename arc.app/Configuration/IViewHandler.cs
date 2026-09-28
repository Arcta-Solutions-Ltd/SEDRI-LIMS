using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface IViewHandler
    {
        Task<string> GetListView();
    }
}
