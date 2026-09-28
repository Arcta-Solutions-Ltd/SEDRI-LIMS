using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Configuration for the "Single Specimen For Specimen List" query.
/// </summary>
internal class SingleSpecimenForSpecimenListQuery : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Single Specimen For Specimen List" query.
    /// </summary>
    /// <remarks>
    /// Field and join set is aligned with <see cref="SpecimenListQuery"/> so list row refresh
    /// returns the same shape as the list query (plus Organisation join for record view consumers).
    /// </remarks>
    /// <returns>
    /// A string representation of the query configuration, including its metadata and filtering rules.
    /// </returns>
    public string Get()
    {
        return @"{
                        'Query': 'SingleSpecimenForSpecimenList',
                        'TableName': 'Specimen',
                        'Type': 'Single',
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int' },
                            { 'Name': 'StateId', 'Type': 'int' },
                            { 'Name': 'SpecimenTypeId', 'Type': 'int' },
                            { 'Name': 'LaboratoryId', 'Type': 'int' },
                            { 'Name': 'AdmissionDate', 'Type': 'date' },
                            { 'Name': 'ClinicalContactNo', 'Type': 'string' },
                            { 'Name': 'AccessionNumber', 'Type': 'string' },
                            { 'Name': 'Barcode', 'Type': 'string' },
                            { 'Name': 'ExistingBarcode', 'Type': 'string' },
                            { 'Name': 'BottleOnlyWeight', 'Type': 'numeric' },
                            { 'Name': 'BloodAndBottleWeight', 'Type': 'numeric' },
                            { 'Name': 'CollectionDate', 'Type': 'date' },
                            { 'Name': 'ReceivedDate', 'Type': 'date' },
                            { 'Name': 'ReceivedTime', 'Type': 'string' },
                            { 'Name': 'AlertTypeId', 'Type': 'int' },
                            { 'Name': 'LastModifiedDate', 'Type': 'datetime', 'KnownAs': 'lastmodifieddate' },
                            { 'Name': 'TestCategoryId', 'Type': 'string' },
                            { 'Name': 'CultureTypeCategoryId', 'Type': 'string' },
                            { 'Name': 'Tags', 'Type': 'specimentags', 'KnownAs': 'tags' }
                        ],
                        'Joins': [
                            { 'Table': 'Patient', 'Fields': [ { 'Name': 'FirstName' }, { 'Name': 'Surname' }, { 'Name': 'PatientRef' } ] },
                            { 'Table': 'AlertType', 'Type': 'Left', 'On': 'AlertTypeId', 'From': 'Id', 'Fields': [ { 'Name': 'Colour' }, { 'Name': 'AlertCategoryId' } ] },
                            { 'Table': 'SpecimenTag', 'On': 'Id', 'From': 'SpecimenId', 'ConditionOn': 'TagId', 'CompareField': 'ListItemId' },
                            { 'Table': 'Organisation', 'Fields': [ { 'Name': 'OrganisationName' } ] }
                        ],
                        'ListItems': 'PatientLocation, SpecimenType, SpecimenSite, ReceivedCondition, Diagnosis, State',
                        'Where': [
                            {'Field': 'Id', 'Comparison': '=' }
                        ],
                        'Tags': 'SP'
                    }";
    }
}
