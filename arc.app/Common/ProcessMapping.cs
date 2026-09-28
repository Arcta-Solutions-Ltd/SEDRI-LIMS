using arc.domain.Configuration.MappingsConfig;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public class ProcessMapping : IProcessMapping
    {
        private readonly ISpecialMappingFactory _specialMappingFactory;
        private readonly ILocalFieldResolver _localFieldResolver;

        public ProcessMapping(ISpecialMappingFactory specialMappingFactory, ILocalFieldResolver localFieldResolver)
        {
            _specialMappingFactory = specialMappingFactory;
            _localFieldResolver = localFieldResolver;
        }

        public async Task<string> ProcessAsync(MapperConfig config, string  message)
        {
            if (config.Type.ToLower() == "special")
            {
                var mapper = _specialMappingFactory.GetMapper(config.Name);
                return mapper.Map(message);
            } else
            {
//                message = _jsonElementRemover.RemoveElementsByValue(message, "Crafted");
                var requiredLocalFields = config.GetRequiredLocalFields();

                // Resolve local fields.
                var localFields = await _localFieldResolver.GetFieldValuesAsync(requiredLocalFields);

                // Set local fields in mapper.
                config.SetRequiredLocalFields(localFields);

                return config.Map(message);
            }
        }
    }
}
