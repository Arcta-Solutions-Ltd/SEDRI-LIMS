using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class OrderTableFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'ordertableform',
                        viewTitle: 'Order table.',
                        initialQuery: 'OrderTableQuery',
                        saveEvent: 'ordertable',
                        suppressRecordView: true,
                        pages: ['ordertablepage']
                    }";

            return form;
        }
    }
}
