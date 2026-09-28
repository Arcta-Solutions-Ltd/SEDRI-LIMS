using arc.common;
using System;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface ISpecialValidatorAsync
    {
        Task<string> ValidateMessageAsync();
    }
}
