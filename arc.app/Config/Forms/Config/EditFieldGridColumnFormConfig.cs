using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Form for editing a fieldgrid column. Uses subform pattern with deferSave.
    /// Field options for RulesEditor come from parent (other GridFields in same grid).
    /// </summary>
    internal class EditFieldGridColumnFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editfieldgridcolumnform',
                        viewTitle: '@ConEdiGC@',
                        saveEvent: 'editfieldgridcolumn',
                        saveOperation: 'updategrid',
                        suppressRecordView: true,
                        pages: [ 'editfieldgridcolumnpage']
                    }";

            return form;
        }
    }
}
