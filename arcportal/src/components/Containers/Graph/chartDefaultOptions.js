export const CHART_ANIMATION = { duration: 1000, easing: 'easeOutQuart' };

/**
 * Maps Arc graphtype strings to Chart.js chart type ids.
 * @param {string|undefined} type
 * @returns {string}
 */
export function chartJsTypeFromArcType(type) {
    const k = (type || '').toLowerCase();
    if (k === 'donut') return 'doughnut';
    if (k === 'polar') return 'polarArea';
    return k;
}

/**
 * Shared Chart.js options for graph wrappers (Analytics and Home dashboard).
 * @param {string|undefined} type - Arc graphtype (bar, line, pie, donut, polar, radar)
 * @param {boolean|undefined} stack - Stacked bar/line when true
 * @param {string|undefined} text - Chart title text
 * @param {boolean} fillContainer - When true, chart fills its tile container
 * @returns {import('chart.js').ChartOptions}
 */
export function buildChartOptions(type, stack, text, fillContainer) {
    const k = (type || '').toLowerCase();
    const options = {
        maintainAspectRatio: !fillContainer,
        responsive: true,
        animation: { ...CHART_ANIMATION },
        // Chart.js sizes a new canvas from the container width first, then re-measures and
        // resizes once the real height constraint is known. Its default resize transition has
        // duration 0, which snaps a freshly created chart to its final state and cancels the
        // enter animation. Giving resize the same timing lets that first draw run to completion.
        transitions: {
            resize: {
                animation: { ...CHART_ANIMATION },
            },
        },
        plugins: {
            legend: {
                position: 'bottom',
                ...(k === 'polar' ? { display: true } : {}),
            },
            title: {
                display: !fillContainer,
                text,
            },
        },
    };

    if (k === 'bar' || k === 'line') {
        options.scales = {
            x: { stacked: Boolean(stack) },
            y: { stacked: Boolean(stack), ticks: { beginAtZero: true } },
        };
    }

    if (k === 'radar') {
        options.scales = {
            r: {
                beginAtZero: true,
            },
        };
    }

    return options;
}
