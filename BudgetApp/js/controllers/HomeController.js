(function() {
    'use strict';

    angular.module('budgetApp').controller('homeController', ['$scope', 'homeViewModel', function ($scope, homeViewModel) {

        $scope.model = homeViewModel.model;

    }]);

})();