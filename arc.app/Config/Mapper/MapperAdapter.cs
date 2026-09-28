using arc.app.SystemConfig;
using arc.common.Models;
using arc.common.Models.User;
using arc.domain.Configuration.MappingsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config.Mapper
{
    public class MapperAdapter : IMapperAdapter
    {
        private readonly IMapperFactory _mapperFactory;
        private readonly IConfigRepository _configRepository;
        private readonly TokenInfoModel _tokenInfoModel;

        public MapperAdapter(IMapperFactory mapperFactory, IConfigRepository configRepository, TokenInfoModel tokenInfoModel)
        {
            _mapperFactory = mapperFactory;
            _configRepository = configRepository;
            _tokenInfoModel = tokenInfoModel;
        }

        public async Task<MapperConfig> GetMapperAsync(string mapperName)
        {
            var parameters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "ConfigName", Value = mapperName } } };
            var configRecord = await _configRepository.SingleConfigByNameAsync(parameters);

            var mapperDef = new MapperConfig(_tokenInfoModel);
            if (configRecord.Contents == null || configRecord.Contents == "{}") {
                mapperDef = _mapperFactory.GetMapper(mapperName);
                mapperDef.SetTokenInfo(_tokenInfoModel);
            } else
            { 
                mapperDef = JsonConvert.DeserializeObject<MapperConfig>(configRecord.Contents);
                mapperDef.LoadMapperDefinition(configRecord.Contents);
            }
            return mapperDef;
        }
    }
}
