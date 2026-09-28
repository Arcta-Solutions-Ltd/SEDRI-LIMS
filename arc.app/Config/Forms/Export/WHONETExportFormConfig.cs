using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class WHONETExportFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'whonetexportform',
                        viewTitle: 'WHONET export',
                        saveEvent: 'whonetexport',
                        suppressRecordView: true,
                        pages: ['whonetexportpage']
                    }";

            return form;
        }
    }
}
