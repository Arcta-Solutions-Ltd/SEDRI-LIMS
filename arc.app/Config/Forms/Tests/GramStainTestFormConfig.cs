using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class GramStainTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'gramstaintestform',
                        uievent: 'gramstaintestuievent',
                        title: '@TesGra@',
                        saveEvent: 'gramstaintest',
                        initialQuery: 'gramstaintestbyid',
                        suppressRecordView: true,
                        formtype: 'directtest',
                        datasection: 'GramStainDataSection',
                        configurable: 'Yes',
                        pages: ['gramstaintestpage']
                    }";

            return form;
        }
    }
}
