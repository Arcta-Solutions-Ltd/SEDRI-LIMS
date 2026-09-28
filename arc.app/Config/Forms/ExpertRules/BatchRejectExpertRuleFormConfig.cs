using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Form configuration for batch rejecting expert rules from the list view.
/// </summary>
internal class BatchRejectExpertRuleFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch reject expert rule form.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'batchrejectexpertruleform',
            viewTitle: '@RulBatE@',
            saveEvent: 'batchrejectexpertrule',
            suppressRecordView: true,
            pages: ['batchrejectexpertrulepage']
        }";
    }
}
