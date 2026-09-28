using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class CellCountTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'cellcounttestform',
                        uievent: 'cellcounttestuievent',
                        title: '@TesCel@',
                        initialQuery: 'cellcounttestbyid',
                        saveEvent: 'cellcounttest',
                        suppressRecordView: true,
                        formtype: 'directtest',
                        datasection: 'CellCountDataSection',
                        configurable: 'Yes',
                        pages: ['cellcounttestpage']
                    }";

            return form;
        }
    }
}
