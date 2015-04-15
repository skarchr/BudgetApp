(function() {
    'use strict';

    angular.module('budgetApp').controller('transactionController', ['$scope', 'transactionModel', function ($scope, transactionModel) {

        $scope.model = transactionModel.model;

    }]);

})();