import React from 'react';
import { TooltipHost } from '@fluentui/react';
import { Icon } from '@fluentui/react/lib/Icon';

import TranslateTag from '../../../../Utils/Local/TranslateTag';
import { getStandardTooltipProps } from '../../../../Utils/General/StandardTooltipProps';
import { resolveOverridePreviewText } from '../astSusceptibilityOverrideUtils';

/**
 * Inline icons for manually set susceptibility on an AST line. Edit opens the override panel;
 * trash reverts to calculated susceptibility. Hover shows audit metadata on the edit icon.
 */
const ManualSusceptibilityIcon = (props) => {
    const override = props.override;
    if (!override?.IsManuallySet && override?.isManuallySet !== true) {
        return null;
    }

    const preview = resolveOverridePreviewText(props.cannedOptions, override);
    const overriddenFromLabel = props.resolveSusceptibilityLabel
        ? props.resolveSusceptibilityLabel(override.OverriddenFromSusceptibilityId ?? override.overriddenFromSusceptibilityId)
        : '';
    const setByUsername = override.SetByUsername ?? override.setByUsername ?? '';

    const hoverContent = (
        <div className="astform-manual-override-hover">
            {setByUsername ? (
                <div>{TranslateTag('@AstSusSetBy@', props.language)}: {setByUsername}</div>
            ) : null}
            {override.SetAt || override.setAt ? (
                <div>{TranslateTag('@AstSusSetAt@', props.language)}: {new Date(override.SetAt ?? override.setAt).toLocaleString()}</div>
            ) : null}
            {overriddenFromLabel ? (
                <div>{TranslateTag('@AstSusOverFrom@', props.language)}: {overriddenFromLabel}</div>
            ) : null}
            {preview ? <div>{preview}</div> : null}
        </div>
    );

    const revertTooltipContent = (
        <div className="astform-manual-override-hover">
            <div>{TranslateTag('@AstSusRev@', props.language)}</div>
            <div>{TranslateTag('@AstSusRevConf@', props.language)}</div>
        </div>
    );

    const tooltipProps = getStandardTooltipProps();

    return (
        <div className="astform-manual-susceptibility-icons">
            <TooltipHost
                content={hoverContent}
                tooltipProps={tooltipProps}
                calloutProps={{ gapSpace: 10 }}
            >
                <button
                    type="button"
                    id={props.id}
                    className="astform-manual-susceptibility-icon"
                    aria-label={TranslateTag('@AstSusMan@', props.language)}
                    onClick={(e) => {
                        e.preventDefault();
                        e.stopPropagation();
                        if (props.onClick) {
                            props.onClick();
                        }
                    }}
                >
                    <Icon iconName="Edit" />
                </button>
            </TooltipHost>
            {props.onRevert ? (
                <TooltipHost
                    content={revertTooltipContent}
                    tooltipProps={tooltipProps}
                    calloutProps={{ gapSpace: 10 }}
                >
                    <button
                        type="button"
                        id={props.trashId}
                        className="astform-manual-susceptibility-trash"
                        aria-label={TranslateTag('@AstSusRev@', props.language)}
                        onClick={(e) => {
                            e.preventDefault();
                            e.stopPropagation();
                            props.onRevert();
                        }}
                    >
                        <Icon iconName="Trash" />
                    </button>
                </TooltipHost>
            ) : null}
        </div>
    );
};

export default ManualSusceptibilityIcon;
