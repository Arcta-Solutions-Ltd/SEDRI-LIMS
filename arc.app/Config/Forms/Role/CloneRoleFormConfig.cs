using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class CloneRoleFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'cloneroleform',
                        initialQuery: 'rolebyidforeditrole',
                        saveEvent: 'clonerole',
                        finishButtonText: '@GenCloA@',
                        suppressRecordView: true,
                        pages: ['clonerolepage']
                    }";

            return form;
        }
    }
}
