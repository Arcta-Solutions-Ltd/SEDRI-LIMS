const FormatDecimalForMask = (number, precision, scale) => {
    var result = number.toString();
    var unit = result;
    var unitDigits = precision - scale;
    var mantissa = "";
    if (result.includes(".")) {
        var components = result.split(".");
        unit = components[0];
        mantissa = components[1];
    }
    if (unit.length < unitDigits) {
        var requiredPadding = unitDigits - unit.length;
        for (var i = 0; i < requiredPadding; i++) {
            unit = "0" + unit;
        }
    }
    if (mantissa.length < scale) {
        var requiredPadding = scale - mantissa.length;
        for (var i = 0; i < requiredPadding; i++) {
            mantissa = mantissa + "0";
        }
    }
    return unit + "." + mantissa;;
}

export default FormatDecimalForMask;
