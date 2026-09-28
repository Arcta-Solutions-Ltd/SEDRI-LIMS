import React from 'react';
import { Dialog, DialogType, DialogFooter } from '@fluentui/react/lib/Dialog';
import { PrimaryButton, DefaultButton } from '@fluentui/react';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { getRepeatTitle, getRepeatPrompt } from '../../../Utils/Forms/RepeatConfig';

/**
 * Asked after a form with a repeat configuration has saved: run the form again from its repeat page, or
 * finish. Used by the Neoshield request so several specimens can be registered against one request.
 *
 * @param {object} props
 * @param {boolean} props.visible
 * @param {object} props.repeat - The Repeat block from the form config, supplying the title and prompt. Its
 * text arrives translated, because configuration is translated in the backend.
 * @param {Array} props.language
 * @param {function():void} props.onRepeat
 * @param {function():void} props.onFinish
 */
const RepeatPrompt = (props) => {

    if (!props.visible) { return null; }

    const repeatClickHandler = (event) => {
        event.preventDefault();
        event.stopPropagation();
        props.onRepeat();
    };

    const finishClickHandler = (event) => {
        event.preventDefault();
        event.stopPropagation();
        props.onFinish();
    };

    return (
        <Dialog
            hidden={false}
            onDismiss={() => {}}
            minWidth={420}
            dialogContentProps={{
                type: DialogType.normal,
                title: getRepeatTitle(props.repeat),
                subText: getRepeatPrompt(props.repeat)
            }}
            modalProps={{ isBlocking: true }}
        >
            <DialogFooter>
                <PrimaryButton id="repeat-prompt-yes" onClick={repeatClickHandler} text={TranslateTag('@GenYesA@', props.language)} />
                <DefaultButton id="repeat-prompt-no" onClick={finishClickHandler} text={TranslateTag('@GenNo@', props.language)} />
            </DialogFooter>
        </Dialog>
    );
};

export default RepeatPrompt;
