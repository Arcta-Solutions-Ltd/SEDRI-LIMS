import React, { useMemo } from 'react';
import { ComboBox } from '@fluentui/react';
import { filterInstrumentProfilesForManualRequest, toComboOptions } from './filterInstrumentProfilesForManualRequest';

/**
 * Combobox of instrument profiles for the current embedded-list context; options come from InitialQuery
 * (InstrumentProfileDetails) and are filtered client-side.
 */
const InstrumentProfileSelector = (props) => {
    const ctx = props.data?.RequestContext ?? props.data?.requestContext;
    const details = props.data?.InstrumentProfileDetails ?? props.data?.instrumentProfileDetails ?? [];
    const filtered = useMemo(() => filterInstrumentProfilesForManualRequest(ctx, details), [ctx, details]);
    const options = useMemo(() => toComboOptions(filtered), [filtered]);
    const optionsStyled = useMemo(
        () =>
            options.map((o) => ({
                key: o.key,
                text: o.text,
                styles: {
                    optionText: {
                        fontFamily: 'Calibri, Calibri_MSFontService, sans-serif',
                        overflow: 'visible',
                        whiteSpace: 'normal',
                    },
                },
            })),
        [options]
    );

    const disabled = props.config?.Disabled && props.config.Disabled !== false ? true : false;
    const selectedKey = props.config?.value;

    const onChange = (event, option) => {
        props.changeHandler(event, option);
    };

    const tabIndex = props.config?.noTab === undefined || !props.config.noTab ? undefined : -1;

    if (!optionsStyled.length) {
        return null;
    }

    return (
        <ComboBox
            onKeyDown={props.onKeyDown}
            id={props.config.Id}
            required={props.config.Required}
            placeholder={props.config.Placeholder}
            label={props.config.Label}
            allowFreeform={false}
            autoComplete="on"
            useComboBoxAsMenuWidth
            selectedKey={selectedKey}
            options={optionsStyled}
            onChange={onChange}
            tabIndex={tabIndex}
            disabled={disabled}
        />
    );
};

export default InstrumentProfileSelector;
