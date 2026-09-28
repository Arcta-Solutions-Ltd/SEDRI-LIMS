using arc.app.Common;
using arc.app.Config.UIEvents.Admission;
using arc.app.Config.UIEvents.Billing;
using arc.app.Config.UIEvents.ExpertRules;
using arc.app.Config.UIEvents.Import;
using arc.app.Config.UIEvents.Images;
using arc.app.Config.UIEvents.Request;
using arc.app.Config.UIEvents.Specification;
using arc.domain.Configuration.UIEventsConfig;
using System.Collections.Generic;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Factory class responsible for creating and retrieving UI event configurations.
/// </summary>
public class UIEventConfigFactory : IUIEventConfigFactory
{
    /// <summary>
    /// Retrieves the UI event configuration by name.
    /// </summary>
    /// <param name="name">The name of the UI event configuration to retrieve.</param>
    /// <returns>A <see cref="UIEventConfig"/> object corresponding to the specified name.</returns>
    public UIEventConfig GetEvent(string name)
    {
        return new List<IDefinitionFactory>()
        {
            new AlertUIEventFactory(),
            new AdmissionUIEventFactory(),
            new ExpertRuleUIEventFactory(),
            new AssetUIEventFactory(),
            new BarcodeUIEventFactory(),
            new BillingUIEventFactory(),
            new CodingUIEventFactory(),
            new ConfigUIEventFactory(),
            new ExportUIEventFactory(),
            new GraphUIEventFactory(),
            new ImageUIEventFactory(),
            new ImportUIEventFactory(),
            new InstrumentsUIEventFactory(),
            new LaboratoryUIEventFactory(),
            new LanguageUIEventFactory(),
            new ListUIEventFactory(),
            new LocationUIEventFactory(),
            new MonitoringUIEventFactory(),
            new OrganisationUIEventFactory(),
            new PatientUIEventFactory(),
            new QualityUIEventFactory(),
            new QualityUIEventFactory(),
            new ReportUIEventFactory(),
            new RequestUIEventFactory(),
            new RoleUIEventFactory(),
            new SpecificationUIEventFactory(),
            new SpecimenUIEventFactory(),
            new TestUIEventFactory(),
            new UserUIEventFactory(),
            new SettingUIEventFactory(),
        }.GetConfigByName<UIEventConfig>(name);
    }
}
