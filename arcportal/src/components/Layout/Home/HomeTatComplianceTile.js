import React, { useEffect, useMemo, useState } from 'react';
import Post from '../../../Data/Post';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { buildTatComplianceParameters } from './buildDashboardQueryParameters';
import { HOMEDASHBOARD_TAT_COMPLIANCE_QUERY } from './homeDashboardUtils';
import {
    DEFAULT_KPI_FONT_SIZE_PX,
    KPI_FONT_SIZE_MAX,
    KPI_FONT_SIZE_MIN,
} from './homeKpiPalette';

const RAG_HEX = {
    Green: '#107c10',
    Amber: '#ca5010',
    Red: '#a4262c',
};

/**
 * @param {string} template
 * @param {string[]} parts
 */
function applyPlaceholders(template, parts) {
    let s = template;
    for (let i = 0; i < parts.length; i++) {
        s = s.split(`{${i}}`).join(parts[i] != null ? String(parts[i]) : '');
    }
    return s;
}

/**
 * Home dashboard TAT Compliance KPI: server aggregate via {@link HOMEDASHBOARD_TAT_COMPLIANCE_QUERY}.
 * @param {object} props
 * @param {object} props.section TAT Compliance section config.
 * @param {string} props.language Culture for translated strings.
 * @param {string} [props.homeAuthScopeKey] Toolbar lab/org scope; triggers refetch when JWT scope changes on Home.
 */
const HomeTatComplianceTile = (props) => {
    const { section, language, homeAuthScopeKey = '' } = props;
    const [data, setData] = useState(null);
    const requestKey = useMemo(
        () =>
            JSON.stringify({
                p: buildTatComplianceParameters(section),
                scope: homeAuthScopeKey ?? '',
            }),
        [section, homeAuthScopeKey]
    );

    useEffect(() => {
        const criteria = {
            Name: HOMEDASHBOARD_TAT_COMPLIANCE_QUERY,
            Parameters: buildTatComplianceParameters(section),
        };
        Post(
            'query/filteredget',
            criteria,
            (res) => {
                setData(res && typeof res === 'object' ? res : null);
            },
            () => setData(null)
        );
    }, [requestKey]);

    const pct = data?.PrimaryPercent ?? data?.primaryPercent;
    const onTime = data?.OnTimeCount ?? data?.onTimeCount ?? 0;
    const total = data?.TotalCount ?? data?.totalCount ?? 0;
    const late = data?.LateCount ?? data?.lateCount ?? 0;
    const roll = data?.RollingAveragePercent ?? data?.rollingAveragePercent;
    const rag = data?.Rag ?? data?.rag ?? 'Red';
    const headlineColor = RAG_HEX[rag] || RAG_HEX.Red;

    const lateH = section?.tatLateThresholdHours != null ? section.tatLateThresholdHours : 48;
    const avgDays = section?.tatRollingAverageDays != null ? section.tatRollingAverageDays : 0;

    const statTemplate = TranslateTag('@DasHomTatStat@', language) || '{0}/{1} on time, {2} > {3}h late';
    const statLine =
        total > 0
            ? applyPlaceholders(statTemplate, [String(onTime), String(total), String(late), String(lateH)])
            : '';

    const rollTemplate = TranslateTag('@DasHomTatAvgLine@', language) || '{0}-day avg: {1}%';
    const rollLine =
        avgDays > 0 && roll != null && roll !== undefined
            ? applyPlaceholders(rollTemplate, [String(avgDays), String(roll)])
            : '';

    const fontPx =
        section?.tatFontSizePx != null
            ? parseInt(String(section.tatFontSizePx), 10)
            : DEFAULT_KPI_FONT_SIZE_PX;
    const fontSize = Number.isFinite(fontPx)
        ? Math.min(KPI_FONT_SIZE_MAX, Math.max(KPI_FONT_SIZE_MIN, fontPx))
        : DEFAULT_KPI_FONT_SIZE_PX;
    const fontWeight = section?.tatBold !== false ? 700 : 400;

    return (
        <div className="home-dashboard-tile-inner home-tat-compliance-tile">
            <div className="home-tat-compliance-inner">
                <output
                    className="home-tat-compliance-value"
                    style={{ color: headlineColor, fontSize: `${fontSize}px`, fontWeight }}
                    aria-live="polite"
                >
                    {pct != null && pct !== undefined ? `${pct}%` : '—'}
                </output>
                {statLine ? <div className="home-tat-compliance-sub">{statLine}</div> : null}
                {rollLine ? <div className="home-tat-compliance-sub home-tat-compliance-roll">{rollLine}</div> : null}
            </div>
        </div>
    );
};

export default HomeTatComplianceTile;
