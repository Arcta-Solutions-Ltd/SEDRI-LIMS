using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace arc.app.Config.UIEvents.Export
{
    
    internal class DeleteExportProfileFieldUIEventConfig:IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleteexportprofilefielduievent',
                        description: 'Delete a field from profile',
                        type: 'form',
                        action: 'deleteexportprofilefieldform'
                    }";

            return newEvent;
        }
    }
}
