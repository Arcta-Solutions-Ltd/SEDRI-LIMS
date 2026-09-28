import React from 'react';
import { TooltipHost } from '@fluentui/react';
import { Icon } from '@fluentui/react/lib/Icon';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import { DEFAULT_EXPERT_RULE_COLOUR } from '../expertRuleColours';

/**
 * Inline icon shown next to an AST line (Disk/MIC or special-consideration line) that triggered one or more
 * expert rules. Hovering lists every triggered rule (action rules and guidance rules) colour-coded to match the
 * Expert Rules section, making the linkage between an AST line and the rules it triggered easy to see.
 *
 * @param {Object} props
 * @param {Array<{ruleId:number, ruleName:string, ruleText:string, colour:string, hasActions:boolean}>} props.triggers - Rules triggered by this line.
 * @param {string} [props.id] - DOM id for the icon (used by tests).
 * @param {string} [props.language] - Current language for the accessibility label.
 * @param {string} [props.ariaLabelTag] - Translation tag for the accessibility label (defaults to '@AstExpTrg@'); the
 *   organism-name guidance icon passes '@AstOrgGui@'.
 */
const ExpertTriggerIcon = (props) => {
    const triggers = props.triggers;
    if (!Array.isArray(triggers) || triggers.length === 0) {
        return null;
    }

    const ariaLabelTag = props.ariaLabelTag || '@AstExpTrg@';

    const iconColour = triggers.length === 1 ? triggers[0].colour : DEFAULT_EXPERT_RULE_COLOUR;

    const hoverContent = (
        <div className="astform-expert-trigger-hover">
            {triggers.map((t) => (
                <div
                    key={t.ruleId}
                    id={`ast-expert-trigger-rule-${t.ruleId}`}
                    className="astform-expert-trigger-hover-item"
                >
                    <span
                        className="astform-expert-trigger-hover-swatch"
                        style={{ backgroundColor: t.colour }}
                        aria-hidden="true"
                    />
                    <span className="astform-expert-trigger-hover-text">
                        <strong>{t.ruleName}</strong>
                        {t.ruleText ? `: ${t.ruleText}` : ''}
                    </span>
                </div>
            ))}
        </div>
    );

    return (
        <TooltipHost content={hoverContent} id={props.id ? `${props.id}-tooltip` : undefined}>
            <span
                id={props.id}
                className="astform-expert-trigger-icon"
                style={{ backgroundColor: iconColour }}
                aria-label={TranslateTag(ariaLabelTag, props.language)}
            >
                <Icon iconName="DecisionSolid" />
            </span>
        </TooltipHost>
    );
};

export default ExpertTriggerIcon;
