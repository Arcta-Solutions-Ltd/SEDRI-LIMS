using arc.app.Common;

namespace arc.app.Config.Forms.ExpertRules;

/// <summary>
/// Form configuration for batch approving expert rules from the list view.
/// </summary>
internal class BatchApproveExpertRuleFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch approve expert rule form.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'batchapproveexpertruleform',
            viewTitle: '@RulBatC@',
            saveEvent: 'batchapproveexpertrule',
            suppressRecordView: true,
            pages: ['batchapproveexpertrulepage']
        }";
    }
}
