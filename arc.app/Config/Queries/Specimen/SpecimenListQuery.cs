using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// This class represents the definition of the specimen list query.
    /// </summary>
    internal class SpecimenListQuery : IDefinition
    {
        /// <summary>
        /// Retrieves the query definition for the specimen list.
        /// </summary>
        /// <returns>A string containing the query definition in JSON format.</returns>
        public string Get()
        {
            return @"{
                    'Query': 'SpecimenList',
                    'TableName': 'Specimen',
                    'Type': 'Select',
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
                        { 'Table': 'SpecimenTag', 'On': 'Id', 'From': 'SpecimenId', 'ConditionOn': 'TagId', 'CompareField': 'ListItemId' }
                    ],
                    'ListItems': 'PatientLocation, SpecimenType, SpecimenSite, ReceivedCondition, Diagnosis, State',
                    'Where': [
                        { 'Field': 'SpecimenTypeId', 'Comparison': 'oneof' },
                        { 'Field': 'SpecimenSiteId', 'Comparison': 'oneof' },
                        { 'Field': 'StateId', 'Comparison': 'oneof' },
                        { 'Field': 'ReceivedDate', 'Comparison': 'dayselapsed' },
                        { 'Field': 'AccessionNumber', 'Comparison': 'contains', 'orGroup': 'search' },
                        { 'Field': 'Barcode', 'Comparison': 'equals', 'orGroup': 'search' },
                        { 'Field': 'ExistingBarcode', 'Comparison': 'equals', 'orGroup': 'search' },
                        { 'Field': 'FirstName', 'Comparison': 'contains', 'orGroup': 'search' },
                        { 'Field': 'Surname', 'Comparison': 'contains', 'orGroup': 'search' },
                        { 'Field': 'PatientRef', 'Comparison': 'contains', 'orGroup': 'search' },
                        { 'Field': 'SpecimenType', 'Comparison': 'contains', 'orGroup': 'search' },
                        { 'Field': 'State', 'Comparison': 'contains', 'orGroup': 'search' }
                    ],
                    'filters': [ { 'field': 'StateId', 'comparison': '!=', 'values': [ '534', '537', '528' ] } ],
                    'Orderby': 'lastmodifieddate',
                    'Descending': true,
                    'Tags': 'SP'
                }";
        }
    }

}
