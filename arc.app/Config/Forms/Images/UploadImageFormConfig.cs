using arc.app.Common;

namespace arc.app.Config.Forms.Images;

/// <summary>
/// Configuration for the upload image form
/// </summary>
internal class UploadImageFormConfig : IDefinition
{
    public string Get()
    {
        return @"{
            'name': 'uploadimageform',
            'pages': [
                'uploadimagepage'
            ],
            'title': '@ImgUpl@',
            'saveEvent': 'uploadimageevent',
            'configurable': 'No',
            'suppressRecordView': true
        }";
    }
}
