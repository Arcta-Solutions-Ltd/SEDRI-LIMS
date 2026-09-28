using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents the query configuration for retrieving a single supplier from the supplier list.
/// </summary>
internal class SingleSupplierForSupplierListQuery : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Single Supplier for Supplier List" query.
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the query that retrieves a single supplier's details from the supplier list.
    /// </returns>
    public string Get()
    {
        return @"{  
                'Query': 'supplierlistquery', 'TableName': 'Supplier', 'Type': 'Single', 'OrderBy' : 'Name',
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
                    {'Field': 'Id', 'Comparison': '=' } 
                ]
            }";
    }
}

