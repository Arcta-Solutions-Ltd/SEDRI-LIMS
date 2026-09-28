using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace arc.app.Config.UIEvents.Export
{
    internal class RunExportUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'runexportuievent',
                        description: 'Run Export',
                        type: 'form',
                        action: 'runexportform'
                    }";

            return newEvent;
        }
    }
}
