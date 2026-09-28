using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpecimenListByPatientIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SpecimenListByPatientId', 
                        'TableName': 'Specimen', 
                        'Type': 'Select', 
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int'},
                            { 'Name': 'PatientId', 'Type': 'int'},
                            { 'Name': 'StateId', 'Type': 'int'},
                            { 'Name': 'AdmissionDate', 'Type': 'date' },
                            { 'Name': 'ClinicalContactNo', 'Type': 'string' },
                            { 'Name': 'AccessionNumber', 'Type': 'string'}, 
                            { 'Name': 'Barcode', 'Type': 'string'},
                            { 'Name': 'ExistingBarcode', 'Type': 'string'},
                            { 'Name': 'BottleOnlyWeight', 'Type': 'numeric'},
                            { 'Name': 'BloodAndBottleWeight', 'Type': 'numeric'},
                            { 'Name': 'CollectionDate', 'Type': 'date'},
                            { 'Name': 'ReceivedDate', 'Type': 'date'}
                        ],
                        'Joins': [
                            { 'Table': 'Patient', 'Fields': [{'Name': 'FirstName'},{ 'Name': 'Surname'}] }
                        ],
                        'ListItems': 'PatientLocation, SpecimenType, SpecimenSite, ReceivedCondition, Diagnosis, State',
                        'Where' : [
                            {'Field': 'PatientId', 'Comparison': '=' } 
                        ],
                        'Orderby': 'Id'
                    }";
        }
    }
}
