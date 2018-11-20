(function() {

    function decimalAdjust(type, value, exp) {
        if (typeof exp === "undefined" || +exp === 0) {
            return NaN;
        }

        value = +value;
        exp = +exp;

        if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0)) {
            return NaN;
        }

        value = value.toString().split("e");
        value = Math[type](+(value[0] + "e" + (value[1] ? (+value[1] - exp) : -exp)));

        value = value.toString().split("e");
        return +(value[0] + "e" + (value[1] ? (+value[1] + exp) : exp));
    }

    //Decimal round
    if (!Math.round10) {
        Math.round10 = function(value, exp) {
            return decimalAdjust("round", value, exp);
        };
    }

    //Decimal floor
    if (!Math.floor10) {
        Math.floor10 = function(value, exp) {
            return decimalAdjust("floor", value, exp);
        };
    }

    //Decimal ceil
    if (!Math.ceil10) {
        Math.ceil10 = function(value, exp) {
            return decimalAdjust("ceil", value, exp);
        };
    }

})();