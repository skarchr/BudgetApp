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

    }]);


})();