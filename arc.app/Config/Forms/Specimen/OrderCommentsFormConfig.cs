using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class OrderCommentsFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'ordercommentsform',
                        viewTitle: 'Order comments definition.',
                        saveEvent: 'ordercomments',
                        initialquery: 'ordercommentsquery',
                        suppressRecordView: true,
                        pages: [ 'ordercommentspage']
                    }";

            return form;
        }
    }
}
