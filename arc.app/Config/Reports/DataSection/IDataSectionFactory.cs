using arc.domain.Configuration.ReportsConfig;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Factory interface to create instances of DataSectionConfig.
/// </summary>
public interface IDataSectionFactory
{
    /// <summary>
    /// Retrieves a DataSectionConfig instance by its name.
    /// </summary>
    /// <param name="sectionName">The name of the data section.</param>
    /// <returns>A DataSectionConfig instance.</returns>
    DataSectionConfig GetSection(string sectionName);
}
