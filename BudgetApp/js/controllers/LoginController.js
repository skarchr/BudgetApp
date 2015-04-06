(function() {
    'use strict';

    angular.module('budgetApp').controller('loginController', ['$scope', '$http', function($scope, $http) {

        $scope.countries = [];
        $scope.currencies = [];
        $scope.country = '';
        $scope.currency = '';


        $scope.passwordStrength = '';


        $scope.$watch('password', function(newVal, oldVal) {

            if (newVal !== oldVal) {

                var strength = (newVal.length >= 6 ? 1 : 0);

                if (strength > 0) {
                    strength += containsCapitalLetter($scope.password) + containsNumber($scope.password) + (newVal.length >= 8 ? 1 : 0);
                }

                $scope.passwordStrength = strength;
            }
           
        });

        var containsCapitalLetter = function(input) {

            for (var i = 0; i < input.length; i++) {

                if (input[i].match(/^[A-Z]/))
                    return 1;

            }                        
            return 0;
        }

        var containsNumber = function (input) {

            for (var i = 0; i < input.length; i++) {

                if (input[i].match(/^[0-9]/))
                    return 1;

            }
            return 0;
        }


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

    }]);

})();