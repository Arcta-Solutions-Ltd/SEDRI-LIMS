using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Form configuration definition for removing a culture test.
/// Launches the 'removedirecttestpage' and posts to the 'removeculturetest' event.
/// </summary>
internal class RemoveCultureTestFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the Remove Culture Test form.
    /// - name: 'removeculturetestform'
    /// - saveEvent: 'removeculturetest'
    /// - initialquery: 'removeculturetestquery'
    /// - pages: [ 'removedirecttestpage' ]
    /// </summary>
    public string Get()
    {
        var form = @"{
                        name: 'removeculturetestform',
                        viewTitle: 'Remove a culture test.',
                        saveEvent: 'removeculturetest',
                        initialquery: 'removeculturetestquery',
                        suppressRecordView: true,
                        pages: ['removedirecttestpage']
                    }";

        return form;
    }
}
