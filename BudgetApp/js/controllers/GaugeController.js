(function () {
    'use strict';

    angular.module('budgetApp').controller('gaugeController', ['$scope', function ($scope) {

        $scope.xOffset = 0;

        $scope.yOffset = 40;

        $scope.arrowWidth = 3;

        $scope.findXPos = function(income, expenses) {

            var x1 = income > expenses ? expenses : income;
            var x2 = income > expenses ? income : expenses;

            var midPoint = x1 + ((x2 - x1) / 2);

            return $scope.xOffset + midPoint;
        };

        $scope.findPlacement = function (income, expenses) {

            var x2 = income > expenses ? expenses : income;
            var x1 = income > expenses ? income : expenses;

            var difference = x1 - x2;

            if (difference > 40)
                return 'middle';

            return 'end';
        };

        $scope.findBalance = function (income, expenses) {

            var x1 = income > expenses ? expenses : income;

            return $scope.xOffset + x1;

        };

        $scope.findWidth = function(income, expenses) {
            var x1 = income > expenses ? expenses : income;
            var x2 = income > expenses ? income : expenses;

            return x2 - x1;
        };

        $scope.findColor = function(income, expenses) {            

            return income > expenses ? '#9cf5df' : '#f2aeae';
        };

    }]);


})();