using arc.app.Common;
using arc.domain.Configuration.MappingsConfig;
using arc.app.Config.Mapper.Admission;
using arc.app.Config.Mapper.Specification;
using Newtonsoft.Json;
using System;

namespace arc.app.Config.Mapper;

/// <summary>
/// Factory class for retrieving mapper configurations based on the specified mapping name.
/// </summary>
public class MapperFactory : IMapperFactory
{
    /// <summary>
    /// Retrieves a mapper configuration object based on the provided mapping name.
    /// </summary>
    /// <param name="mappingName">The name of the mapping configuration to retrieve.</param>
    /// <returns>
    /// A <see cref="MapperConfig"/> object containing the configuration details, or null if no matching definition is found.
    /// </returns>
    public MapperConfig GetMapper(string mappingName)
    {
        try
        {
            var def = "";

            def = GetDefinitionFromFactory(def, new AdmissionMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new AssetMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new CodingMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new ConfigMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new ExportMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new InstrumentsMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new LaboratoryMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new LanguageMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new ListMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new LocationMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new OrganisationMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new PatientMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new QcOrganismMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new RoleMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new SpecimenMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new SpecificationMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new TagMapperFactory(), mappingName);
            def = GetDefinitionFromFactory(def, new TestsMapperFactory(), mappingName);

            var mapping = def == "" ? null : JsonConvert.DeserializeObject<MapperConfig>(def);

            mapping?.LoadMapperDefinition(def);

            return mapping;
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Retrieves the definition from a specified factory if the current definition is not already set.
    /// </summary>
    /// <param name="current">The current definition string.</param>
    /// <param name="factory">The factory to create the definition from.</param>
    /// <param name="queryName">The name of the query definition to retrieve.</param>
    /// <returns>
    /// A definition string if found; otherwise, the current definition string.
    /// </returns>
    private string GetDefinitionFromFactory(string current, IDefinitionFactory factory, string queryName)
    {
        if (current == "")
        {
            var instance = factory.Create(queryName);
            if (instance != null)
            {
                current = instance.Get();
            }
        }
        return current;
    }
}


