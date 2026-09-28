using arc.app.Common;

namespace arc.app.Config.Queries.Asset;

/// <summary>
/// Represents a query definition for retrieving supplier list data.
/// </summary>
internal class SupplierListQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON string representing the supplier list query definition.
    /// </summary>
    /// <returns>A JSON string that defines the supplier list query.</returns>
    public string Get()
    {
        return @"{  
                'Query': 'supplierlistquery', 'TableName': 'Supplier', 'Type': 'Select', 'OrderBy' : 'Name',
                'Fields': [
                    {'Name': 'Id', 'Type': 'int'},
                    {'Name': 'Name', 'Type': 'string'},
                    {'Name': 'Code', 'Type': 'string'}
                ],
                'Joins': [
                    { 'Table': 'Location', 'Type': 'Left', 'Fields': [{'Name': 'FullyQualifiedName'}] }
                ],
                'ListItems': 'SupplierStatus',
                'Where' : [
                    {'Field': 'Name', 'Comparison': 'contains', orGroup: 'search' },
                    {'Field': 'Code', 'Comparison': 'contains', orGroup: 'search' },
                    {'Field': 'SupplierStatusId', 'Comparison': 'oneof' },
                    {'Field': 'LocationId', 'Comparison': 'in'}
                ],
                'Orderby': 'Name',
                'Descending': false
            }";
    }
}

