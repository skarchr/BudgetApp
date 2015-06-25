(function () {
    'use strict';
    angular.module('budgetApp').controller('currencyController', ['$scope', '$http', 'toastr', 'userModel', 'commonService', function ($scope, $http, toastr, userModel, commonService) {

        $scope.dirty = false;

        $scope.confirmDelete = '';

        $scope.regexDouble = /^(\d+(?:[\.\,]\d{1,2})?)$/;

        $http.get('/js/resources/countries.json')
            .success(function (data) {
                $scope.countries = data;
            })
            .error(function (data) {
                $scope.countries = [];
            });

        $scope.getIcon = function (input) {

            return commonService.getIconUrl(input);

        };

        $http.get('/js/resources/currencies.json')
            .success(function (data) {
                $scope.currencies = data;                
            })
            .error(function (data) {
                $scope.currencies = [];
            });

        $scope.$watchGroup(['currencies', 'countries'], function(newVal, oldVal) {

            if (newVal[0] !== oldVal[0] && newVal[1] !== oldVal[1]) {
                $scope.model = userModel.model;                
            }
                               
        });


        $scope.saveProfile = function() {
            if ($scope.model.currency !== undefined && $scope.model.country !== undefined) {

                $http.post('../Manage/SaveProfile', {
                        currency: $scope.model.currency,
                        country: $scope.model.country,
                        range: $scope.model.range,
                        monthlyExpensesGoal: $scope.model.monthlyExpensesGoal,
                        monthlySavingGoal: $scope.model.monthlySavingGoal
                    })
                    .success(function(data, status, headers, config) {
                        toastr.success('Profile updated!');
                        $scope.dirty = false;
                    })
                    .error(function (data, status, headers, config) {
                        toastr.error('Something went wrong!');
                    });
            }

        };

        


    }]);

})();