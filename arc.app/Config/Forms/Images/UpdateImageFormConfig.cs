using arc.app.Common;

namespace arc.app.Config.Forms.Images;

/// <summary>
/// Configuration for the update image form
/// </summary>
internal class UpdateImageFormConfig : IDefinition
{
    public string Get()
    {
        return @"{
            'name': 'updateimageform',
            'pages': [
                'updateimagepage'
            ],
            'initialQuery': 'SingleImageQuery',
            'title': '@ImgUpd@',
            'saveEvent': 'updateimageevent',
            'configurable': 'No',
            'suppressRecordView': true
        }";
    }
}
