using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace arc.app.Config.Forms.Export
{
    internal class EditExportProfileFieldsFormConfig:IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editexportprofilefieldsform',
                        viewTitle: 'Edit fields order',
                        title: '@TesEdiE@',
                        saveEvent: 'editexportprofilefields',
                        initialQuery: 'editexportprofilefieldsbyexportprofileid',
                        suppressRecordView: true,
                        pages: ['editexportprofilefieldspage']
                    }";

            return form;
        }
    }
}
