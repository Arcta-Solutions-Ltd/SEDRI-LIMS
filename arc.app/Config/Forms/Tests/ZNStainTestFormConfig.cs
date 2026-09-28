using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class ZNStainTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'znstaintestform',
                        uievent: 'znstaintestuievent',
                        saveEvent: 'znstaintest',
                        initialQuery: 'znstaintestbyid',
                        title: '@TesZnsA@',
                        suppressRecordView: true,
                        formtype: 'directtest',
                        datasection: 'ZnStainDataSection',
                        configurable: 'Yes',
                        pages: ['znstaintestpage']
                    }";

            return form;
        }
    }
}
