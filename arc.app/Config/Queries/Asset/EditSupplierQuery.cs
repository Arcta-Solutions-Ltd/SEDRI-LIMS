using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents the query configuration for editing supplier details.
/// </summary>
internal class EditSupplierQuery : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Edit Supplier" query.
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the query to edit supplier details.
    /// </returns>
    public string Get()
    {
        return @"{  
                'Query': 'editsupplierquery', 'TableName': 'Supplier', 'Type': 'Single', 'OrderBy' : 'Name',
                'Fields': [
                    {'Name': 'Id', 'Type': 'int'},
                    {'Name': 'Name', 'Type': 'string'},
                    {'Name': 'Code', 'Type': 'string'},
                    {'Name': 'AddressLine1', 'Type': 'string'},
                    {'Name': 'AddressLine2', 'Type': 'string'},
                    {'Name': 'Zipcode', 'Type': 'string'},
                    {'Name': 'LocationId', 'Type': 'int'},
                    {'Name': 'SupplierStatusId', 'Type': 'int'}
                ],
                'Where' : [
                    {'Field': 'Id', 'Comparison': '=' } 
                ]
            }";
    }
}

