using System.Threading.Tasks;

namespace arc.common
{
    public interface ICommandHandler<T>
    {
        Task<int> HandleAsync(T value);
    }
}
