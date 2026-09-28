using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Defines a query for retrieving detailed information about a specimen and its associated patient,
/// including timestamps, weights, and identifiers, as well as joined patient name data.
/// </summary>
internal class SpecimenandPatientRecordQuery : IDefinition
{
    /// <summary>
    /// Returns the query definition in JSON format, including selected fields from the Specimen table,
    /// a join with the Patient table, a list of dropdown-related list items, and a filter on specimen ID.
    /// </summary>
    /// <returns>A JSON-formatted string representing the query definition.</returns>
    public string Get()
    {
        return @"{ 
                        'Query': 'SpecimenandPatientRecord', 
                        'TableName': 'Specimen', 
                        'Type': 'Single', 
                        'Fields': [
                            { 'Name': 'AdmissionDate', 'Type': 'date' }, 
                            { 'Name': 'ClinicalContactNo', 'Type': 'string' }, 
                            { 'Name': 'AccessionNumber', 'Type': 'string'}, 
                            { 'Name': 'ExistingBarcode', 'Type': 'string'}, 
                            { 'Name': 'BottleOnlyWeight', 'Type': 'numeric'}, 
                            { 'Name': 'BloodAndBottleWeight', 'Type': 'numeric'},
                            { 'Name': 'CollectionDate', 'Type': 'date'}, 
                            { 'Name': 'ReceivedDate', 'Type': 'date'},
                            { 'Name': 'CollectionTime', 'Type': 'string'}, 
                            { 'Name': 'ReceivedTime', 'Type': 'string'}
                        ],
                        'Joins': [
                            { 'Table': 'Patient', 'Fields': [{'Name': 'FirstName'},{ 'Name': 'Surname'}] }
                        ],
                        'ListItems': 'PatientLocation, SpecimenType, SpecimenSite, ReceivedCondition, SpecimenAppearance, Ward, Diagnosis, State',
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ]
                    }";
    }
}
