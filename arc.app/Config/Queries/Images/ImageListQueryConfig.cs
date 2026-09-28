using arc.app.Common;

namespace arc.app.Config.Queries.Images;

/// <summary>
/// Configuration for the image list query
/// </summary>
internal class ImageListQueryConfig : IDefinition
{
    public string Get()
    {
        return @"{
            'QueryName': 'ImageListQuery',
            'Description': 'Get list of all active images',
            'TableName': 'images',
            'Type': 'Select',
            'Fields': [
                { 'Name': 'id', 'Type': 'int', 'IsPrimaryKey': true },
                { 'Name': 'name', 'Type': 'string' },
                { 'Name': 'description', 'Type': 'string' }
            ],
            'OrderBy': 'name'
        }";
    }
}
