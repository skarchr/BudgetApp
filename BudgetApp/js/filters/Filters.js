(function() {
    'use strict';

    angular.module('budgetApp')
        .filter('monthName', function () {

            return function (input) {

                var shortMonthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

                return shortMonthNames[input - 1];

            }

        });

})();