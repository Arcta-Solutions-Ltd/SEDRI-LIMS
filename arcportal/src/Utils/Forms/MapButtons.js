const MapButtons = (buttons) => {

    const newButtons = buttons.map((button) => {
        return { key: button.Key,
                 text: button.Text,
                 icon: button.Icon,
                 UIEvent: button.UIEvent,
                 primaryAction: button.PrimaryAction,
                 onFinish: button.OnFinish ?? button.onFinish,
                 refreshConfig: button.RefreshConfig ?? button.refreshConfig,
                 buttons: button.Buttons ?? button.buttons,
                 rules: button.Rules,
                 addChildContext: button.AddChildContext ?? button.addChildContext,
                 prefillFormFields: button.PrefillFormFields ?? button.prefillFormFields
                }
    })

    for (const rec of newButtons) {
        if (rec.buttons !== undefined && Array.isArray(rec.buttons)) {
            rec.buttons = rec.buttons.map((button) => {
                return { key: button.Key,
                         text: button.Text,
                         icon: button.Icon,
                         UIEvent: button.UIEvent,
                         primaryAction: button.PrimaryAction,
                         onFinish: button.OnFinish ?? button.onFinish,
                         refreshConfig: button.RefreshConfig ?? button.refreshConfig,
                         addChildContext: button.AddChildContext ?? button.addChildContext,
                         prefillFormFields: button.PrefillFormFields ?? button.prefillFormFields
                        }
            });
        }
    }

    return newButtons;

}

export default MapButtons;