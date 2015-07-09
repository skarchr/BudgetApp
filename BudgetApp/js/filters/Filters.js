(function() {
    'use strict';

    var shortMonthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];


    var formatNumber = function (number) {

        if (number === null)
            return number;

        var numb = number.toFixed(2) + '';
        var x = numb.split(' ');
        var x1 = x[0];
        var x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ' ' + '$2');
        }
        return x1 + x2;
    };

    angular.module('budgetApp')

        .filter('currencyFormatter', function () {

            return function (input) {
                return formatNumber(input);
            }

        })

        .filter('monthName', function () {

            return function (input) {

                return shortMonthNames[input - 1];

            }

        })
        .filter('monthName2', function () {

            return function (input) {

                return shortMonthNames[new Date(input).getMonth()];

            }

        })
    .filter('camelCaseToNormal', function () {

        return function (input) {

            var result = input.replace(/([A-Z])/g, " $1");

            return (result.charAt(0).toUpperCase() + result.slice(1)).trim();

        }

    })
    .filter('maxLength', function () {

        return function (input, length) {

            if(input.length > length){

                var result = input.substring(0, length);

                return result + '...';
            }
            return input;

        }

    });

})();