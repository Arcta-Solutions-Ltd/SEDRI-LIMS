using arc.data.model.Coding;
using arc.data.model.Configuration;
using arc.data.model.Culture;
using arc.data.model.Export;
using arc.data.model.File;
using arc.data.model.Image;
using arc.data.model.Instruments;
using arc.data.model.Laboratory;
using arc.data.model.Lists;
using arc.data.model.Organism;
using arc.data.model.Quality;
using arc.data.model.Specimen;
using arc.data.model.Asset;
using arc.data.model.User;
using arc.data.model.Patient;
using arc.data.model.Event;
using arc.data.model.Report;
using arc.data.model.Queue;

namespace arc.data.model
{
    /// <summary>
    /// Contains utilities which help when using the data models.
    /// </summary>
    public class DataModelUtils
    {
        /// <summary>
        /// Returns and dictionary matching the data model to the table name in the database for each table.
        /// </summary>
        public static Dictionary<Type, string> DataModelList()
        {
            return new Dictionary<Type, string>
            {
                { typeof(AdmissionDataModel), "admission" },
                { typeof(AdditionalDataModel), "additional" },
                { typeof(AlertDataModel), "alert" },
                { typeof(AlertLinesDataModel), "alertlines" },
                { typeof(AlertTestLinesDataModel), "alerttestlines" },
                { typeof(AlertTypeDataModel), "alerttype" },
                { typeof(AntibioticDataModel), "antibiotic" },
                { typeof(AntibioticCodingDataModel), "antibioticcoding" },
                { typeof(AlertApprovalDataModel), "alertapproval" },
                { typeof(BreakpointApprovalDataModel), "breakpointapproval" },
                { typeof(ExpertRuleDataModel), "expertrule" },
                { typeof(ExpertRuleConditionDataModel), "expertrulecondition" },
                { typeof(ExpertRuleTestConditionDataModel), "expertruletestcondition" },
                { typeof(ExpertRuleActionDataModel), "expertruleaction" },
                { typeof(ExpertRuleApprovalDataModel), "expertruleapproval" },
                { typeof(ExpertRuleSpecimenTypeDataModel), "expertrulespecimentype" },
                { typeof(BreakpointDataModel), "breakpoint" },
                { typeof(ConfigsDataModel), "configs" },
                { typeof(CultureDataModel), "culture" },
                { typeof(ExportProfileDataModel), "exportprofile" },
                { typeof(ExportProfileMappingDataModel), "exportprofilemapping" },
                { typeof(ExportProfileRecordDataModel), "exportprofilerecord" },
                { typeof(FamilyDataModel), "family" },
                { typeof(FileAttachmentDataModel), "fileattachments" },
                { typeof(GenusDataModel), "genus" },
                { typeof(ImageDataModel), "images" },
                { typeof(InstrumentErrorsDataModel), "instrumenterrors" },
                { typeof(InstrumentResultsDataModel), "instrumentresults" },
                { typeof(IqcTestProfileQcantibioticsDataModel), "iqctestprofileqcantibiotics" },
                { typeof(IqcTestProfileQcorganismsDataModel), "iqctestprofileqcorganisms" },
                { typeof(IqcTestProfilesDataModel), "iqctestprofiles" },
                { typeof(LaboratoryDataModel), "laboratory" },
                { typeof(LaboratoryConfigsDataModel), "laboratoryconfigs" },
                { typeof(LanguageDataModel), "language" },
                { typeof(ListDataModel), "list" },
                { typeof(ListItemDataModel), "listitem" },
                { typeof(ListItemParentChildDataModel), "listitemparentchild" },
                { typeof(OrganismDataModel), "organism" },
                { typeof(OrganismCodingDataModel), "organismcoding" },
                { typeof(OrganismSynonymDataModel), "organismsynonyms" },
                { typeof(OrderCatDataModel), "ordercat" },
                { typeof(QcAntibioticsDataModel), "qcantibiotics" },
                { typeof(QcOrganismsDataModel), "qcorganisms" },
                { typeof(RoleDataModel), "role" },
                { typeof(SpeciesDataModel), "species" },
                { typeof(SpecimenDataModel), "specimen" },
                { typeof(StorageDataModel), "storage" },
                { typeof(SubspeciesDataModel), "subspecies" },
                { typeof(TestPatternDataModel), "testpattern" },
                { typeof(TestPatternLineDataModel), "testpatternline" },
                { typeof(TestsDataModel), "tests" },
                { typeof(UsersDataModel), "users" },
                { typeof(AccessionNumberDataModel), "accessionnumber" },
                { typeof(AntibioticGroupDataModel), "antibioticgroup" },
                { typeof(AstDataModel), "ast" },
                { typeof(BarcodeCounterDataModel), "barcodecounter" },
                { typeof(BillingProfileDataModel), "billingprofile" },
                { typeof(BillingRecordDataModel), "billingrecord" },
                { typeof(BillingRuleDataModel), "billingrule" },
                { typeof(ConfigsHistoryDataModel), "configshistory" },
                { typeof(ConfigTypeDataModel), "configtype" },
                { typeof(ContactDataModel), "contact" },
                { typeof(CultureAlertDataModel), "culturealert" },
                { typeof(CultureTestDataModel), "culturetests" },
                { typeof(EventDataModel), "event" },
                { typeof(ExportRunHistoryDataModel), "exportrunhistory" },
                { typeof(InventoryDataModel), "inventory" },
                { typeof(IqcResultDataModel), "iqcresults" },
                { typeof(IqcTestDataModel), "iqctests" },
                { typeof(LaboratoryUserDataModel), "laboratoryuser" },
                { typeof(LocationDataModel), "location" },
                { typeof(NameListDataModel), "namelist" },
                { typeof(OrganisationDataModel), "organisation" },
                { typeof(OrganisationUserDataModel), "organisationuser" },
                { typeof(OrganismAliasDataModel), "organismalias" },
                { typeof(PatientDataModel), "patient" },
                { typeof(PatientCommentDataModel), "patientcomment" },
                { typeof(PatientTagDataModel), "patienttag" },
                { typeof(QueueDataModel), "queue" },
                { typeof(ReportHistoryDataModel), "reporthistory" },
                { typeof(RequestDataModel), "request" },
                { typeof(ResultLineDataModel), "resultline" },
                { typeof(SerotypeDataModel), "serotype" },
                { typeof(SpecialAstRowDataModel), "specialastrow" },
                { typeof(SpecificationDataModel), "specification" },
                { typeof(SpecimenAlertDataModel), "specimenalert" },
                { typeof(SpecimenCommentDataModel), "specimencomment" },
                { typeof(SpecimenStateHistoryDataModel), "specimenstatehistory" },
                { typeof(SpecimenTagDataModel), "specimentag" },
                { typeof(SpecimenTestDataModel), "specimentest" },
                { typeof(SpecimenTypeBreakpointDataModel), "specimentypebreakpoint" },
                { typeof(SpecimenTypeTestPatternDataModel), "specimentypetestpattern" },
                { typeof(StorageContentDataModel), "storagecontents" },
                { typeof(StorageItemDataModel), "storageitems" },
                { typeof(SupplierDataModel), "supplier" },
                { typeof(SupplierContactDataModel), "suppliercontact" },
                { typeof(SupplierInventoryDataModel), "supplierinventory" },
                { typeof(TestTypeDataModel), "testtype" },
                { typeof(TopicDataModel), "topic" },
                { typeof(TopicTranslationDataModel), "topictranslation" },
                { typeof(UserRoleDataModel), "userrole" }
            };
        }
    }
}
