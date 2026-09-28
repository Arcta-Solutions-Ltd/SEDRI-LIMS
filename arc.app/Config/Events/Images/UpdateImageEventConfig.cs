using arc.app.Common;

namespace arc.app.Config.Events.Images;

/// <summary>
/// Configuration for the update image event
/// </summary>
internal class UpdateImageEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            'EventName': 'updateImageEvent',
            'Description': '@ImgEdiA@',
            'EventType': 'special',
            'Topic': 'Configuration',
            'TableName': 'images',
            'ValidationRules': [
                { 'field': 'name', 'rule': 'required', 'message': '@ImgYou@' },
                { 'field': 'description', 'rule': 'required', 'message': '@ImgYouA@' },
                { 'field': 'fileattachmentid', 'rule': 'required', 'message': '@ImgYouB@' }
            ],
            'Display': [
                { 'Label': 'Name', 'Translation': '@GenNam@', 'List': 'No' },
                { 'Label': 'Description', 'Translation': '@GenDis@', 'List': 'No' }
            ]
        }";
    }
}
