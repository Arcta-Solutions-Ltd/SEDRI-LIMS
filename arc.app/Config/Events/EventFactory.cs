using arc.app.Common;
using arc.domain.Configuration.EventsConfig;
using arc.app.Config.Events.Admission;
using arc.app.Config.Events.Billing;
using arc.app.Config.Events.Images;
using arc.app.Config.Events.Request;
using arc.app.Config.Events.Specification;
using System.Collections.Generic;

namespace arc.app.Config.Events;

/// <summary>
/// Factory class responsible for creating and retrieving event configurations.
/// </summary>
public class EventFactory : IEventFactory
{
    /// <summary>
    /// Retrieves the event configuration by name.
    /// </summary>
    /// <param name="name">The name of the event configuration to retrieve.</param>
    /// <returns>A <see cref="EventConfig"/> object corresponding to the specified name.</returns>
    public EventConfig GetEvent(string name)
    {
        return new List<IDefinitionFactory>()
        {
            new AlertEventFactory(),
            new AdmissionEventFactory(),
            new AssetEventFactory(),
            new ASTEventFactory(),
            new BarcodeEventFactory(),
            new BillingEventFactory(),
            new CodingEventFactory(),
            new ConfigEventFactory(),
            new ExpertRuleEventFactory(),
            new ExportEventFactory(),
            new ImageEventFactory(),
            new InstrumentsEventFactory(),
            new LaboratoryEventFactory(),
            new LanguageEventFactory(),
            new ListEventFactory(),
            new LocationEventFactory(),
            new MonitoringEventFactory(),
            new OrganisationEventFactory(),
            new PatientEventFactory(),
            new QualityEventFactory(),
            new ReportEventFactory(),
            new RequestEventFactory(),
            new RoleEventFactory(),
            new SettingEventFactory(),
            new SpecificationEventFactory(),
            new SpecimenEventFactory(),
            new TestEventFactory(),
            new UserEventFactory()
        }.GetConfigByName<EventConfig>(name);
    }
}
