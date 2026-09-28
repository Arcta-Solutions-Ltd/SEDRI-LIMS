using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the Turn Around Time page.
/// </summary>
internal class TurnAroundTimePageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration.
    /// </summary>
    public string Get()
    {
        var page = @"{
                            name: 'turnaroundtimepage',
                            pageTitle: '@GenTAT@',
                            text: '@GenTAT@',
                            wider: true,
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { Id: 'Id', Type: 'hidden' },
                                                { Id: 'LaboratoryId', Type: 'hidden' },
                                                { Id: 'SpecimenRangeGrid', Type: 'fieldgrid', Label: '@GenTATA@', Icon: 'Edit', IconText: '@GenComG@', Embedded: true, GridFields: [
                                                    { Id: 'Id', Type: 'hidden' },
                                                    { Id: 'RangeFromDays', Type: 'hidden' },
                                                    { Id: 'RangeFromHours', Type: 'hidden' },
                                                    { Id: 'RangeFromMinutes', Type: 'hidden' },
                                                    { Id: 'RangeToDays', Type: 'hidden' },
                                                    { Id: 'RangeToHours', Type: 'hidden' },
                                                    { Id: 'RangeToMinutes', Type: 'hidden' },
                                                    { Id: 'RangeDisplay', Type: 'text', Width: 'wide', FieldFormat: '@RangeFromDays@d @RangeFromHours@h @RangeFromMinutes@m - @RangeToDays@d @RangeToHours@h @RangeToMinutes@m', GridTitle: '@GenTATE@' },
                                                    { Id: 'Colour', Type: 'colourswatch', Width: 'medium', GridTitle: '@GenColA@' }
                                                ], RemoveGridAddButton: false, RemoveGridDeleteButton: false, IncludeGridFormButton: true, AddFormUIEvent: 'addturnaroundtimerangeuievent', FormUIEvent: 'editturnaroundtimerangeuievent', onFinish: 'updategrid' },
                                                { Id: 'DirectTestDefaultRangeGrid', Type: 'fieldgrid', Label: '@GenTATF@', Icon: 'Edit', IconText: '@GenComG@', Embedded: true, GridFields: [
                                                    { Id: 'Id', Type: 'hidden' },
                                                    { Id: 'RangeFromDays', Type: 'hidden' },
                                                    { Id: 'RangeFromHours', Type: 'hidden' },
                                                    { Id: 'RangeFromMinutes', Type: 'hidden' },
                                                    { Id: 'RangeToDays', Type: 'hidden' },
                                                    { Id: 'RangeToHours', Type: 'hidden' },
                                                    { Id: 'RangeToMinutes', Type: 'hidden' },
                                                    { Id: 'RangeDisplay', Type: 'text', Width: 'wide', FieldFormat: '@RangeFromDays@d @RangeFromHours@h @RangeFromMinutes@m - @RangeToDays@d @RangeToHours@h @RangeToMinutes@m', GridTitle: '@GenTATE@' },
                                                    { Id: 'Colour', Type: 'colourswatch', Width: 'medium', GridTitle: '@GenColA@' }
                                                ], RemoveGridAddButton: false, RemoveGridDeleteButton: false, IncludeGridFormButton: true, AddFormUIEvent: 'addturnaroundtimerangeuievent', FormUIEvent: 'editturnaroundtimerangeuievent', onFinish: 'updategrid' },
                                                { Id: 'DirectTestOverrideGrid', Type: 'fieldgrid', Label: '@GenTATG@', Icon: 'Edit', IconText: '@GenComG@', Embedded: true, GridFields: [
                                                    { Id: 'Id', Type: 'hidden' },
                                                    { Id: 'TestName', Type: 'combobox', Width: 'wide', GridTitle: '@GenTesD@', optionsName: 'directtestconfiglist' },
                                                    { Id: 'RangeFromDays', Type: 'hidden' },
                                                    { Id: 'RangeFromHours', Type: 'hidden' },
                                                    { Id: 'RangeFromMinutes', Type: 'hidden' },
                                                    { Id: 'RangeToDays', Type: 'hidden' },
                                                    { Id: 'RangeToHours', Type: 'hidden' },
                                                    { Id: 'RangeToMinutes', Type: 'hidden' },
                                                    { Id: 'RangeDisplay', Type: 'text', Width: 'wide', FieldFormat: '@RangeFromDays@d @RangeFromHours@h @RangeFromMinutes@m - @RangeToDays@d @RangeToHours@h @RangeToMinutes@m', GridTitle: '@GenTATE@' },
                                                    { Id: 'Colour', Type: 'colourswatch', Width: 'medium', GridTitle: '@GenColA@' }
                                                ], RemoveGridAddButton: false, RemoveGridDeleteButton: false, IncludeGridFormButton: true, AddFormUIEvent: 'adddirecttestoverrideuievent', FormUIEvent: 'editdirecttestoverrideuievent', onFinish: 'updategrid' },
                                                { Id: 'CultureTestDefaultRangeGrid', Type: 'fieldgrid', Label: '@GenTATH@', Icon: 'Edit', IconText: '@GenComG@', Embedded: true, GridFields: [
                                                    { Id: 'Id', Type: 'hidden' },
                                                    { Id: 'RangeFromDays', Type: 'hidden' },
                                                    { Id: 'RangeFromHours', Type: 'hidden' },
                                                    { Id: 'RangeFromMinutes', Type: 'hidden' },
                                                    { Id: 'RangeToDays', Type: 'hidden' },
                                                    { Id: 'RangeToHours', Type: 'hidden' },
                                                    { Id: 'RangeToMinutes', Type: 'hidden' },
                                                    { Id: 'RangeDisplay', Type: 'text', Width: 'wide', FieldFormat: '@RangeFromDays@d @RangeFromHours@h @RangeFromMinutes@m - @RangeToDays@d @RangeToHours@h @RangeToMinutes@m', GridTitle: '@GenTATE@' },
                                                    { Id: 'Colour', Type: 'colourswatch', Width: 'medium', GridTitle: '@GenColA@' }
                                                ], RemoveGridAddButton: false, RemoveGridDeleteButton: false, IncludeGridFormButton: true, AddFormUIEvent: 'addturnaroundtimerangeuievent', FormUIEvent: 'editturnaroundtimerangeuievent', onFinish: 'updategrid' },
                                                { Id: 'CultureTestOverrideGrid', Type: 'fieldgrid', Label: '@GenTATI@', Icon: 'Edit', IconText: '@GenComG@', Embedded: true, GridFields: [
                                                    { Id: 'Id', Type: 'hidden' },
                                                    { Id: 'TestName', Type: 'combobox', Width: 'wide', GridTitle: '@GenTesD@', optionsName: 'culturetestconfiglist' },
                                                    { Id: 'RangeFromDays', Type: 'hidden' },
                                                    { Id: 'RangeFromHours', Type: 'hidden' },
                                                    { Id: 'RangeFromMinutes', Type: 'hidden' },
                                                    { Id: 'RangeToDays', Type: 'hidden' },
                                                    { Id: 'RangeToHours', Type: 'hidden' },
                                                    { Id: 'RangeToMinutes', Type: 'hidden' },
                                                    { Id: 'RangeDisplay', Type: 'text', Width: 'wide', FieldFormat: '@RangeFromDays@d @RangeFromHours@h @RangeFromMinutes@m - @RangeToDays@d @RangeToHours@h @RangeToMinutes@m', GridTitle: '@GenTATE@' },
                                                    { Id: 'Colour', Type: 'colourswatch', Width: 'medium', GridTitle: '@GenColA@' }
                                                ], RemoveGridAddButton: false, RemoveGridDeleteButton: false, IncludeGridFormButton: true, AddFormUIEvent: 'addculturetestoverrideuievent', FormUIEvent: 'editculturetestoverrideuievent', onFinish: 'updategrid' }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}
