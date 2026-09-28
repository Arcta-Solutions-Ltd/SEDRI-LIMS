using arc.app.Common;

namespace arc.app.Config.Events.Images;

/// <summary>
/// Configuration for the delete image event
/// </summary>
internal class DeleteImageEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            'EventName': 'deleteImageEvent',
            'Description': '@ImgDelA@',
            'EventType': 'deletedata',
            'Topic': 'Configuration',
            'TableName': 'images',
            'ValidationRules': [
                { 'field': 'id', 'rule': 'required', 'message': 'Image ID is required' }
            ],
            'Display': [
                { 'Label': 'id', 'Translation': '@ImgId@', 'List': 'No' }
            ]
        }";
    }
}
