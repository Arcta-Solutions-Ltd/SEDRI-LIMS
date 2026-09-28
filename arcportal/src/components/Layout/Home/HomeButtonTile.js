import React, { useEffect, useMemo, useState } from 'react';
import Post from '../../../Data/Post';
import HomeCardItem from './HomeCardItem/HomeCardItem';
import './HomeButtonTile.css';
import { buttonKindToQueryName } from './homeDashboardUtils';
import { buildDashboardQueryParameters } from './buildDashboardQueryParameters';

/**
 * Summary counts for specimen state / type / tags on the home dashboard.
 * @param {object} props
 * @param {object} props.section Normalized home section (button kind, filters, timerange).
 * @param {function} props.onCardClick Card click handler.
 * @param {string} [props.homeAuthScopeKey] Toolbar lab/org scope (e.g. `L1`/`O2`); when it changes, data is refetched for the new JWT scope.
 */
const HomeButtonTile = (props) => {
    const { section, onCardClick, homeAuthScopeKey = '' } = props;
    const [data, setData] = useState([]);

    const requestKey = useMemo(
        () =>
            JSON.stringify({
                q: buttonKindToQueryName(section.buttonKind),
                p: buildDashboardQueryParameters(section),
                scope: homeAuthScopeKey ?? '',
            }),
        [section, homeAuthScopeKey]
    );

    useEffect(() => {
        const name = buttonKindToQueryName(section.buttonKind);
        const criteria = { Name: name, Parameters: buildDashboardQueryParameters(section) };
        Post('query/filteredget', criteria, (res) => setData(res || []), () => setData([]));
    }, [requestKey]);

    return (
        <div className="home-dashboard-tile-inner">
            <div className="home-button-tile-list">
                {data.length > 0 &&
                    data.map((field, index) => (
                        <HomeCardItem
                            key={field.Id ?? index}
                            type={section.buttonKind}
                            value={field.Value}
                            id={field.Id}
                            number={field.Number}
                            onClick={onCardClick}
                        />
                    ))}
            </div>
        </div>
    );
};

export default HomeButtonTile;
