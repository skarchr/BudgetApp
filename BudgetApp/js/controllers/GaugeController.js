(function () {
    'use strict';

    angular.module('budgetApp').controller('gaugeController', ['$scope', function ($scope) {

        $scope.xOffset = 0;

        $scope.yOffset = 40;

        $scope.arrowWidth = 4;

        var arrayGenerator = function(interval, amount) {

            var arr = new Array();

            var temp = 25;

            for (var i = 0; i <= amount; i++) {
                arr.push(temp);

                temp += interval;
            }

            return arr;

        };

        $scope.xLines = arrayGenerator(50, 10);

    }]);


})();