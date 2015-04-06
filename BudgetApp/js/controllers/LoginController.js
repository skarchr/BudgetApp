(function() {
    'use strict';

    angular.module('budgetApp').controller('loginController', ['$scope', '$http', 'loginModel', function ($scope, $http, loginModel) {

        $scope.model = loginModel.model;

        $scope.countries = [];
        $scope.currencies = [];
        $scope.Country = '';
        $scope.Currency = '';
        $scope.passwordStrength = '';

        var containsCapitalLetter = function (input) {

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

        var evaluatePasswordStrength = function (input) {
            var strength = (input.length >= 6 ? 1 : 0);

            if (strength > 0) {
                strength += containsCapitalLetter($scope.model.Password) + containsNumber($scope.model.Password) + (input.length >= 8 ? 1 : 0);
            }

            $scope.passwordStrength = strength;
        }

        if (loginModel.model !== null && loginModel.model.hasOwnProperty('Password')) {
            evaluatePasswordStrength(loginModel.model.Password);
        }


        $scope.$watch('model.Password', function(newVal, oldVal) {

            if (newVal !== oldVal) {

                evaluatePasswordStrength(newVal);
            }
           
        });

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