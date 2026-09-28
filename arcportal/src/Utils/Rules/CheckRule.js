

const CheckRule = (rule, data) => {

    const field = rule.Field ?? rule.field;
    const ruleType = rule.Rule ?? rule.rule;
    const ruleValue = rule.Value ?? rule.value;

    if (field === undefined || field === "") { return true; }

    const key = Object.keys(data).find(k => k.toLowerCase() === field.toLowerCase());

    if (key === undefined) { return ruleType !== "=" && ruleType !== "isnotempty" && ruleType !== "contains"; }

    const value = data[key];

    switch ((ruleType ?? "").toLowerCase()) {
        case "isnotempty":
            return value !== undefined && value !== null && value.toString() !== ""
        case "isempty":
            return value === undefined || value === null || value.toString() === "";
        case "=":
            return value !== undefined && value !== null && CompareValueString(value.toString(), ruleValue ?? "");
        case "!=":
            return value === undefined || value === null || value.toString() !== ruleValue;
        case "contains":
            return value !== undefined && value !== null && ValueContainsOption(value.toString(), ruleValue ?? "");
        default:
            return true;
    }
}

// A multi-select combobox stores its selection as a comma separated list, so a rule that gates on one of the
// chosen options has to split the field value rather than the rule value the way "=" does.
const ValueContainsOption = (valueToCompare, ruleValue) => {
    const selected = valueToCompare.split(",").map(v => v.trim());
    const wanted = ruleValue.split(",").map(v => v.trim());

    return wanted.some(want => selected.includes(want));
}

const CompareValueString = (valueToCompare, ruleValue) => {
    const values = ruleValue.split(",");

    for (const val of values) {
        if (val.trim() === valueToCompare) {
            return true;
        }
    }
    return false;
}

export default CheckRule;
