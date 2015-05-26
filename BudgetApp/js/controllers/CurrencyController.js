(function () {
    'use strict';
    angular.module('budgetApp').controller('currencyController', ['$scope', '$http', 'userModel', function ($scope, $http, userModel) {

        $http.get('/js/resources/countries.json')
            .success(function (data) {
                $scope.countries = data;
            })
            .error(function (data) {
                $scope.countries = [];
            });



        $http.get('/js/resources/currencies.json')
            .success(function (data) {
                $scope.currencies = data;                
            })
            .error(function (data) {
                $scope.currencies = [];
            });

        $scope.$watchGroup(['currencies', 'countries'], function(newVal, oldVal) {

            if (newVal[0] !== oldVal[0] && newVal[1] !== oldVal[1])
                $scope.model = userModel.model;

        });


    }]);

})();