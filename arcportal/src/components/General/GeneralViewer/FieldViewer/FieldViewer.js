import React from 'react';
import { Separator } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import { translateEmbeddedLanguageTags } from '../../../../Utils/General/FormatAgeDisplay';
import FileThumbnailGallery from '../../FileThumbnailGallery/FileThumbnailGallery';
import { fieldIsVisibleInViewer } from '../fieldViewerVisibility';
import './FieldViewer.css';

/** Character count above which a field value spans the full row width in the flex grid. */
const FULL_WIDTH_CHAR_THRESHOLD = 80;

/**
 * Returns whether a read-only field should use the full-width row layout so long text wraps
 * without overlapping adjacent fields in the flex grid.
 *
 * @param {{ Type?: string, Value?: unknown }} field - Field config from a record view mapper.
 * @returns {boolean}
 */
export function fieldUsesFullWidthLayout(field) {
    if (field.Type === 'upload') {
        return true;
    }
    if (field.Type === 'multiline') {
        return true;
    }
    const valueLen = String(field.Value ?? '').length;
    return valueLen > FULL_WIDTH_CHAR_THRESHOLD;
}

/**
 * FieldViewer renders label/value pairs for standard record view sections.
 *
 * @param {Object} props
 * @param {Array} props.data - Field definitions (Id, Label, Value, Type, Highlight).
 * @param {string} props.language - Current language for Yes/No translation.
 */
const FieldViewer = (props) => {

    /**
     * Translates toggle display values (Yes/No) for the current language.
     *
     * @param {string} fieldValue
     * @returns {string}
     */
    const toggleTranslator = (fieldValue) => {
        switch (fieldValue) {
            case 'Yes':
                return TranslateTag("@GenYesA@", props.language);
            case 'No':
                return TranslateTag("@GenNo@", props.language);
            default:
                if (typeof fieldValue === 'string' && fieldValue.includes('@')) {
                    return translateEmbeddedLanguageTags(fieldValue, props.language);
                }
                return fieldValue;
        }
    };

    const fieldsToDisplay = (props.data ?? []).filter(fieldIsVisibleInViewer);

    return (
        <React.Fragment>
            <div className='fieldviewer-fieldcontent'>
                {fieldsToDisplay.map((field) => {
                    const isUpload = field.Type === 'upload';
                    const isFullWidth = fieldUsesFullWidthLayout(field);
                    const fieldClassName = [
                        'fieldviewer-field',
                        isUpload ? 'fieldviewer-upload' : '',
                        isFullWidth && !isUpload ? 'fieldviewer-fullwidth' : '',
                    ].filter(Boolean).join(' ');

                    return (
                        <div
                            key={field.Id}
                            id={field.Id ? `fieldviewer-field-${field.Id}` : undefined}
                            className={fieldClassName}
                        >
                            {field.Label && (
                                <div className='fieldviewer-itemname'>
                                    {field.Label + ':'}
                                </div>
                            )}
                            <div
                                id={field.Id ? `fieldviewer-${field.Id}` : undefined}
                                className={'fieldviewer-itemvalue' + (field.Highlight ? '-highlight ' : '') + (isUpload ? ' fieldviewer-upload-value' : '')}
                            >
                                {isUpload ? (
                                    <FileThumbnailGallery fileIds={field.Value} language={props.language} readOnly />
                                ) : (
                                    toggleTranslator(field.Value)
                                )}
                            </div>
                        </div>
                    );
                })}
            </div>
            <div className='fieldviewer-separator'>
                {(fieldsToDisplay.length > 0) ? (
                    <Separator></Separator>
                ) : (null)}
            </div>
        </React.Fragment>
    );
};

export default FieldViewer;
