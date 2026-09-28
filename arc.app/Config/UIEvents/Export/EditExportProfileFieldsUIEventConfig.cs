using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace arc.app.Config.UIEvents.Export
{
    internal class EditExportProfileFieldsUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editexportprofilefieldsuievent',
                        description: 'Edit Fields order',
                        type: 'form',
                        action: 'editexportprofilefieldsform'
                    }";

            return newEvent;
        }
    }
}
