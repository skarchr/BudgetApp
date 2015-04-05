(function() {
    'use strict';

    angular.module('budgetApp').controller('loginController', ['$scope', '$http', function($scope, $http) {

        $scope.countries = [];
        $scope.country = '';

        $http.get('/js/resources/countries.json')
            .success(function (data) {
                $scope.countries = data;
            })
            .error(function (data) {
                $scope.countries = [];
            });

    }]);

})();