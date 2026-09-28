const ApplyButtonRule = (button,filterValues) => {

    let matched = true;

    if (filterValues !== undefined) {
        if (button.Rules !== undefined && button.Rules !== "") {
            for (const rule of button.Rules) {
                const match = filterValues.filter((r) => rule.Field.toLowerCase() === r.FieldName.toLowerCase() )
                let foundValue = true;
                if (match.length > 0) {
                    if (match[0].values === undefined) {
                        switch (rule.Rule.toLowerCase()) {
                            case "isnotempty":
                                foundValue = false;
                                break;
                            case "=":
                                foundValue = false;
                                break;
                            case "!=":
                                foundValue = true;
                                break;
                            default:
                                foundValue = true;
                        }
                    } else {
                        const currentValues = match[0].values;

                        for (const value of currentValues) {
                            switch (rule.Rule.toLowerCase()) {
                                case "isnotempty":
                                    foundValue = foundValue && value.toString() !== "";
                                    break;
                                case "=":
                                    foundValue = foundValue && ApplyEqualsRule(rule.Value, value.toString());
                                    break;
                                case "!=":
                                    foundValue = foundValue && value.toString() !== rule.Value;
                                    break;
                                default:
                                    foundValue = foundValue && true;
                            }
                        }
                    }
                    matched = matched && foundValue;
                }
            }
        }
    }

    return matched;
}

const ApplyEqualsRule = (rule, value) => {

    if (rule.includes("|")) {
        const ruleValueList = rule.split("|");
        for (const item of ruleValueList) {
            if ( item === value) {
                return true;
            }
        }
    } else {
        return value === rule;
    }

}

export default ApplyButtonRule;