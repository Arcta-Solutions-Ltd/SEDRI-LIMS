using arc.app.Common;

namespace arc.app.Config.Queries.Specimen
{
    /// <summary>
    /// This class defines the query to list comments by specimen ID.
    /// Implements the IDefinition interface.
    /// </summary>
    internal class CommentListBySpecimenIdQuery : IDefinition
    {
        /// <summary>
        /// Retrieves the query details as a JSON string.
        /// </summary>
        /// <returns>A JSON string containing the query details.</returns>
        public string Get()
        {
            return @"{
                    'Query': 'CommentListBySpecimenId',                        
                    'TableName': 'SpecimenComment', 
                    'Type': 'Special',
                    'Translate': true,
                    'Tags': 'SP'
                }";
        }
    }
}
