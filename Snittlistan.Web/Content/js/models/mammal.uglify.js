// ReSharper disable UnusedParameter
(function (mammal_global, undefined) {
    // ReSharper restore UnusedParameter
    "use strict";

    var mammal = function (args) {
        var that = { };

        var get_name = function () {
            return args.name;
        };
        that.get_name = get_name;

        var says = function () {
            return args.saying || '';
        };
        that.says = says;

        return that;
    };

    mammal_global.mammal = mammal;

    return mammal_global;
})(window.Mammal = window.Mammal || { });
