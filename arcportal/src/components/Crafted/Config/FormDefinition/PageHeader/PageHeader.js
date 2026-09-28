import React from 'react';
import { IconButton, TooltipHost } from '@fluentui/react';
import TranslateTag from '../../../../../Utils/Local/TranslateTag';
import { getStandardTooltipProps } from '../../../../../Utils/General/StandardTooltipProps';

/**
 * Renders the page title and add form group/edit/delete/reorder/visibility actions for a page in the
 * Form Definition. Respects configureActions (add/edit/delete) when present on the page.
 * When useFormDefinitionStyle is true, uses bordered container styling; otherwise uses section title styling.
 * @param {Object} props.canAddFormGroup - Whether to show the add form group button
 * @param {Function} props.addFormGroup - Callback when add form group is clicked
 * @param {Object} props.canReorder - Whether to show the reorder form groups button
 * @param {Function} props.reorder - Callback when reorder is clicked
 * @param {Object} props.canEditRules - Whether to show the page visibility button
 * @param {Function} props.editRules - Callback when page visibility is clicked
 */
const PageHeader = (props) => {
    let canEdit = props.canEdit;
    let canDelete = props.canDelete;
    let canAddFormGroup = props.canAddFormGroup;
    let canReorder = props.canReorder;

    if (
        props.page.configureActions !== undefined &&
        props.page.configureActions !== null
    ) {
        canEdit = canEdit && props.page.configureActions.includes('edit');
        canDelete = canDelete && props.page.configureActions.includes('delete');
        canAddFormGroup = canAddFormGroup && props.page.configureActions.includes('add');
        canReorder = canReorder && props.page.configureActions.includes('edit');
    } else {
        canAddFormGroup = false;
    }

    const pageLabel = props.useFormDefinitionStyle && props.language
        ? TranslateTag("@GenPagA@", props.language) + ": "
        : "";

    const headerClass = props.useFormDefinitionStyle ? "formdefinition-page-header" : "managerecord-sectiontitle";

    const editPageTooltip = TranslateTag("@ConEdi@", props.language) || "Edit Form";
    const deletePageTooltip = TranslateTag("@ConDelX@", props.language) || "Delete Page";
    const reorderTooltip = TranslateTag("@ConReorderFG@", props.language) || "Reorder subsections";
    const pageRulesTooltip = TranslateTag("@ConPagVis@", props.language) || "Page Visibility";
    const addFormGroupTooltip = TranslateTag("@ConAddFG@", props.language) || "Add subsection";
    const showTableName = props.page.showTableName === true;
    const resolvedTableName = (props.page.tableName || props.page.TableName || 'specimen').toLowerCase();

    const renderTooltipIconButton = (tooltip, id, iconName, onClick) => (
        <TooltipHost
            id={`${id}-tooltip`}
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
        <div className={headerClass}>
            {pageLabel}{props.page.pageTitle}
            {showTableName && (
                <span id={`page-tablename-${props.page.id}`} className="formdefinition-page-tablename">
                    {' '}[{resolvedTableName}]
                </span>
            )}
            <span className="formdefinition-menuposition">
                {canAddFormGroup && props.addFormGroup && renderTooltipIconButton(
                    addFormGroupTooltip,
                    `page-addformgroup-${props.page.id}`,
                    'Add',
                    () => props.addFormGroup(props.page)
                )}
                {canEdit && renderTooltipIconButton(
                    editPageTooltip,
                    `page-edit-${props.page.id}`,
                    'Edit',
                    () => props.edit(props.page)
                )}
                {canDelete && renderTooltipIconButton(
                    deletePageTooltip,
                    `page-delete-${props.page.id}`,
                    'Delete',
                    () => props.delete(props.page)
                )}
                {canReorder && props.reorder && renderTooltipIconButton(
                    reorderTooltip,
                    `page-reorderfg-${props.page.id}`,
                    'ChevronUnfold10',
                    () => props.reorder(props.page)
                )}
                {props.canEditRules && props.editRules && renderTooltipIconButton(
                    pageRulesTooltip,
                    `page-rules-${props.page.id}`,
                    'BranchFork2',
                    () => props.editRules(props.page)
                )}
            </span>
        </div>
    );
};

export default PageHeader;
