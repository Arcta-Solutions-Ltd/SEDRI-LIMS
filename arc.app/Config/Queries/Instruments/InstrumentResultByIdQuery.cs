using arc.app.Common;

namespace arc.app.Config.Queries;
/// <summary>
/// Represents a query to retrieve an instrument result by ID.
/// </summary>
internal class InstrumentResultByIdQuery : IDefinition
{
    /// <summary>
    /// Builds and returns a query string in JSON format.
    /// The query retrieves a single record from the 'InstrumentResults' table
    /// based on the specified ID field.
    /// </summary>
    /// <returns>A JSON-formatted query string.</returns>
    public string Get()
    {
        return @"{ 
                    'Query': 'InstrumentResultByIdQuery', 
                    'TableName': 'InstrumentResults', 
                    'Type': 'Single', 
                    'Fields': [
                        {'Name': 'Id', 'Type': 'int'}
                    ],
                    'Joins': [
                        { 'Table': 'Specimen', 'Fields': [{'Name': 'AccessionNumber'}] }
                    ],
                    'Where' : [
                        {'Field': 'Id', 'Comparison': '=' } 
                    ]
                }";
    }
}

