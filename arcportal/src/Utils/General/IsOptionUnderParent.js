import StringUtils from './StringUtils';

/**
 * Decides whether an option of a child list should be offered under the currently selected parent.
 *
 * An option's ParentKey holds the comma separated ids of its parents, aggregated from
 * listitemparentchild. It is null when the entry was saved without a parent, which table maintenance
 * allows for cross-list tables. An unparented entry is unrestricted, so it is offered under every
 * parent rather than being filtered out.
 *
 * @param {{ParentKey?: *, parentkey?: *, parentKey?: *}} option - Option from a list configuration.
 * @param {*} parentValue - Id of the selected parent.
 * @returns {boolean} True when the option belongs under that parent.
 */
const IsOptionUnderParent = (option, parentValue) => {
    const parents = option?.ParentKey ?? option?.parentkey ?? option?.parentKey;

    if (parents === null || parents === undefined || parents === '') {
        return true;
    }

    return StringUtils.containsString(String(parents), parentValue);
};

export default IsOptionUnderParent;
