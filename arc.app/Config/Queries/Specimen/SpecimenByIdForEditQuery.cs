using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpecimenByIdForEditQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SpecimenByIdForEdit', 
                        'TableName': 'Specimen', 
                        'Type': 'Single', 
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int' }, 
                            { 'Name': 'PatientLocationId', 'Type': 'string' }, 
                            { 'Name': 'OrganisationId', 'Type': 'int' }, 
                            { 'Name': 'AdmissionDate', 'Type': 'date' },
                            { 'Name': 'DiagnosisId', 'Type': 'int' }, 
                            { 'Name': 'ClinicalContactNo', 'Type': 'string' },
                            { 'Name': 'AntibioticsInLast24hrsId', 'Type': 'int' },
                            { 'Name': 'TempInLast24hrsId', 'Type': 'int' },
                            { 'Name': 'AdditionalClinicalInformation', 'Type': 'string' },
                            { 'Name': 'SpecimenTypeId', 'Type': 'int' }, 
                            { 'Name': 'SpecimenSiteId', 'Type': 'int' }, 
                            { 'Name': 'AccessionNumber', 'Type': 'string'}, 
                            { 'Name': 'ExistingBarcode', 'Type': 'string'},
                            { 'Name': 'FurtherInformation', 'Type': 'string' },
                            { 'Name': 'BottleOnlyWeight', 'Type': 'numeric'},
                            { 'Name': 'BloodAndBottleWeight', 'Type': 'numeric'},
                            { 'Name': 'ReceivedConditionId', 'Type': 'int' }, 
                            { 'Name': 'SpecimenAppearanceId', 'Type': 'int' }, 
                            { 'Name': 'CollectionDate', 'Type': 'date'}, 
                            { 'Name': 'ReceivedDate', 'Type': 'date'},
                            { 'Name': 'CollectionTime', 'Type': 'string'}, 
                            { 'Name': 'ReceivedTime', 'Type': 'string'},
                            { 'Name': 'TestCategoryId', 'Type': 'string' },
                            { 'Name': 'CultureTypeCategoryId', 'Type': 'string' },
                            { 'Name': 'AgeYears', 'Type': 'int' },
                            { 'Name': 'AgeMonths', 'Type': 'int' },
                            { 'Name': 'AgeDays', 'Type': 'int' },
                            { 'Name': 'AgeHours', 'Type': 'int' }
                        ],
                        'Joins': [
                            { 'Table': 'Patient', 'Fields': [{'Name': 'PatientRef'}] }
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ],
                        'Tags': 'SP'
                    }";
        }
    }
}
