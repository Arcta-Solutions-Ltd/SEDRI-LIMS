using arc.common.Models.Role;
using System;
using System.Collections.Generic;

namespace arc.common.Models.Specimen
{
    public class CreateSpecimenEventModel
    {
        public int Id { get; set; }
        public string AccessionNumber { get; set; }
        public int PatientId { get; set; }

        /// <summary>
        /// Admission the specimen belongs to. Zero asks for a new admission to be created from the admission
        /// fields in the payload; null leaves the specimen with no admission, which is how every form that
        /// predates the Neoshield request behaves.
        /// </summary>
        public int? AdmissionId { get; set; }

        /// <summary>
        /// Request the specimen belongs to. Zero asks for a new request to be created from the request fields
        /// in the payload; null leaves the specimen with no request.
        /// </summary>
        public int? RequestId { get; set; }

        /// <summary>Json blob written to Admission.MoreData when a new admission is created.</summary>
        public string AdmissionMoreData { get; set; }

        /// <summary>Json blob written to Request.MoreData when a new request is created.</summary>
        public string RequestMoreData { get; set; }

        /// <summary>Human readable reference generated for a newly created request.</summary>
        public string RequestReference { get; set; }

        public string PatientRef { get; set; }
        public string Surname { get; set; }
        public string PatientLocation { get; set; }
        public int PatientLocationId { get; set; }
        public string AdmissionDate { get; set; }
        public string Diagnosis { get; set; }
        public int DiagnosisId { get; set; }
        public string ClinicalContactNo { get; set; }
        public string SpecimenSite { get; set; }
        public int SpecimenSiteId { get; set; }
        public string SpecimenType { get; set; }
        public int SpecimenTypeId { get; set; }
        public string Action { get; set; }
        public int ActionId { get; set; }
        public string Message { get; set; }
        public string ReceivedCondition { get; set; }
        public int ReceivedConditionId { get; set; }
        public string RejectionReason { get; set; }
        public int SelectReasonId { get; set; }
        public string SpecimenAppearance { get; set; }
        public int SpecimenAppearanceId { get; set; }
        public int StateId { get; set; }
        public string Barcode { get; set; }
        public string ExistingBarcode { get; set; }
        public string CollectionDate { get; set; }
        public DateTime CollectionDateAsDate { get; set; }
        public string CollectionTime { get; set; }
        public string ReceivedDate { get; set; }
        public DateTime? ReceivedDateAsDate { get; set; }
        public string ReceivedTime { get; set; }
        public int AntibioticsInLast24hrsId { get; set; }
        public int TempInLast24hrsId { get; set; }
        public string AdditionalClinicalInformation { get; set; }
        public DateTime? AdmissionDateAsDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string FirstName { get; set; }
        public string DateOfBirth { get; set; }
        public DateTime? DateOfBirthAsDate { get; set; }
        public string Age { get; set; }
        public int? AgeYears { get; set; }
        public int? AgeMonths { get; set; }
        public int? AgeDays { get; set; }
        public int? AgeHours { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int LocationId { get; set; }
        public string ZipCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string Gender { get; set; }
        public int GenderId { get; set; }
        public string PatientBarcode { get; set; }
        public int LaboratoryId { get; set; }
        public int OrganisationId { get; set; }
        public int SpecimenWeight { get; set; }
        public decimal? BottleOnlyWeight { get; set; }
        public decimal? BloodAndBottleWeight { get; set; }
        public string FurtherInformation { get; set; }
        public string RequestCultureTests { get; set; }
        public string MelioidosisCultureTests { get; set; }
        public string ManufacturersBarcode { get; set; }
        public string View { get; set; }
        public string DefaultView { get; set; }
        public List<MenuPermissionEventModel> Crafted { get; set; }
        public string MoreData { get; set; }
    }
}
