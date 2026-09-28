import React, { useState, useEffect } from 'react';
import {
    ColorPicker,
    ChoiceGroup,
    Toggle,
    getColorFromString,
    updateA,
    Label,
} from '@fluentui/react';
import { mergeStyleSets } from '@fluentui/react/lib/Styling';
import TranslateTag from '../../../Utils/Local/TranslateTag';

const ArcColourPicker = (props) => {
    const white = getColorFromString('#ffffff');

    const [color, setColor] = useState(white);
    const [showPreview, setShowPreview] = useState(true);
    const [alphaType, setAlphaType] = useState('alpha');
    const justUpdatedFromUser = React.useRef(false);

    useEffect(() => {
        if (justUpdatedFromUser.current) {
            justUpdatedFromUser.current = false;
            return;
        }
        if (props.config.value !== undefined) {
            const colour = getColorFromString(props.config.value);
            setColor(colour);
        }
    }, [props]);

    const alphaOptions = [
        { key: 'alpha', text: TranslateTag('@GenAlp@', props.language) },
        {
            key: 'transparency',
            text: TranslateTag('@GenTraA@', props.language),
        },
        { key: 'none', text: TranslateTag('@GenNon@', props.language) },
    ];

    const classNames = mergeStyleSets({
        wrapper: { display: 'flex' },
        column2: { marginLeft: 10 },
    });

    const colorPickerStyles = {
        panel: { padding: 12 },
        root: {
            maxWidth: 352,
            minWidth: 352,
        },
        colorRectangle: { height: 268 },
    };

    const updateColor = React.useCallback(
        (ev, colorObj) => {
            justUpdatedFromUser.current = true;
            setColor(colorObj);
            props.changeHandler(props.config.Id, colorObj.str);
        },
        [props]
    );

    const onShowPreviewClick = React.useCallback(
        (ev, checked) => setShowPreview(!!checked),
        []
    );
    const onAlphaTypeChange = React.useCallback(
        (ev, option = alphaOptions[0]) => {
            if (option.key === 'none') {
                setColor(updateA(color, 100));
            }
            setAlphaType(option.key);
        },
        [color, alphaOptions]
    );

    return (
        <div id={props.config.Id}>
            <Label>{props.config.Label}</Label>
            <div className={classNames.wrapper}>
                <ColorPicker
                    color={color}
                    onChange={updateColor}
                    alphaType={alphaType}
                    showPreview={showPreview}
                    styles={colorPickerStyles}
                    strings={{
                        hueAriaLabel: TranslateTag('@GenHue@', props.language),
                        green: TranslateTag('@GenGre@', props.language),
                        blue: TranslateTag('@GenBlu@', props.language),
                        red: TranslateTag('@GenRed@', props.language),
                        alpha: TranslateTag('@GenAlpA@', props.language),
                        hex: TranslateTag('@GenHex@', props.language),
                    }}
                />

                <div className={classNames.column2}>
                    <Toggle
                        label={TranslateTag('@GenSho@', props.language)}
                        onChange={onShowPreviewClick}
                        checked={showPreview}
                    />
                    <ChoiceGroup
                        label={TranslateTag('@GenAlpB@', props.language)}
                        options={alphaOptions}
                        defaultSelectedKey={alphaOptions[0].key}
                        onChange={onAlphaTypeChange}
                    />
                </div>
            </div>
        </div>
    );
};

export default ArcColourPicker;
