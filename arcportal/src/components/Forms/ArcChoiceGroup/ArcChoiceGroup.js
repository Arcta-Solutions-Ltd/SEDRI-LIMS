import React from 'react';
import { ChoiceGroup } from '@fluentui/react';
import { connect } from 'react-redux';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { appendOtherOptionIfNeeded } from '../../../Utils/Forms/OtherOptionConstants';

const ArcChoiceGroup = (props) => {
    const radioChangeHandler = (event, value) => {
        props.radioChangeHandler(props.config.Id, value.key);
    };

    const label = props.config.Label;

    const fieldForOptions = { ...props.config, Options: Array.isArray(props.config.Options) ? [...props.config.Options] : [] };
    appendOtherOptionIfNeeded(fieldForOptions, props.otherOptionParentIds);

    const options = (fieldForOptions.Options || []).map((option) => {
        const text = typeof option.text === 'string' && option.text.startsWith('@')
            ? TranslateTag(option.text, props.language)
            : option.text;
        return {
            key: option.key,
            text,
            id: `${props.config.Id}-option-${option.key}`,
        };
    });

    return (
        <ChoiceGroup
            id={props.config.Id}
            selectedKey={props.config.value}
            onKeyDown={props.onKeyDown}
            options={options}
            onChange={radioChangeHandler}
            label={label}
            disabled={props.config.ReadOnly === true}
        />
    );
};

const mapStateToProps = (state) => ({
    language: state.config.language,
});

export default connect(mapStateToProps)(ArcChoiceGroup);
