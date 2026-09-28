import React, { useMemo, useRef } from 'react';
import GraphFactory from '../../Containers/Graph/GraphFactory';
import FormatDataIntoGraph from '../../Containers/Graph/GraphUtils';
import { GRAPH_BACKGROUND_COLORS } from '../../Containers/Graph/graphBackgroundPalette';
import { chartPropsFromGraphtypeKey } from '../../Containers/Graph/graphGraphtypeMapping';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { useHomeGraphData } from './useHomeGraphData';
import { getFirstGraphtypeId } from './homeGraphFilterUi';

/**
 * @param {object} props
 * @param {string} [props.homeAuthScopeKey] Passed through to {@link useHomeGraphData} so graphs refetch after lab/org switch.
 */
const HomeGraphTile = (props) => {
    const { graphConfig, section, lists, language, homeAuthScopeKey = '' } = props;
    const graphElement = useRef(null);
    const { threeDimensions, twoDimensions, sourceData } = useHomeGraphData(
        graphConfig,
        section,
        lists,
        homeAuthScopeKey
    );

    const { type: vizType, stack } = useMemo(() => {
        const hasGraphtypeFilter = graphConfig?.Filters?.some((f) => f.Key === 'graphtype');
        const graphtypeId = getFirstGraphtypeId(section?.filters?.graphtype);
        if (hasGraphtypeFilter && graphtypeId) {
            return chartPropsFromGraphtypeKey(graphtypeId);
        }
        return { type: graphConfig?.Type, stack: graphConfig?.Stack };
    }, [graphConfig, section?.filters?.graphtype]);

    const graphData = useMemo(() => {
        if (!threeDimensions || !twoDimensions || !graphConfig) return null;
        if (!sourceData || sourceData.length === 0) return null;
        return FormatDataIntoGraph(threeDimensions, twoDimensions, GRAPH_BACKGROUND_COLORS, vizType);
    }, [threeDimensions, twoDimensions, sourceData, graphConfig, vizType]);

    if (!graphConfig) {
        return <div className="home-dashboard-tile-message">{TranslateTag('@GraNo@', language)}</div>;
    }

    if (!graphData) {
        return (
            <div className="home-dashboard-tile-message">
                {TranslateTag(graphConfig.Title, language) || graphConfig.Title}
            </div>
        );
    }

    return (
        <div className="home-dashboard-graph-wrap">
            <div className="home-dashboard-graph-canvas">
                <GraphFactory
                    id={`home-graph-${graphConfig.Name}`}
                    ref={graphElement}
                    data={graphData}
                    text={graphConfig.Title}
                    type={vizType}
                    stack={stack}
                    fillContainer
                />
            </div>
        </div>
    );
};

export default HomeGraphTile;
