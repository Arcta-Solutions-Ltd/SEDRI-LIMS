using arc.app.Config;
using arc.app.Config.Queries;
using arc.app.Reports;
using arc.app.Specimen;
using arc.common.Models;
using arc.domain.Reports;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public  class SpecimenRecordHandler : ISpecimenRecordHandler
    {
        private readonly ISpecimenRepository _specimenRepository;
        private readonly IReplaceListItemValues _listReplacer;
        private readonly ISpecimenRecordUtils _specimenRecordUtils;

        public SpecimenRecordHandler(ISpecimenRepository specimenRepository, IReplaceListItemValues listReplacer, ISpecimenRecordUtils specimenRecordUtils)
        {
            _specimenRepository = specimenRepository;
            _listReplacer = listReplacer;
            _specimenRecordUtils = specimenRecordUtils;
        }

        public async Task<ReportData> HandleAsync(string specimenId, TokenInfoModel token)
        {
            var returnValue = new ReportData();

            var specimenPatient = await _specimenRepository.GetSingleAsync(int.Parse(specimenId));

            // Get Patient details

            returnValue.Standard = await _specimenRecordUtils.GetPatientDetailsAsync(specimenPatient.PatientId, token);
            returnValue.Tables = [];

            // Get Specimen Details

            var specimenValues = await _specimenRecordUtils.GetSpecimenDetailsAsync(specimenId);
            returnValue.Standard.AddRange(specimenValues);

            // Get specimen submitter & approver

            var approverValues = await _specimenRecordUtils.GetApproverDetailsAsync(specimenId);
            returnValue.Standard.AddRange(approverValues);

            // Get Alerts

            var alertValues = await _specimenRecordUtils.GetAlertDetailsAsync(specimenId);
            returnValue.Tables.AddRange(alertValues);

            // Get the direct tests

            returnValue = await _specimenRecordUtils.GetDirectTestsAsync(specimenId, returnValue, token);

            // Get the organism data

            var groups = await _specimenRecordUtils.GetOrganismDataAsync(specimenId, token);
            returnValue.Groups = [groups];

            returnValue.Standard = await _listReplacer.ReplaceInStandardListAsync(returnValue.Standard, _specimenRecordUtils.GetListsUsedInReport());

            return returnValue;
        }

        public async Task<ReportData> FilteredHandlerAsync(string specimenId, TokenInfoModel token, bool containsCultureFields, bool containsPatientFields, bool containsApprovalFields, bool containsDirectTests, bool containsCultureTests)
        {
            var returnValue = new ReportData { Standard = [], Tables = [] };

            // Get Patient details

            if (containsPatientFields)
            {
                var specimenPatient = await _specimenRepository.GetSingleAsync(int.Parse(specimenId));
                returnValue.Standard = await _specimenRecordUtils.GetPatientDetailsAsync(specimenPatient.PatientId, token);
            }

            // Get Specimen Details

            var specimenValues = await _specimenRecordUtils.GetSpecimenDetailsAsync(specimenId);
            returnValue.Standard.AddRange(specimenValues);

            // Get specimen submitter & approver

            if (containsApprovalFields)
            {
                var approverValues = await _specimenRecordUtils.GetApproverDetailsAsync(specimenId);
                returnValue.Standard.AddRange(approverValues);
            }

            // Get Alerts

            //var alertValues = await _specimenRecordUtils.GetAlertDetailsAsync(specimenId);
            //returnValue.Tables.AddRange(alertValues);

            // Get the direct tests

            if (containsDirectTests)
            {
                returnValue = await _specimenRecordUtils.GetDirectTestsAsync(specimenId, returnValue, token);
            }

            // Get the organism data

            if (containsCultureFields)
            {
                var groups = await _specimenRecordUtils.GetOrganismDataAsync(specimenId, token);
                returnValue.Groups = [groups];
            }

            returnValue.Standard = await _listReplacer.ReplaceInStandardListAsync(returnValue.Standard, _specimenRecordUtils.GetListsUsedInReport());

            return returnValue;
        }
    }
}
