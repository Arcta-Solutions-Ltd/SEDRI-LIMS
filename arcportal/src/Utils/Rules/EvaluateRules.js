import CheckRule from './CheckRule';

const EvaluateRules = (type, rules, pageStructure) => {

    let result = true;

    if (rules === undefined) {
        return result;
    }

    for (const rule of rules){
        if ((rule.Effect ?? rule.effect) === type) {
            result = CheckRule(rule, pageStructure === undefined ? "{}" : pageStructure) && result;
        }
    }

    return result;
}

export default EvaluateRules;