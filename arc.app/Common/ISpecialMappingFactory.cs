using arc.common;

namespace arc.app.Common
{
    public interface ISpecialMappingFactory
    {
        IMap GetMapper(string mappingName);
    }
}
