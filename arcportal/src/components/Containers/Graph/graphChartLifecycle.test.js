import {
    buildChartSignature,
    createEnterGuardUntil,
    getDeferredUpdateDelayMs,
    sectionFiltersForDataFetch,
} from './graphChartLifecycle';
import { CHART_ANIMATION } from './chartDefaultOptions';

describe('graphChartLifecycle', () => {
    it('buildChartSignature is stable for identical content', () => {
        const data = { labels: ['A'], datasets: [{ data: [1] }] };
        const options = { responsive: true };
        expect(buildChartSignature(data, options)).toBe(buildChartSignature(data, options));
    });

    it('createEnterGuardUntil covers the full enter animation window', () => {
        const now = 1_000_000;
        expect(createEnterGuardUntil(now)).toBe(now + CHART_ANIMATION.duration);
    });

    it('getDeferredUpdateDelayMs returns remaining guard time', () => {
        const now = 1_000_000;
        const guardUntil = now + 750;
        expect(getDeferredUpdateDelayMs(guardUntil, now)).toBe(750);
        expect(getDeferredUpdateDelayMs(guardUntil, now + 750)).toBe(0);
        expect(getDeferredUpdateDelayMs(guardUntil, now + 900)).toBe(0);
    });

    it('sectionFiltersForDataFetch removes graphtype only', () => {
        expect(sectionFiltersForDataFetch({ graphtype: '483', stateid: '1' })).toEqual({ stateid: '1' });
        expect(sectionFiltersForDataFetch(undefined)).toBeUndefined();
    });
});
