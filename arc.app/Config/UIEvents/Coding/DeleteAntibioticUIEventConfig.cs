using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace arc.app.Config.UIEvents.Coding
{
    internal class DeleteAntibioticUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'deleteantibioticuievent',
                        description: 'Remove Antibiotic',
                        type: 'form',
                        action: 'deleteantibioticform'
                    }";

            return newEvent;
        }
    }
}
