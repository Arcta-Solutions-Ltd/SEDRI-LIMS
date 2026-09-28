using arc.app.Common;

namespace arc.app.Config.Events.Images;

/// <summary>
/// Configuration for the upload image event
/// </summary>
internal class UploadImageEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            'EventName': 'uploadimageevent',
            'Description': '@ImgUplA@',
            'EventType': 'specialadddata',
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
