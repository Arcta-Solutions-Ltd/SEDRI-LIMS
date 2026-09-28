using arc.app.Common;
using arc.domain.Configuration.ReportsConfig;
using System.Collections.Generic;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Factory class to create instances of DataSectionConfig.
/// </summary>
public class DataSectionFactory : IDataSectionFactory
{
    /// <summary>
    /// Retrieves a DataSectionConfig instance by its name.
    /// </summary>
    /// <param name="name">The name of the data section to retrieve.</param>
    /// <returns>A DataSectionConfig instance corresponding to the specified name, or null if not found.</returns>
    public DataSectionConfig GetSection(string name)
    {
        return new List<IDefinitionFactory>()
        {
            new SpecimenDataSectionFactory()
        }.GetConfigByName<DataSectionConfig>(name);
    }
}
