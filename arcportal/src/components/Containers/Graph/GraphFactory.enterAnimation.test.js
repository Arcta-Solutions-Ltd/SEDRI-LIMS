import React from 'react';
import { render, act } from '@testing-library/react';
import { CHART_ANIMATION } from './chartDefaultOptions';

const mockUpdate = jest.fn();
const mockDestroy = jest.fn();

jest.mock('chart.js', () => {
    class MockChart {
        constructor(canvas, config) {
            this.canvas = canvas;
            this.config = config;
            this.data = config.data;
            this.options = config.options;
        }

        update = mockUpdate;

        destroy = mockDestroy;
    }

    MockChart.register = jest.fn();

    return {
        Chart: MockChart,
        CategoryScale: {},
        LinearScale: {},
        BarElement: {},
        PointElement: {},
        LineElement: {},
        ArcElement: {},
        RadialLinearScale: {},
        BarController: {},
        LineController: {},
        PieController: {},
        DoughnutController: {},
        PolarAreaController: {},
        RadarController: {},
        Filler: {},
        Legend: {},
        Title: {},
        Tooltip: {},
    };
});

import GraphFactory from './GraphFactory';

const sampleData = {
    labels: ['Jan', 'Feb'],
    datasets: [{ label: 'A', data: [3, 5], backgroundColor: ['#111', '#222'] }],
};

describe('GraphFactory enter animation guard', () => {
    beforeEach(() => {
        jest.useFakeTimers();
        mockUpdate.mockClear();
        mockDestroy.mockClear();
    });

    afterEach(() => {
        jest.useRealTimers();
    });

    it('does not call chart.update during the enter animation window after create', () => {
        const { rerender } = render(
            <GraphFactory type="bar" data={sampleData} text="Test chart" stack={false} />
        );

        expect(mockUpdate).not.toHaveBeenCalled();

        const sameContentNewReference = JSON.parse(JSON.stringify(sampleData));
        rerender(
            <GraphFactory type="bar" data={sameContentNewReference} text="Test chart" stack={false} />
        );

        act(() => {
            jest.advanceTimersByTime(CHART_ANIMATION.duration - 100);
        });

        expect(mockUpdate).not.toHaveBeenCalled();
    });

    it('defers chart.update until the enter guard expires when data changes during the window', () => {
        const { rerender } = render(
            <GraphFactory type="bar" data={sampleData} text="Test chart" stack={false} />
        );

        const updatedData = {
            labels: ['Jan', 'Feb'],
            datasets: [{ label: 'A', data: [8, 2], backgroundColor: ['#111', '#222'] }],
        };

        rerender(
            <GraphFactory type="bar" data={updatedData} text="Test chart" stack={false} />
        );

        act(() => {
            jest.advanceTimersByTime(CHART_ANIMATION.duration - 100);
        });
        expect(mockUpdate).not.toHaveBeenCalled();

        act(() => {
            jest.advanceTimersByTime(100);
        });
        expect(mockUpdate).toHaveBeenCalledTimes(1);
    });

    it('recreates the chart on type change and still avoids update during the enter window', () => {
        const { rerender } = render(
            <GraphFactory type="bar" data={sampleData} text="Test chart" stack={false} />
        );

        rerender(
            <GraphFactory type="pie" data={sampleData} text="Test chart" stack={false} />
        );

        expect(mockDestroy).toHaveBeenCalled();

        act(() => {
            jest.advanceTimersByTime(CHART_ANIMATION.duration - 50);
        });

        expect(mockUpdate).not.toHaveBeenCalled();
    });
});
