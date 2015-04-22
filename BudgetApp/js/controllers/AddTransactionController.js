(function() {
    'use strict';

    angular.module('budgetApp').controller('addTransactionController', ['$scope', 'addTransactionModel', function ($scope, addTransactionModel) {

        $scope.model = addTransactionModel.model;

        $scope.regexDouble = /^(\d+(?:[\.\,]\d{1,2})?)$/;

    }]);

})();