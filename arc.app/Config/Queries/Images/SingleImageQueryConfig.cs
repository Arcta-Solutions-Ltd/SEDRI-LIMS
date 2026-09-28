using arc.app.Common;

namespace arc.app.Config.Queries.Images;

/// <summary>
/// Configuration for the single image query
/// </summary>
internal class SingleImageQueryConfig : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'SingleImageQuery',
            'Type': 'Single',
            'TableName': 'images',
            'Fields': [
                { 'Name': 'id', 'Type': 'string' },
                { 'Name': 'name', 'Type': 'string' },
                { 'Name': 'description', 'Type': 'string' },
                { 'Name': 'fileattachmentid', 'Type': 'int' }
            ],
            'Where' : [
                {'Field': 'Id', 'Comparison': '=' }
            ],
        }";
    }
}
