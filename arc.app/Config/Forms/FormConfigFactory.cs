using arc.app.Common;
using arc.domain.Configuration.FormsConfig;
using arc.app.Config.Forms.Admission;
using arc.app.Config.Forms.Billing;
using arc.app.Config.Forms.Images;
using arc.app.Config.Forms.ExpertRules;
using arc.app.Config.Forms.Request;
using arc.app.Config.Forms.Specification;
using System.Collections.Generic;

namespace arc.app.Config.Forms;

/// <summary>
/// Factory class responsible for creating and retrieving form configurations.
/// </summary>
public class FormConfigFactory : IFormConfigFactory
{
    /// <summary>
    /// Retrieves the form configuration by name.
    /// </summary>
    /// <param name="name">The name of the form configuration to retrieve.</param>
    /// <returns>A <see cref="FormConfig"/> object corresponding to the specified name.</returns>
    public FormConfig GetForm(string name)
    {
        return new List<IDefinitionFactory>()
        {
            new AlertFormFactory(),
            new AdmissionFormFactory(),
            new AssetFormFactory(),
            new BarcodeFormFactory(),
            new BillingFormFactory(),
            new CodingFormFactory(),
            new ConfigFormFactory(),
            new ExportFormFactory(),
            new ExpertRuleFormFactory(),
            new ImageFormFactory(),
            new InstrumentsFormFactory(),
            new LaboratoryFormFactory(),
            new LanguageFormFactory(),
            new ListFormFactory(),
            new LocationFormFactory(),
            new MonitoringFormFactory(),
            new OrganisationFormFactory(),
            new PatientFormFactory(),
            new QualityFormFactory(),
            new ReportFormFactory(),
            new RequestFormFactory(),
            new RoleFormFactory(),
            new SettingFormFactory(),
            new SpecificationFormFactory(),
            new SpecimenFormFactory(),
            new TestFormFactory(),
            new UserFormFactory()
        }.GetConfigByName<FormConfig>(name);
    }
}
