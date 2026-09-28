using arc.app.Common;
using arc.app.Config.Pages.Admission;
using arc.app.Config.Pages.Billing;
using arc.app.Config.Pages.ExpertRules;
using arc.app.Config.Pages.Images;
using arc.app.Config.Pages.Request;
using arc.app.Config.Pages.Specification;
using arc.domain.Configuration.PagesConfig;
using System.Collections.Generic;

namespace arc.app.Config.Pages;

/// <summary>
/// Factory class responsible for creating and retrieving page configurations.
/// </summary>
public class PageConfigFactory : IPageConfigFactory
{
    /// <summary>
    /// Retrieves the page configuration by name.
    /// </summary>
    /// <param name="name">The name of the page configuration to retrieve.</param>
    /// <returns>A <see cref="PageConfig"/> object corresponding to the specified name.</returns>
    public PageConfig GetPage(string name)
    {
        return new List<IDefinitionFactory>()
        {
            new AlertPageFactory(),
            new AdmissionPageFactory(),
            new AssetPageFactory(),
            new BarcodePageFactory(),
            new BillingPageFactory(),
            new CodingPageFactory(),
            new ConfigPageFactory(),
            new ExpertRulePageFactory(),
            new ExportPageFactory(),
            new ImagePageFactory(),
            new InstrumentsPageFactory(),
            new LaboratoryPageFactory(),
            new LanguagePageFactory(),
            new ListPageFactory(),
            new LocationPageFactory(),
            new MonitoringPageFactory(),
            new OrganisationPageFactory(),
            new PatientPageFactory(),
            new QualityPageFactory(),
            new ReportPageFactory(),
            new RequestPageFactory(),
            new RolePageFactory(),
            new SettingPageFactory(),
            new SpecificationPageFactory(),
            new SpecimenPageFactory(),
            new TestConfigFactory(),
            new UserPageFactory()
        }.GetConfigByName<PageConfig>(name);
    }
}
