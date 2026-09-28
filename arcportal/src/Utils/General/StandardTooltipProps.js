/**
 * Standard tooltip styling: black text on white background.
 * Use with TooltipHost: tooltipProps={getStandardTooltipProps()}
 */
export const getStandardTooltipProps = () => ({
    styles: {
        content: { color: '#000000' }
    },
    calloutProps: {
        styles: {
            beak: { background: '#ffffff' },
            beakCurtain: { background: '#ffffff' },
            calloutMain: { background: '#ffffff' }
        }
    }
});
