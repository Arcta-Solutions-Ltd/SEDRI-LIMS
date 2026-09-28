using System;
using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditTableFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'edittableform',
                        viewTitle: 'Edit a new table.',
                        saveEvent: 'edittable',
                        initialQuery: 'edittablequery',
                        suppressRecordView: true,
                        pages: ['edittablepage']
                    }";

            return form;
        }
    }
}
