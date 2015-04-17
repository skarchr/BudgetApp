(function() {
    'use strict';

    angular.module('budgetApp')
        .filter('monthName', function () {

            return function (input) {

                var shortMonthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

                return shortMonthNames[input - 1];

            }

        })
    .filter('camelCaseToNormal', function () {

        return function (input) {

            var result = input.replace(/([A-Z])/g, " $1");

            return result.charAt(0).toUpperCase() + result.slice(1);

        }

    });

})();