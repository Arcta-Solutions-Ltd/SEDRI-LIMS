import React from 'react';
import { IconButton, ActionButton } from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import { getAstFieldOptions } from './useMappingConstraints';

/**
 * Render the canonical mapping tree as a left-pane editable tree. Uses a
 * recursive component so that arbitrary nesting of objects and arrays can be
 * represented. Each node exposes contextual actions (add attribute / add array
 * / remove) that are dispatched up to the parent editor through callbacks.
 *
 * @param {object} props
 * @param {object} props.node - current node to render.
 * @param {boolean} props.isRoot - true when the node is the root of the tree.
 * @param {Array}   props.fieldOptions - profile field options.
 * @param {function(node:object):void} props.onAddAttribute
 * @param {function(node:object):void} props.onAddArray
 * @param {function(node:object):void} props.onRemove
 * @param {Array}   props.language
 */
const MappingTree = (props) => {
    const { node, isRoot, language } = props;
    if (!node) return null;

    const fieldOptions = props.fieldOptions || [];
    const findFieldText = (key) => {
        const opt = fieldOptions.find((o) => (o.Key || o.key) === key);
        return opt ? (opt.Text || opt.text || key) : key;
    };

    const isUnboundAttribute = node.kind === 'attribute'
        && !(node.fieldKey && String(node.fieldKey).trim())
        && !(node.gridSubFieldId && String(node.gridSubFieldId).trim());
    const isUntypedArray = node.kind === 'array' && !(node.arrayType && String(node.arrayType).trim());

    const renderHeader = () => {
        if (node.kind === 'attribute') {
            if (isUnboundAttribute) {
                return (
                    <span className="mappingtree-header">
                        <span className="mappingtree-attribute">{node.name}</span>
                        <span className="mappingtree-unmapped"> &mdash; {TranslateTag('@ExpProMapUnmapped@', language) || 'no field - will be ignored'}</span>
                    </span>
                );
            }
            const target = node.gridSubFieldId
                ? `${TranslateTag('@ExpProMapField@', language) || 'Field'}: ${node.gridSubFieldId}`
                : `${TranslateTag('@ExpProMapField@', language) || 'Field'}: ${findFieldText(node.fieldKey)}`;
            return (
                <span className="mappingtree-header">
                    <span className="mappingtree-attribute">{node.name}</span>
                    <span className="mappingtree-target"> &mdash; {target}</span>
                </span>
            );
        }
        if (node.kind === 'array') {
            return (
                <span className="mappingtree-header">
                    <span className="mappingtree-array">{node.name}</span>
                    {isUntypedArray ? (
                        <span className="mappingtree-unmapped"> &mdash; {TranslateTag('@ExpProMapUntyped@', language) || 'no type - will be ignored'}</span>
                    ) : (
                        <span className="mappingtree-subtype"> [{node.arrayType}]</span>
                    )}
                </span>
            );
        }
        return (
            <span className="mappingtree-header">
                <span className="mappingtree-object">{node.name || '{ }'}</span>
            </span>
        );
    };

    const canHaveChildren = node.kind === 'object' || node.kind === 'array';
    const isGridArray = node.kind === 'array' && node.arrayType === 'grid';
    const hasAst = getAstFieldOptions(fieldOptions).length > 0;
    const canAddArrayHere = node.kind === 'object'
        || (node.kind === 'array' && node.arrayType === 'cultures' && hasAst);

    const needsMapping = isUnboundAttribute || isUntypedArray;

    return (
        <div className={`mappingtree-node mappingtree-${node.kind}${needsMapping ? ' mappingtree-needsmapping' : ''}`}>
            <div className="mappingtree-row">
                {renderHeader()}
                <div className="mappingtree-actions">
                    {canHaveChildren && (
                        <ActionButton
                            id={isRoot ? 'mapping-root-add-attribute' : undefined}
                            iconProps={{ iconName: 'Add' }}
                            text={TranslateTag('@ExpProMapAddAttr@', language) || 'Add attribute'}
                            onClick={() => props.onAddAttribute(node)}
                        />
                    )}
                    {canAddArrayHere && (
                        <ActionButton
                            id={isRoot ? 'mapping-root-add-array' : undefined}
                            iconProps={{ iconName: 'BulletedList' }}
                            text={TranslateTag('@ExpProMapAddArr@', language) || 'Add array'}
                            onClick={() => props.onAddArray(node)}
                        />
                    )}
                    {node.kind === 'attribute' && props.onEditAttribute && (
                        <IconButton
                            iconProps={{ iconName: 'Edit' }}
                            title={TranslateTag('@ExpProMapBind@', language) || 'Bind field'}
                            ariaLabel={TranslateTag('@ExpProMapBind@', language) || 'Bind field'}
                            onClick={() => props.onEditAttribute(node)}
                        />
                    )}
                    {node.kind === 'array' && props.onEditArray && (
                        <IconButton
                            iconProps={{ iconName: 'Edit' }}
                            title={TranslateTag('@ExpProMapClassify@', language) || 'Classify array'}
                            ariaLabel={TranslateTag('@ExpProMapClassify@', language) || 'Classify array'}
                            onClick={() => props.onEditArray(node)}
                        />
                    )}
                    {!isRoot && (
                        <IconButton
                            iconProps={{ iconName: 'Delete' }}
                            title={TranslateTag('@ExpProMapDelNode@', language) || 'Remove'}
                            ariaLabel={TranslateTag('@ExpProMapDelNode@', language) || 'Remove'}
                            onClick={() => props.onRemove(node)}
                        />
                    )}
                </div>
            </div>
            {canHaveChildren && Array.isArray(node.children) && node.children.length > 0 && (
                <div className="mappingtree-children">
                    {node.children.map((child) => (
                        <MappingTree
                            key={child.__id}
                            node={child}
                            isRoot={false}
                            fieldOptions={fieldOptions}
                            onAddAttribute={props.onAddAttribute}
                            onAddArray={props.onAddArray}
                            onEditAttribute={props.onEditAttribute}
                            onEditArray={props.onEditArray}
                            onRemove={props.onRemove}
                            language={language}
                        />
                    ))}
                </div>
            )}
            {isGridArray && (
                <div className="mappingtree-gridhint">
                    {TranslateTag('@ExpProMapArrGrid@', language) || 'Grid'}
                </div>
            )}
        </div>
    );
};

export default MappingTree;
