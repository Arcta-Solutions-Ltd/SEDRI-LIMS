import React, { useEffect, useRef, useMemo } from 'react';
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    BarElement,
    PointElement,
    LineElement,
    ArcElement,
    RadialLinearScale,
    BarController,
    LineController,
    PieController,
    DoughnutController,
    PolarAreaController,
    RadarController,
    Filler,
    Legend,
    Title,
    Tooltip,
} from 'chart.js';
import { buildChartOptions, chartJsTypeFromArcType } from './chartDefaultOptions';
import {
    buildChartSignature,
    createEnterGuardUntil,
    getDeferredUpdateDelayMs,
} from './graphChartLifecycle';

ChartJS.register(
    CategoryScale,
    LinearScale,
    BarElement,
    PointElement,
    LineElement,
    ArcElement,
    RadialLinearScale,
    BarController,
    LineController,
    PieController,
    DoughnutController,
    PolarAreaController,
    RadarController,
    Filler,
    Legend,
    Title,
    Tooltip
);

const SUPPORTED_TYPES = new Set(['bar', 'donut', 'line', 'pie', 'polar', 'radar']);

/**
 * Deep-clones chart data so Chart.js in-place mutations do not corrupt React state.
 * @param {import('chart.js').ChartData|undefined} data
 * @returns {import('chart.js').ChartData|undefined}
 */
function cloneChartData(data) {
    if (!data) return data;
    return JSON.parse(JSON.stringify(data));
}

/**
 * @param {React.ForwardedRef<import('chart.js').Chart|null>} ref
 * @param {import('chart.js').Chart|null} instance
 */
function assignChartRef(ref, instance) {
    if (typeof ref === 'function') {
        ref(instance);
    } else if (ref) {
        ref.current = instance;
    }
}

/**
 * Renders an Arc analytics chart with direct Chart.js lifecycle control.
 *
 * react-chartjs-2 calls chart.update() immediately after creating a chart, which cancels the
 * Chart.js enter animation. Creating the instance here keeps that animation on graphtype changes.
 * Exactly one instance exists at a time: changing chart type recreates it, everything else
 * (data, stacking, title) updates the existing instance so it animates in place.
 */
const GraphFactory = React.forwardRef((props, ref) => {
    const { type, data, text, stack, id, fillContainer } = props;
    const domId = id || 'graph';
    const arcType = (type || '').toLowerCase();
    const chartJsType = chartJsTypeFromArcType(arcType);
    const isSupported = Boolean(type) && SUPPORTED_TYPES.has(arcType);

    const canvasRef = useRef(null);
    const chartRef = useRef(null);
    const enterGuardUntilRef = useRef(0);
    const lastAppliedSignatureRef = useRef(null);
    const deferredUpdateTimerRef = useRef(null);

    const options = useMemo(
        () => buildChartOptions(arcType, stack, text, Boolean(fillContainer)),
        [arcType, stack, text, fillContainer]
    );

    const latestConfigRef = useRef({ data, options });
    latestConfigRef.current = { data, options };

    const chartSignature = useMemo(
        () => buildChartSignature(data, options),
        [data, options]
    );

    useEffect(() => {
        const canvas = canvasRef.current;
        const { data: latestData, options: latestOptions } = latestConfigRef.current;

        if (!canvas || !latestData || !isSupported) {
            return undefined;
        }

        if (deferredUpdateTimerRef.current) {
            clearTimeout(deferredUpdateTimerRef.current);
            deferredUpdateTimerRef.current = null;
        }

        const instance = new ChartJS(canvas, {
            type: chartJsType,
            data: cloneChartData(latestData),
            options: { ...latestOptions },
        });

        chartRef.current = instance;
        enterGuardUntilRef.current = createEnterGuardUntil();
        lastAppliedSignatureRef.current = buildChartSignature(latestData, latestOptions);
        assignChartRef(ref, instance);

        return () => {
            if (deferredUpdateTimerRef.current) {
                clearTimeout(deferredUpdateTimerRef.current);
                deferredUpdateTimerRef.current = null;
            }
            instance.destroy();
            chartRef.current = null;
            assignChartRef(ref, null);
        };
    }, [chartJsType, isSupported, ref]);

    useEffect(() => {
        const chart = chartRef.current;
        if (!chart || !data) {
            return undefined;
        }

        if (chartSignature === lastAppliedSignatureRef.current) {
            return undefined;
        }

        const applyUpdate = () => {
            const currentChart = chartRef.current;
            const { data: latestData, options: latestOptions } = latestConfigRef.current;
            const signature = buildChartSignature(latestData, latestOptions);

            if (!currentChart || !latestData || signature === lastAppliedSignatureRef.current) {
                return;
            }

            currentChart.data = cloneChartData(latestData);
            currentChart.options = { ...latestOptions };
            currentChart.update();
            lastAppliedSignatureRef.current = signature;
        };

        const delayMs = getDeferredUpdateDelayMs(enterGuardUntilRef.current);
        if (delayMs > 0) {
            deferredUpdateTimerRef.current = setTimeout(() => {
                deferredUpdateTimerRef.current = null;
                applyUpdate();
            }, delayMs);

            return () => {
                if (deferredUpdateTimerRef.current) {
                    clearTimeout(deferredUpdateTimerRef.current);
                    deferredUpdateTimerRef.current = null;
                }
            };
        }

        applyUpdate();
        return undefined;
    }, [chartSignature]);

    if (!isSupported) {
        return <>No graph found</>;
    }

    const canvas = (
        <canvas
            key={chartJsType}
            id={domId}
            ref={canvasRef}
            role="img"
            aria-label={text || 'Chart'}
        />
    );

    if (fillContainer) {
        return <div className="home-dashboard-graph-chart-fill">{canvas}</div>;
    }

    return canvas;
});

export default GraphFactory;
