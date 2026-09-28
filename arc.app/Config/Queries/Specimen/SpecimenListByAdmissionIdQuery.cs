using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query definition returning specimens linked to a single admission for embedded record view lists.
/// </summary>
internal class SpecimenListByAdmissionIdQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query definition.
    /// </summary>
    /// <returns>A JSON string containing the query configuration.</returns>
    public string Get()
    {
        return @"{ 
                        'Query': 'SpecimenListByAdmissionId', 
                        'TableName': 'Specimen', 
                        'Type': 'Select', 
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int'},
                            { 'Name': 'PatientId', 'Type': 'int'},
                            { 'Name': 'AdmissionId', 'Type': 'int'},
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
                            {'Field': 'AdmissionId', 'Comparison': '=' } 
                        ],
                        'Orderby': 'Id'
                    }";
    }
}
