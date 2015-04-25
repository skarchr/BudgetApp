(function() {
    'use strict';


    angular.module('budgetApp').controller('importController', ['$scope', 'importModel', function($scope, importModel) {

        $scope.model = importModel.model;

        $scope.regexDouble = /^(\d+(?:[\.\,]\d{1,2})?)$/;

    }]);

})();