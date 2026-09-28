using System;
using System.Threading.Tasks;

namespace arc.common
{
    public interface ICacheManager
    {
        public ValueTask<T> GetAsync<T>(Func<Task<T>> func, string key);
    }
}
