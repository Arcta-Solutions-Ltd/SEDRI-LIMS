using arc.app.Common;
using arc.app.Patient;
using arc.app.Security;
using arc.common.Models;
using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace arc.app.Specimen
{
    internal class SpecimenReqOrgAndLabValidator : ISpecialValidatorAsync
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly ILaboratoryRepository _laboratoryRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ILogWriter _logWriter;
        private readonly string _message;
        private readonly TokenInfoModel _token;

        public SpecimenReqOrgAndLabValidator(IOrganisationRepository organisationRepository, ILaboratoryRepository laboratoryRepository, IPatientRepository patientRepository, ILogWriter logWriter, string message, TokenInfoModel token)
        {
            _organisationRepository = organisationRepository;
            _laboratoryRepository = laboratoryRepository;
            _patientRepository = patientRepository;
            _logWriter = logWriter;
            _message = message;
            _token = token;
        }

        public async Task<string> ValidateMessageAsync()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var specimen = JsonConvert.DeserializeObject<CreateSpecimenEventModel>(_message, settings);

            var numberOfOrgs = (await _organisationRepository.GetOrganisationsForListAsync(_token)).Count();
            var numberOfLabs = (await _laboratoryRepository.GetLaboratoriesForListAsync(_token)).Count();

            //if (! _remote && numberOfOrgs == 0) { return "@SpeValC@"; }
            if (specimen.OrganisationId == 0 && numberOfOrgs > 1) { return "@SpeValC@"; }
            if (specimen.LaboratoryId == 0 && numberOfLabs > 1) { return "@SpeValA@"; }

            if (specimen.OrganisationId > 0 && !await _organisationRepository.IsOrganisationEnabledAsync(specimen.OrganisationId))
            {
                _logWriter.LogInfo($"Rejected specimen save because organisation is disabled; OrganisationId={specimen.OrganisationId}", "SpecimenReqOrgAndLabValidator", "ValidateMessageAsync");
                return "@OrgDis@";
            }

            if (specimen.PatientId == 0)
            {
                specimen.PatientRef = specimen.PatientRef.Trim().ToString();
                var queryFilters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig() { Key = "PatientRef", Value = specimen.PatientRef }}};
                var existingPatient = await _patientRepository.PatientRefDuplicateAsync(queryFilters);
                if (existingPatient > 0) { return "@PatErr@"; }
            }

            return "";
        }
    }
}
