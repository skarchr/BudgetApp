(function() {
    'use strict';

    angular.module('budgetApp').controller('transactionController', ['$scope', 'transactionModel', function ($scope, transactionModel) {

        $scope.filter = transactionModel.filter;

        $scope.model = transactionModel.model;

        $scope.urls = transactionModel.urls;

        $scope.showBalance = false;

        $scope.calcPercentage = function(target, other) {

            var onePercent = (other + target)  / 100;

            var result = target / onePercent;

            return 'width:'+result.toFixed(0)+'%';

        };

    }]);

})();