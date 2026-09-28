using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Form for adding a new fieldgrid column. Uses subform pattern with deferSave.
    /// Field options for RulesEditor come from parent (other GridFields in same grid).
    /// </summary>
    internal class AddFieldGridColumnFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addfieldgridcolumnform',
                        viewTitle: '@ConAddGC@',
                        saveEvent: 'addfieldgridcolumn',
                        saveOperation: 'updategrid',
                        suppressRecordView: true,
                        pages: [ 'addfieldgridcolumnpage']
                    }";

            return form;
        }
    }
}
