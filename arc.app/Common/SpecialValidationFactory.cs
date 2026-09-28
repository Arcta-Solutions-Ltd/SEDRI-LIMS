using arc.app.AST;
using arc.app.Coding;
using arc.app.ExpertRule;
using arc.app.Config.Workflows;
using arc.app.Configuration;
using arc.app.Exports;
using arc.app.List;
using arc.app.Location;
using arc.app.Patient;
using arc.app.Quality;
using arc.app.Security;
using arc.app.Settings;
using arc.app.Settings.SettingProviders;
using arc.app.Specimen;
using arc.app.SystemConfig;
using arc.app.Instruments;
using arc.common.Models;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public class SpecialValidationFactory : ISpecialValidationFactory
    {
        private readonly ISpecimenRepository _specimenRepository;
        private readonly IOrganisationRepository _organisationRepository;
        private readonly ILaboratoryRepository _laboratoryRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IWorkflowAdapter _workflowAdapter;
        private readonly IConfigRepository _configRepository;
        private readonly ISettingProviderFactory _settingProviderFactory;
        private readonly ILogWriter _logWriter;
        private readonly ICultureRepository _cultureRepository;
        private readonly IAntibioticRepository _antibioticRepository;
        private readonly IGeneralRepository _generalRepository;
        private readonly IFormConfigDefinition _formConfigDefinition;
        private readonly IListRepository _listRepository;
        private readonly IExistingFieldCatalogue _existingFieldCatalogue;

        public SpecialValidationFactory(ISpecimenRepository specimenRepository, IOrganisationRepository organisationRepository, ILaboratoryRepository laboratoryRepository,
            IPatientRepository patientRepository, ILocationRepository locationRepository, ILogWriter logWriter, IWorkflowAdapter workflowAdapter, IConfigRepository configRepository,
            ISettingProviderFactory settingProviderFactory, ICultureRepository cultureRepository, IAntibioticRepository antibioticRepository, IGeneralRepository generalRepository,
            IFormConfigDefinition formConfigDefinition, IListRepository listRepository, IExistingFieldCatalogue existingFieldCatalogue)
        {
            _formConfigDefinition = formConfigDefinition;
            _listRepository = listRepository;
            _existingFieldCatalogue = existingFieldCatalogue;
            _specimenRepository = specimenRepository;
            _organisationRepository = organisationRepository;
            _laboratoryRepository = laboratoryRepository;
            _locationRepository = locationRepository;
            _patientRepository = patientRepository;
            _workflowAdapter = workflowAdapter;
            _logWriter = logWriter;
            _configRepository = configRepository;
            _settingProviderFactory = settingProviderFactory;
            _cultureRepository = cultureRepository;
            _antibioticRepository = antibioticRepository;
            _generalRepository = generalRepository;
        }

        /// <summary>
        /// Runs the synchronous custom validator registered for an event, if there is one.
        /// </summary>
        /// <param name="eventName">The event being saved.</param>
        /// <param name="message">The raw event payload.</param>
        /// <returns>An empty string when the payload is acceptable, otherwise a translation tag describing the problem.</returns>
        public string ValidateMessage(string eventName, string message)
        {
            ISpecialValidator validator;

            _logWriter.LogInfo($"Finding custom validator for event {eventName}", "SpecialValidationFactory", "ValidateMessage");
            switch (eventName.ToLower())
            {
                case "addcustomentry":
                case "editcustomentry":
                    validator = new CustomEntryValidator(message);
                    return validator.ValidateMessage();
                case "addbreakpoint":
                case "editbreakpoint":
                    validator = new BreakpointValidator(message);
                    return validator.ValidateMessage();
                case "addexportprofile":
                case "editexportprofile":
                    validator = new ExportProfileValidator(message);
                    return validator.ValidateMessage();
                case "addexportprofilefield":
                    validator = new ExportProfileFieldValidator(message);
                    return validator.ValidateMessage();
                case "addmappingevent":
                case "editmappingevent":
                    validator = new AddMappingValidator(message);
                    return validator.ValidateMessage();
                case "addtestpattern":
                case "edittestpattern":
                    validator = new TestPatternValidator(message);
                    return validator.ValidateMessage();
                case "addtableentry":
                case "edittableentry":
                    validator = new TableEntryValidator(message);
                    return validator.ValidateMessage();
                case "editfield":
                case "addfield":
                    validator = new FieldDetailsValidator(message);
                    var fieldDetailsError = validator.ValidateMessage();
                    if (!string.IsNullOrEmpty(fieldDetailsError))
                    {
                        _logWriter.LogInfo(
                            $"Field configuration validation failed for {eventName.ToLower()}: {fieldDetailsError}",
                            "SpecialValidationFactory",
                            "ValidateMessage");
                    }
                    return fieldDetailsError;
                case "newreceivedspecimen":
                    validator = new ReceivedSpecimenRequestValidator(message);
                    return validator.ValidateMessage();
                case "updateast":
                    validator = new ASTValidator(message, _logWriter, _antibioticRepository, _generalRepository, _cultureRepository);
                    return validator.ValidateMessage();
                case "addiqctest":
                    return new AddIqcTestValidator(message).ValidateMessage();
                case "editiqctestqcorganisms":
                    return new EditIqcTestQcOrganismsValidator(message).ValidateMessage();
                case "addexpertrule":
                    validator = new ExpertRuleValidator(message);
                    return validator.ValidateMessage();
                case "editexpertrule":
                    validator = new ExpertRuleValidator(message);
                    return validator.ValidateMessage();
                case "addinstrumentprofile":
                case "editinstrumentprofile":
                    var instrumentProfileError = new InstrumentProfileValidator(message).ValidateMessage();
                    if (!string.IsNullOrEmpty(instrumentProfileError))
                    {
                        _logWriter.LogInfo(
                            $"Instrument profile validation failed for {eventName.ToLower()}",
                            "SpecialValidationFactory",
                            "ValidateMessage");
                    }
                    return instrumentProfileError;
            }
            _logWriter.LogInfo("No custom validator found", "SpecialValidationFactory", "ValidateMessage");
            return "";
        }

        /// <summary>
        /// Runs the asynchronous custom validator registered for an event, if there is one. Used when
        /// validation needs to read from the database or the configuration store.
        /// </summary>
        /// <param name="eventName">The event being saved.</param>
        /// <param name="message">The raw event payload.</param>
        /// <param name="token">The caller's token, used by validators that are scoped to an organisation.</param>
        /// <returns>An empty string when the payload is acceptable, otherwise a translation tag describing the problem.</returns>
        public async Task<string> ValidateMessageAsync(string eventName, string message, TokenInfoModel token = null)
        {
            ISpecialValidatorAsync validatorAsync;
            _logWriter.LogInfo($"Find asynchronous custom validator for event {eventName}", "SpecialValidationFactory", "ValidateMessage");
            switch (eventName.ToLower())
            {
                case "ackreceipt":
                    validatorAsync = new ACKReceiptValidator(_specimenRepository, message);
                    return await validatorAsync.ValidateMessageAsync();
                case "editculture":
                    validatorAsync = new EditIsolateValidator(_cultureRepository, _logWriter, message);
                    return await validatorAsync.ValidateMessageAsync();
                case "addworkflowentry":
                    validatorAsync = new AddWorkflowEntryValidator(_workflowAdapter, message);
                    return await validatorAsync.ValidateMessageAsync();
                case "editlocation":
                    validatorAsync = new EditLocationValidator(_locationRepository, message);
                    return await validatorAsync.ValidateMessageAsync();
                case "editorganisation":
                    validatorAsync = new EditOrganisationValidator(_organisationRepository, message, token);
                    return await validatorAsync.ValidateMessageAsync();
                case "editsetting":
                    validatorAsync = new EditSettingValidator(_configRepository, _settingProviderFactory, message);
                    return await validatorAsync.ValidateMessageAsync();
                case "remotespecimen":
                    _logWriter.LogInfo("Starting remote specimen custom asynchronous validator", "SpecialValidationFactory", "ValidateMessageAsync");
                    validatorAsync = new SpecimenReqOrgAndLabValidator(_organisationRepository, _laboratoryRepository, _patientRepository, _logWriter, message, token);
                    return await validatorAsync.ValidateMessageAsync();
                case "newreceivedspecimen":
                    _logWriter.LogInfo("Starting received specimen custom asynchronous validator", "SpecialValidationFactory", "ValidateMessageAsync");
                    validatorAsync = new SpecimenReqOrgAndLabValidator(_organisationRepository, _laboratoryRepository, _patientRepository, _logWriter, message, token);
                    return await validatorAsync.ValidateMessageAsync();
                case "editpagerules":
                    _logWriter.LogInfo("Starting page rules custom asynchronous validator", "SpecialValidationFactory", "ValidateMessageAsync");
                    validatorAsync = new PageRulesValidator(_formConfigDefinition, message, _logWriter);
                    return await validatorAsync.ValidateMessageAsync();
                case "editpages":
                    _logWriter.LogInfo("Starting page order custom asynchronous validator", "SpecialValidationFactory", "ValidateMessageAsync");
                    validatorAsync = new PageOrderValidator(_formConfigDefinition, message, _logWriter);
                    return await validatorAsync.ValidateMessageAsync();
                case "addfield":
                case "editfield":
                    _logWriter.LogInfo("Starting field parent link custom asynchronous validator", "SpecialValidationFactory", "ValidateMessageAsync");
                    validatorAsync = new FieldParentLinkValidator(_formConfigDefinition, _listRepository, message, _logWriter);
                    return await validatorAsync.ValidateMessageAsync();
                case "addexistingfield":
                    _logWriter.LogInfo("Starting existing field custom asynchronous validator", "SpecialValidationFactory", "ValidateMessageAsync");
                    validatorAsync = new ExistingFieldValidator(_formConfigDefinition, _existingFieldCatalogue, message, _logWriter);
                    return await validatorAsync.ValidateMessageAsync();
            }
            _logWriter.LogInfo("No asynchronous custom validator found", "SpecialValidationFactory", "ValidateMessage");
            return "";
        }
    }
}
