using arc.app.Common;

namespace arc.app.Config.Queries.Specimen
{
    /// <summary>
    /// This class defines the query to add a culture comment.
    /// Implements the IDefinition interface.
    /// </summary>
    internal class AddCultureCommentQuery : IDefinition
    {
        /// <summary>
        /// Retrieves the query details as a JSON string.
        /// </summary>
        /// <returns>A JSON string containing the query details.</returns>
        public string Get()
        {
            return @"{ 
                    'Query': 'AddCultureCommentQuery',     
                    'TableName': 'Culture',                
                    'Type': 'Special',                     
                    'Translate': true                      
                }";
        }
    }
}

