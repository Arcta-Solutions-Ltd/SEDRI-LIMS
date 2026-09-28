import React from 'react';
import { IconButton, TooltipHost } from '@fluentui/react';
import TranslateTag from '../../../../../Utils/Local/TranslateTag';
import { getStandardTooltipProps } from '../../../../../Utils/General/StandardTooltipProps';

/**
 * Header for a form group in the Form Definition. Renders within a form group block
 * with the form group ordinal (1, 2, 3...) and Edit/Delete/Move actions.
 * Respects page configureActions (add/edit/delete) when present, matching PageHeader.
 * @param {Object} props.formGroup - Form group data (key used internally for routing)
 * @param {Object} props.page - Page data including configureActions
 * @param {string} props.pageName - Page name for stable DOM ids
 * @param {number} props.formGroupIndex - 0-based index for display (Form Group 1, 2, 3...)
 * @param {boolean} props.canMove - Whether the move subsection action is available
 * @param {Function} props.move - Invoked when the move icon is clicked
 */
const FormGroupHeader = (props) => {
    let canEdit = props.canEdit;
    let canDelete = props.canDelete;
    let canMove = props.canMove;

    if (
        props.page?.configureActions !== undefined &&
        props.page?.configureActions !== null
    ) {
        canEdit = canEdit && props.page.configureActions.includes('edit');
        canDelete = canDelete && props.page.configureActions.includes('delete');
        canMove = canMove && props.page.configureActions.includes('edit');
    }

    const formGroupLabel = props.language ? TranslateTag("@ConFG@", props.language) + ": " : "Subsection: ";
    const editFormGroupTooltip = TranslateTag("@ConEdiFG@", props.language) || "Edit subsection";
    const deleteFormGroupTooltip = TranslateTag("@ConDelFG@", props.language) || "Delete subsection";
    const moveFormGroupTooltip = TranslateTag("@ConMovFGGrp@", props.language) || "Move subsection";
    const editButtonId = props.pageName && props.formGroup?.formGroupKey
        ? `formgroup-edit-${props.pageName}-${props.formGroup.formGroupKey}`
        : undefined;
    const moveButtonId = props.pageName && props.formGroup?.formGroupKey
        ? `formgroup-move-${props.pageName}-${props.formGroup.formGroupKey}`
        : undefined;
    const deleteButtonId = props.pageName && props.formGroup?.formGroupKey
        ? `formgroup-delete-${props.pageName}-${props.formGroup.formGroupKey}`
        : undefined;

    const renderTooltipIconButton = (tooltip, id, iconName, onClick) => (
        <TooltipHost
            id={id ? `${id}-tooltip` : undefined}
            content={tooltip}
            tooltipProps={getStandardTooltipProps()}
            calloutProps={{ gapSpace: 10 }}
            delay={0}
        >
            <span title={tooltip} style={{ display: 'inline-block' }}>
                <IconButton
                    id={id}
                    iconProps={{ iconName }}
                    ariaLabel={tooltip}
                    onClick={onClick}
                    styles={{
                        root: { height: '20px', width: '20px', verticalAlign: 'middle' },
                    }}
                />
            </span>
        </TooltipHost>
    );

    return (
        <div className="formdefinition-formgroupheader">
            <span className="formdefinition-formgroupkey">{formGroupLabel}{props.formGroupIndex + 1}</span>
            <span className="formdefinition-menuposition">
                {canEdit && renderTooltipIconButton(
                    editFormGroupTooltip,
                    editButtonId,
                    'Edit',
                    () => props.edit(props.formGroup)
                )}
                {canMove && renderTooltipIconButton(
                    moveFormGroupTooltip,
                    moveButtonId,
                    'Move',
                    () => props.move(props.formGroup)
                )}
                {canDelete && renderTooltipIconButton(
                    deleteFormGroupTooltip,
                    deleteButtonId,
                    'Delete',
                    () => props.delete(props.formGroup)
                )}
            </span>
        </div>
    );
};

export default FormGroupHeader;
