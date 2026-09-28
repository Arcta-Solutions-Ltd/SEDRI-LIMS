using arc.app.Common;

namespace arc.app.Config
{
    /// <summary>
    /// Provides the result-mapping definition used when loading existing list (table) data
    /// for the "Edit Table" initial query. Maps the database row onto the form fields that
    /// are pre-populated when the edit form opens.
    /// </summary>
    internal class EditTableQueryMapper : IDefinition
    {
        /// <summary>
        /// Returns the JSON string that defines how the <c>edittablequery</c> result row is
        /// mapped to the edit-form data model. Only <c>Id</c> and <c>Description</c> are
        /// mapped; <c>Id</c> is used internally to target the correct record on save, and
        /// <c>Description</c> pre-populates the Name field shown to the user.
        /// </summary>
        /// <returns>
        /// A JSON string containing the mapper name, type, mapping rules, and target object shape.
        /// </returns>
        public string Get()
        {
            return @"{  
                        'Name': 'edittablequerymapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Description', Value: 'Description' }
                        ],
                        'Target' : { 
                           'Id': '<:1:>',
                           'Description': '<:2:>'                          
                        }
                     }";
        }
    }
}
