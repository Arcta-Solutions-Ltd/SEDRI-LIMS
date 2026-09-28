using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace arc.app.Config.Forms.Export
{
    internal class DeleteExportProfileFieldFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deleteexportprofilefieldform',
                        viewTitle: 'Delete a export profile',
                        saveEvent: 'deleteexportprofilefield',
                        initialQuery: 'exportprofilefieldbyid',
                        suppressRecordView: true,
                        pages: ['deleteexportprofilefieldpage']
                    }";

            return form;
        }
    }
}
