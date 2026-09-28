import React, { useMemo } from 'react';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { useHomeGraphData } from './useHomeGraphData';
import { sumGraphDashboardCounts } from './homeGraphDataPipeline';
import { resolveKpiSwatchHex } from './homeKpiPalette';

/**
 * Single large numeric KPI for a home section: same graph query as {@link HomeGraphTile}, total = sum of reorganised counts.
 * @param {object} props
 * @param {string} [props.homeAuthScopeKey] Passed through to {@link useHomeGraphData} so KPI refetches after lab/org switch.
 */
const HomeKpiTile = (props) => {
    const { graphConfig, section, lists, language, homeAuthScopeKey = '' } = props;
    const { sourceData, filterState } = useHomeGraphData(graphConfig, section, lists, homeAuthScopeKey);

    const total = useMemo(() => {
        if (sourceData == null || filterState == null) {
            return null;
        }
        return sumGraphDashboardCounts(sourceData, filterState);
    }, [sourceData, filterState]);

    const colorHex = resolveKpiSwatchHex(section.kpiColorSwatchId);
    const fontSize = section.kpiFontSizePx != null ? section.kpiFontSizePx : 40;
    const fontWeight = section.kpiBold !== false ? 700 : 400;

    if (!graphConfig) {
        return <div className="home-dashboard-tile-message">{TranslateTag('@GraNo@', language)}</div>;
    }

    if (total === null) {
        return (
            <div className="home-dashboard-tile-message">
                {TranslateTag(graphConfig.Title, language) || graphConfig.Title}
            </div>
        );
    }

    return (
        <div className="home-dashboard-kpi-inner">
            <output
                className="home-dashboard-kpi-value"
                style={{ color: colorHex, fontSize: `${fontSize}px`, fontWeight }}
                aria-live="polite"
            >
                {total.toLocaleString()}
            </output>
        </div>
    );
};

export default HomeKpiTile;
