using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SingleSpecimenForSpecimenLabelQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SingleSpecimenForSpecimenLabel',
                        'TableName': 'Specimen',
                        'Type': 'Single',
                        'ResultMapping': 'specimenlabelfieldsmapper',
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int'},
                            { 'Name': 'AdmissionDate', 'Type': 'date' },
                            { 'Name': 'ClinicalContactNo', 'Type': 'string' },
                            { 'Name': 'AccessionNumber', 'Type': 'string'},
                            { 'Name': 'ExistingBarcode', 'Type': 'string'},
                            { 'Name': 'BottleOnlyWeight', 'Type': 'numeric'},
                            { 'Name': 'BloodAndBottleWeight', 'Type': 'numeric'},
                            { 'Name': 'CollectionDate', 'Type': 'date'},
                            { 'Name': 'ReceivedDate', 'Type': 'date'},
                            { 'Name': 'StateId', 'Type': 'int'}
                        ],
                        'Joins': [
                            { 'Table': 'Patient', 'Fields': [{'Name': 'FirstName'},{'Name': 'Surname'},{'Name': 'PatientRef'}] },
                            { 'Table': 'SpecimenAlert', 'Type': 'Left', 'On': 'Id', 'From':'SpecimenId', 'Fields': [{'Name': 'AlertId'},{ 'Name': 'AlertTypeId'}] },
                            { 'Table': 'Organisation', 'Fields': [{'Name': 'OrganisationName'}] }
                        ],
                        'ListItems': 'PatientLocation, SpecimenType, SpecimenSite, ReceivedCondition, Diagnosis, State',
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ],
                        'Tags': 'SP'
                    }";
        }
    }
}
