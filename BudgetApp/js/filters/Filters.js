(function() {
    'use strict';

    var shortMonthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    angular.module('budgetApp')
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