using arc.app.Common;

namespace arc.app.Config.Forms;
internal class DeleteImageFormConfig : IDefinition
{
    public string Get()
    {
        return @"{
            'name': 'deleteimageform',
            'pages': [
                'deleteimagepage'
            ],
            'title': 'Delete image form',
            'initialQuery': 'SingleImageQuery',
            'saveEvent': 'deleteimageevent',
            'configurable': 'No',
            'suppressRecordView': true
        }";
    }
}
