import { IconButton, TooltipHost } from '@fluentui/react';
import { HighContrastSelector } from '@fluentui/react';
import React from 'react';
import { getStandardTooltipProps } from '../../../Utils/General/StandardTooltipProps';

const ArcSplitButton = (props) => {

    const icon = { iconName: props.icon };
    const tooltipText = props.tooltip || props.ariaLabel || 'Move';
    const ariaLabel = props.ariaLabel || tooltipText;

    const customSplitButtonStyles = {
        splitButtonMenuButton: { backgroundColor: 'white', width: 28, border: 'none' },
        splitButtonMenuIcon: { fontSize: '7px' },
        splitButtonDivider: { backgroundColor: '#c8c8c8', width: 1, right: 26, position: 'absolute', top: 4, bottom: 4 },
        splitButtonContainer: {
          selectors: {
            [HighContrastSelector]: { border: 'none' },
          },
        },
      };

    const button = (
        <IconButton
            id={props.id}
            split
            iconProps={icon}
            aria-roledescription="split button"
            styles={customSplitButtonStyles}
            menuProps={props.options}
            ariaLabel={ariaLabel}
            onClick={props.onClick}
        />
    );

    return (
        <TooltipHost content={tooltipText} tooltipProps={getStandardTooltipProps()} calloutProps={{ gapSpace: 10 }}>
            {button}
        </TooltipHost>
    );
};

export default ArcSplitButton;
