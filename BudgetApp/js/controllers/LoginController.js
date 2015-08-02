(function() {
    'use strict';

    angular.module('budgetApp').controller('loginController', ['$scope', '$http', '$timeout', 'loginModel', 'commonService', function ($scope, $http, $timeout, loginModel, commonService) {

        $scope.model = loginModel.model;


        $scope.countries = [];
        $scope.currencies = [];
        $scope.Country = '';
        $scope.Currency = '';
        $scope.passwordStrength = '';
        $scope.userExist = false;
        $scope.loading = false;

        $scope.getIcon = function (input) {

            return commonService.getIconUrl(input);

        };

        var checkEmail = function () {            

            setTimeout(function() {
                $http.get(loginModel.action.checkIfUserExist, { params: { id: $scope.model.Email } })
                    .success(function(data) {
                        $scope.userExist = data;
                        $scope.loading = false;
                    })
                    .error(function(data) {
                        console.log('CheckIfUserAlreadyExist: Failed');
                        $scope.loading = false;
                    });
            }, 500);
            
        };


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

        var evaluatePasswordStrength = function(input) {

            if (input === undefined || input.length <= 5) {
                $scope.passwordStrength = 0;
                return;
            }


            var strength = (input.length >= 6 ? 1 : 0);

            if (strength > 0) {
                strength += containsCapitalLetter($scope.model.Password) + containsNumber($scope.model.Password) + (input.length >= 12 ? 1 : 0);
            }

            $scope.passwordStrength = strength;
        };

        if (loginModel.model !== null && loginModel.model.hasOwnProperty('Password')) {
            evaluatePasswordStrength(loginModel.model.Password);
        }


        $scope.$watch('model.Password', function(newVal, oldVal) {

            if (newVal !== oldVal) {

                evaluatePasswordStrength(newVal);
            }
           
        });

        var timeoutPromise;
        $scope.$watch('model.Email', function (newVal, oldVal) {

            $scope.loading = true;

            if (newVal !== undefined) {

                $timeout.cancel(timeoutPromise); //does nothing, if timeout alrdy done
                timeoutPromise = $timeout(function() { //Set timeout                    
                    checkEmail();
                }, 500);

            } else {
                $scope.loading = false;
                $scope.userExist = 'False';
            }
        });

        $http.get('../../js/resources/countries.json')
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