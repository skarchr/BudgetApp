(function() {
    'use strict';

    angular.module('budgetApp').controller('transactionController', ['$scope', 'transactionModel', function ($scope, transactionModel) {

        $scope.startDate = new Date(transactionModel.filter.startDate);
        $scope.endDate = new Date(transactionModel.filter.endDate);

        $scope.filter = transactionModel.filter;

        $scope.model = transactionModel.model;

        $scope.urls = transactionModel.urls;


    }]);

})();