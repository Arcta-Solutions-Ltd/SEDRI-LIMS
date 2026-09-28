using arc.app.Common;

namespace arc.app.Config.Queries.Admission
{
    /// <summary>
    /// Resolves the query definitions for the Admission and Request tables.
    /// </summary>
    internal class AdmissionQueryFactory : IDefinitionFactory
    {
        /// <summary>
        /// Creates the query definition matching the supplied name.
        /// </summary>
        /// <param name="definitionName">Name of the query definition to create.</param>
        /// <returns>The matching definition, or null when this factory does not own the name.</returns>
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "admissionsforpatient" => new AdmissionsForPatientQuery(),
                "admissionforadmissionview" => new AdmissionForAdmissionViewQuery(),
                "doesadmissioncontainspecimenscheckquery" => new DoesAdmissionContainSpecimensCheckQuery(),
                "doesadmissioncontainrequestscheckquery" => new DoesAdmissionContainRequestsCheckQuery(),
                "doesrequestcontainspecimenscheckquery" => new DoesRequestContainSpecimensCheckQuery(),
                "editadmissionquery" => new EditAdmissionQuery(),
                "editrequestquery" => new EditRequestQuery(),
                "requestforrequestview" => new RequestForRequestViewQuery(),
                "requestsforadmission" => new RequestsForAdmissionQuery(),
                "requestsforpatient" => new RequestsForPatientQuery(),
                "manageadmissionattachmentsforminitialquery" => new ManageAdmissionAttachmentsFormInitialQuery(),
                "managerequestattachmentsforminitialquery" => new ManageRequestAttachmentsFormInitialQuery(),
                "admissionattachmentsforadmissionview" => new AdmissionAttachmentsForAdmissionViewQuery(),
                "requestattachmentsforrequestview" => new RequestAttachmentsForRequestViewQuery(),
                _ => null,
            };
        }
    }
}
